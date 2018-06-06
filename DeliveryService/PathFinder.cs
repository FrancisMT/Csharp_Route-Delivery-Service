using System;
using System.Collections.Generic;
using System.Linq;
using DeliveryService.Models;

namespace DeliveryService
{
    /// <summary>
    /// Dijkstra's algorithm is used for finding
    /// the shipping path that minimizes either delivery time or cost.
    /// </summary>
    public class PathFinder
    {
        private readonly bool preferCheaperDelivery;

        private readonly Point shippingStartPoint;
        private readonly Point shippingEndPoint;

        private readonly List<Point> shippingPoints;
        private readonly List<Route> shippingRoutes;

        /// <summary>
        /// Visited nodes will not be visited twice. 
        /// </summary>
        private HashSet<Point> visitedPoints;
        private HashSet<Point> unvisitedPoints;

        /// <summary>
        /// Used as a resource to "backtrack" the path from the end point to the starting point.
        /// </summary>
        private Dictionary<Point, Point> pointPredecessors;

        /// <summary>
        /// Can be either time or cost.
        /// </summary>
        private Dictionary<Point, float> pointDistances;

        /// <summary>
        /// Can be either time or cost.
        /// </summary>
        private Dictionary<Point, float> pointAlternativeDistances;

        public PathFinder(string startPointName, string endPointName, bool saveMoney = true)
        {
            preferCheaperDelivery = saveMoney;

            var dbBridge = new DatabaseBridge();
            shippingPoints = dbBridge.ReadPoints();
            shippingRoutes = dbBridge.ReadRoutes();

            shippingStartPoint = shippingPoints.FirstOrDefault(pt => pt.Name == startPointName);
            shippingEndPoint = shippingPoints.FirstOrDefault(pt => pt.Name == endPointName);

            if (shippingStartPoint == null || shippingEndPoint == null)
            {
                throw new System.Exception("Invalid Path Endpoints");
            }
        }

        /// <summary>
        /// Apply Dijkstra's algorithm.
        /// </summary>
        public void FindShortestPaths()
        {
            visitedPoints = new HashSet<Point>();
            unvisitedPoints = new HashSet<Point>();

            pointPredecessors = new Dictionary<Point, Point>();
            pointDistances = new Dictionary<Point, float>();
            pointAlternativeDistances = new Dictionary<Point, float>();

            // Set tentative distances of the initial point to 0 (zero). 
            pointDistances.Add(shippingStartPoint, 0);
            pointAlternativeDistances.Add(shippingStartPoint, 0);
            unvisitedPoints.Add(shippingStartPoint);

            while (unvisitedPoints.Count() > 0)
            {
                var point = getPointWithMinimumTentativeDistance(unvisitedPoints);

                visitedPoints.Add(point);
                unvisitedPoints.Remove(point);

                findMinimalDistancesToNeighbors(point);
            }
        }

        /// <summary>
        /// For the current point, all of its unvisited neighbors are considered
        /// and their tentative distances through the current point are calculated.
        /// The newly calculated tentative distance are compared to the current assigned value
        /// and the smaller one is assigned.
        /// </summary>
        private void findMinimalDistancesToNeighbors(Point basePoint)
        {
            var neighborPoints = getNeighborPoints(basePoint);

            foreach (var neighborPoint in neighborPoints)
            {
                // Distance to neighborPoint through basePoint. 
                float distanceToNeighborThroughBasePoint = getPointShortestDistance(pointDistances, basePoint) + getDistanceBetweenPoints(basePoint, neighborPoint, preferCheaperDelivery);

                if (distanceToNeighborThroughBasePoint < getPointShortestDistance(pointDistances, neighborPoint))
                {
                    addOrUpdatePointDictionary(pointDistances, neighborPoint, distanceToNeighborThroughBasePoint);
                    addOrUpdatePointDictionary(pointPredecessors, neighborPoint, basePoint);

                    // Also save alternative distance.
                    {
                        float alternativeDistanceToNeighborThroughBasePoint = getPointShortestDistance(pointAlternativeDistances, basePoint) + getDistanceBetweenPoints(basePoint, neighborPoint, !preferCheaperDelivery);
                        addOrUpdatePointDictionary(pointAlternativeDistances, neighborPoint, alternativeDistanceToNeighborThroughBasePoint);
                    }

                    // Neighbor point is now ready to be visited.
                    unvisitedPoints.Add(neighborPoint);
                }
            }
        }

        private float getDistanceBetweenPoints(Point startPoint, Point endPoint, bool saveMoney)
        {
            float distanceBetweenPoints = 0.0f;

            foreach (var route in shippingRoutes)
            {
                if (route.StartPointID.Equals(startPoint.ID) && route.EndPointID.Equals(endPoint.ID))
                {
                    distanceBetweenPoints = saveMoney ? route.Cost : route.Time;
                    break;
                }
            }

            return distanceBetweenPoints;
        }

        private List<Point> getNeighborPoints(Point point)
        {
            var neighborPoints = new List<Point>();

            foreach (var route in shippingRoutes)
            {
                if (route.StartPointID.Equals(point.ID))
                {
                    var endPoint = shippingPoints.FirstOrDefault(pt => pt.ID == route.EndPointID);
                    if (endPoint != null && !wasPointAlreadyVisited(endPoint))
                    {
                        neighborPoints.Add(endPoint);
                    }
                }
            }

            return neighborPoints;
        }

        private Point getPointWithMinimumTentativeDistance(HashSet<Point> points)
        {
            Point minDistancePoint = null;

            foreach (var point in points)
            {
                if (minDistancePoint == null)
                {
                    minDistancePoint = point;
                }
                else if (getPointShortestDistance(pointDistances, point) < getPointShortestDistance(pointDistances, minDistancePoint))
                {
                    minDistancePoint = point;
                }
            }
            return minDistancePoint;
        }

        private bool wasPointAlreadyVisited(Point point)
        {
            return visitedPoints.Contains(point);
        }

        /// <summary>
        /// All points have a tentative distance value equal to zero for the initial point and infinity for all other points.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Point distance</returns>
        private float getPointShortestDistance(Dictionary<Point, float> pointDictionary, Point point)
        {
            float pointDistance = -1.0f;

            if (pointDictionary.TryGetValue(point, out pointDistance))
            {
                return pointDistance;
            }
            else
            {
                // Big enough number to be treated as infinity.
                return float.MaxValue;
            }
        }

        /// <summary>
        /// Get the delivery route (path from the starting point to the end point of the delivery).
        /// </summary>
        /// <returns>Delivery route point list  or null if no path exists.</returns>
        public List<Point> GetDeliveryRoutePoints()
        {
            var pointsPath = new List<Point>();
            var pointToBackTrack = shippingEndPoint;

            bool pathExists = pointPredecessors.TryGetValue(pointToBackTrack, out Point pathValidationPoint);
            if (!pathExists)
            {
                // No path exists for this point.
                return null;
            }

            pointsPath.Add(pointToBackTrack);
            while (pointPredecessors.TryGetValue(pointToBackTrack, out pointToBackTrack))
            {
                pointsPath.Add(pointToBackTrack);
            }

            // Put path in the correct order.
            pointsPath.Reverse();
            return pointsPath;
        }
        
        /// <summary>
        /// Return the distances of the shipping endpoint.
        /// </summary>
        /// <returns>end distance and alternative end distance</returns>
        public Tuple<float, float> GetEndPointDistances()
        {
            pointDistances.TryGetValue(shippingEndPoint, out float endDistance);
            pointAlternativeDistances.TryGetValue(shippingEndPoint, out float alternativeEndDistance);
            return new Tuple<float, float>(endDistance, alternativeEndDistance);
        }

        private void addOrUpdatePointDictionary<T>(Dictionary<Point, T> pointDictionary, Point point, T newValue)
        {
            if (pointDictionary.TryGetValue(point, out T valueInDictionary))
            {
                // Value exists! Update.
                pointDictionary[point] = newValue;
            }
            else
            {
                // Value doesn't exist! Add.
                pointDictionary.Add(point, newValue);
            }
        }
    }
}