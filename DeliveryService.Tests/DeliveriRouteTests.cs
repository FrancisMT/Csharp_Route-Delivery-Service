using System;
using DeliveryService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DeliveryService.Tests
{
    [TestClass]
    public class DeliveriRouteTests
    {
        [TestMethod]
        public void GetCheapestRoute()
        {
            var dbBridge = new DatabaseBridge();

            // Add pointes
            {
                var pointList = new List<Point>
                {
                    new Point("A"),
                    new Point("B"),
                    new Point("C"),
                    new Point("D"),
                    new Point("E"),
                    new Point("F"),
                    new Point("G"),
                    new Point("H"),
                    new Point("I")
                };

                foreach (var point in pointList)
                {
                    dbBridge.CreatePoint(point);
                }

            }

            // Add routes
            {
                var routeList = new List<Route>
                {
                    new Route(20,1,1,3),
                    new Route(5,30,1,5),
                    new Route(1,10,1,8),
                    new Route(12,1,3,2),
                    new Route(1,30,8,5),
                    new Route(5,3,5,4),
                    new Route(50,4,4,6),
                    new Route(50,40,6,7),
                    new Route(50,45,6,9),
                    new Route(73,64,7,2),
                    new Route(5,65,9,2),
                };

                foreach (var route in routeList)
                {
                    dbBridge.CreateRoute(route);
                }
            }

            var deliveryRoute = new DeliveryRoute
            {
                PreferCheapestPath = true,
                StartPoint = "A",
                EndPoint = "E",
            };

            var pathFinder = new PathFinder(deliveryRoute.StartPoint, deliveryRoute.EndPoint, deliveryRoute.PreferCheapestPath);
            pathFinder.FindShortestPaths();

            var deliveryRouteDistances = pathFinder.getEndPointDistances();

            deliveryRoute.RoutePath = pathFinder.getDeliveryRoutePoints();
            deliveryRoute.AverageCost = deliveryRouteDistances.Item1;
            deliveryRoute.AverageTime = deliveryRouteDistances.Item2;

            Assert.IsTrue(deliveryRoute.AverageCost < deliveryRoute.AverageTime);
        }

        [TestMethod]
        public void GetFastestRoute()
        {
            var dbBridge = new DatabaseBridge();

            // Add pointes
            {
                var pointList = new List<Point>
                {
                    new Point("A"),
                    new Point("B"),
                    new Point("C"),
                    new Point("D"),
                    new Point("E"),
                    new Point("F"),
                    new Point("G"),
                    new Point("H"),
                    new Point("I")
                };

                foreach (var point in pointList)
                {
                    dbBridge.CreatePoint(point);
                }

            }

            // Add routes
            {
                var routeList = new List<Route>
                {
                    new Route(20,1,1,3),
                    new Route(5,30,1,5),
                    new Route(1,10,1,8),
                    new Route(12,1,3,2),
                    new Route(1,30,8,5),
                    new Route(5,3,5,4),
                    new Route(50,4,4,6),
                    new Route(50,40,6,7),
                    new Route(50,45,6,9),
                    new Route(73,64,7,2),
                    new Route(5,65,9,2),
                };

                foreach (var route in routeList)
                {
                    dbBridge.CreateRoute(route);
                }
            }

            var deliveryRoute = new DeliveryRoute
            {
                PreferCheapestPath = false,
                StartPoint = "A",
                EndPoint = "E",
            };

            var pathFinder = new PathFinder(deliveryRoute.StartPoint, deliveryRoute.EndPoint, deliveryRoute.PreferCheapestPath);

            pathFinder.FindShortestPaths();

            var deliveryRouteDistances = pathFinder.getEndPointDistances();

            deliveryRoute.RoutePath = pathFinder.getDeliveryRoutePoints();
            deliveryRoute.AverageCost = deliveryRouteDistances.Item1;
            deliveryRoute.AverageTime = deliveryRouteDistances.Item2;

            Assert.IsTrue(deliveryRoute.AverageTime < deliveryRoute.AverageCost);

        }

    }
}
