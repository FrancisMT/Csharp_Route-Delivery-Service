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

        private readonly Point shippintStartPoint;
        private readonly Point shippintEndPoint;

        private readonly List<Point> shippingPoints;
        private readonly List<Route> shippingRoutes;

        /// <summary>
        /// Visited nodes will not be visited twice. 
        /// </summary>
        private HashSet<Point> visitedPoints;
        private HashSet<Point> unvisitedPoints;

        /// <summary>
        /// Used as a resource to "backtrack" the path from the start to the end point.
        /// </summary>
        private Dictionary<Point, Point> pointPredecessors;
        private Dictionary<Point, float> pointDistances;

        public PathFinder(string startPointName, string endPointName, bool saveMoney = true)
        {
            preferCheaperDelivery = saveMoney;

            var dbBridge = new DatabaseBridge();
            shippingPoints = dbBridge.ReadPoints();
            shippingRoutes = dbBridge.ReadRoutes();

            shippintStartPoint = shippingPoints.FirstOrDefault(pt => pt.Name == startPointName);
            shippintEndPoint = shippingPoints.FirstOrDefault(pt => pt.Name == endPointName);

            if (shippintStartPoint == null || shippintEndPoint == null)
            {
                throw new System.Exception("Invalid Path Endpoints");
            }
        }

        /// <summary>
        /// Apply the Dijkstra's algorithm.
        /// </summary>
        public void FindShortestPaths()
        {
            visitedPoints = new HashSet<Point>();
            unvisitedPoints = new HashSet<Point>();

            pointPredecessors = new Dictionary<Point, Point>();
            pointDistances = new Dictionary<Point, float>();

            // Set tentative distance of the initial point to 0 (zero). 
            pointDistances.Add(shippintStartPoint, 0);
            unvisitedPoints.Add(shippintStartPoint);

            while (unvisitedPoints.Count() > 0)
            {
                var point = getPointWithMinimumTentativeDistance(unvisitedPoints);

                visitedPoints.Add(point);
                unvisitedPoints.Remove(point);
                findMinimalDistancesToNeighbors(point);
            }
        }

        /// <summary>
        /// For the current point, consider all of its unvisited neighbors
        /// and calculate their tentative distances through the current point.
        /// Compare the newly calculated tentative distance to the current assigned value
        /// and assign the smaller one.
        /// </summary>
        private void findMinimalDistancesToNeighbors(Point basePoint)
        {
            var neighborPoints = getNeighborPoints(basePoint);

            foreach (var neighborPoint in neighborPoints)
            {
                //distance to B through A 

                float distanceToNeighborThroughBasePoint = getPointShortestDistance(basePoint) + getDistanceBetweenPoints(basePoint, neighborPoint);

                if (distanceToNeighborThroughBasePoint < getPointShortestDistance(neighborPoint))
                {
                    pointDistances.Add(neighborPoint, distanceToNeighborThroughBasePoint);
                    pointPredecessors.Add(neighborPoint, basePoint);

                    // Neighbor point is now ready to be visited.
                    unvisitedPoints.Add(neighborPoint);
                }
            }
        }

        private float getDistanceBetweenPoints(Point startPoint, Point endPoint)
        {
            float distanceBetweenPoints = 0.0f;

            foreach (var route in shippingRoutes)
            {
                if (route.StartPointID.Equals(startPoint.ID) && route.EndPointID.Equals(endPoint.ID))
                {
                    distanceBetweenPoints = preferCheaperDelivery ? route.Cost : route.Time;
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
                else if (getPointShortestDistance(point) < getPointShortestDistance(minDistancePoint))
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
        /// All points have a tentative distance value: zero for the initial point and infinity for all other points.
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Point distance</returns>
        private float getPointShortestDistance(Point point)
        {
            float pointDistance = -1.0f;

            if (pointDistances.TryGetValue(point, out pointDistance))
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
        /// Get the delivery route (path from the start to the end point of the delivery).
        /// </summary>
        /// <returns>Delivery route point list  or null if no path exists.</returns>
        public List<Point> getDeliveryRoutePoints()
        {
            var pointsPath = new List<Point>();
            var pointToBackTrack = shippintEndPoint;

            bool pathExists = pointPredecessors.TryGetValue(pointToBackTrack, out Point pathValidationPoint);
            if (!pathExists)
            {
                // No path exists for this point.
                return null;
            }

            pointsPath.Add(pointToBackTrack);
            while (pointPredecessors.TryGetValue(pointToBackTrack, out Point backtrackValidationPoint))
            {
                pointPredecessors.TryGetValue(pointToBackTrack, out pointToBackTrack);
                pointsPath.Add(pointToBackTrack);
            }

            // Put path in the correct order.
            pointsPath.Reverse();
            return pointsPath;
        }
    }
}