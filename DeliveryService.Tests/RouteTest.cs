using System.Linq;
using DeliveryService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeliveryService.Tests
{
    [TestClass]
    public class RouteTest
    {
        [TestMethod]
        public void AddNewRoute()
        {
            var dbBridge = new DatabaseBridge();

            // Get points from the database.
            var points = dbBridge.ReadPoints();
            var pointWithLowestID = points.Min(pt => pt.ID);
            var pointWithHighestID = points.Max(pt => pt.ID);

            var routeToAdd = new Route
            {
                Cost = 42,
                Time = 42,
                StartPointID = pointWithLowestID,
                EndPointID = pointWithHighestID,
            };

            // Get routes from the database.
            var routes = dbBridge.ReadRoutes();
            var routeWithSameEndPoints = routes.FirstOrDefault(rt => rt.StartPointID == pointWithLowestID && rt.EndPointID == pointWithHighestID);

            // Try to create route.
            bool routeCreated = dbBridge.CreateRoute(routeToAdd);

            if (routeWithSameEndPoints == null)
            {
                // Route should be created successfully.
                Assert.IsTrue(routeCreated);
            }
            else
            {
                // Route should already exists.
                Assert.IsFalse(routeCreated);
            }
        }

        [TestMethod]
        public void GetRoute()
        {
            var dbBridge = new DatabaseBridge();

            // Get points from the database.
            var points = dbBridge.ReadPoints();
            var pointWithLowestID = points.Min(pt => pt.ID);
            var pointWithHighestID = points.Max(pt => pt.ID);

            var routeToAdd = new Route
            {
                Cost = 42,
                Time = 42,
                StartPointID = pointWithLowestID,
                EndPointID = pointWithHighestID,
            };

            // Try to create route.
            bool RouteCreated = dbBridge.CreateRoute(routeToAdd);

            // Get routes from the database.
            var routes = dbBridge.ReadRoutes();
            var routeWithSameEndPoints = routes.FirstOrDefault(rt => rt.StartPointID == pointWithLowestID && rt.EndPointID == pointWithHighestID);

            Assert.AreNotEqual(routeWithSameEndPoints, null);
        }

        [TestMethod]
        public void UpdateExistingRoute()
        {
            var dbBridge = new DatabaseBridge();

            // Get points from the database.
            var points = dbBridge.ReadPoints();
            var pointWithLowestID = points.Min(pt => pt.ID);
            var pointWithHighestID = points.Max(pt => pt.ID);

            // Get routes from the database.
            var routes = dbBridge.ReadRoutes();
            var routeWithMaxID = routes.Max(rt => rt.ID);

            var routeToUpdate = new Route
            {
                Cost = 24,
                Time = 24,
                StartPointID = pointWithLowestID,
                EndPointID = pointWithHighestID,
            };

            // Try to update route.
            var routeUpdated = dbBridge.UpdateRoute(routeWithMaxID, routeToUpdate);

            // Assert that the route was updated.
            Assert.IsTrue(routeUpdated);
        }

        [TestMethod]
        public void UpdateNonExistingRoute()
        {
            var dbBridge = new DatabaseBridge();

            // Get points from the database.
            var points = dbBridge.ReadPoints();
            var pointWithLowestID = points.Min(pt => pt.ID);
            var pointWithHighestID = points.Max(pt => pt.ID);

            // Get routes from the database.
            var routes = dbBridge.ReadRoutes();
            var routeWithMaxID = routes.Max(rt => rt.ID);

            var routeToUpdate = new Route
            {
                Cost = 24,
                Time = 24,
                StartPointID = pointWithLowestID,
                EndPointID = pointWithHighestID,
            };

            // Try to update a route with a non-existent ID.
            var routeUpdated = dbBridge.UpdateRoute(++routeWithMaxID, routeToUpdate);

            // Assert that the route was not updated.
            Assert.IsFalse(routeUpdated);
        }

        [TestMethod]
        public void DeleteExistingRoute()
        {
            var dbBridge = new DatabaseBridge();

            // Get routes from the database.
            var routes = dbBridge.ReadRoutes();
            var routeWithMaxID = routes.Max(rt => rt.ID);

            // Try to delete route.
            var routeDeleted = dbBridge.DeleteRoute(routeWithMaxID);

            // Assert that the route was deleted.
            Assert.IsTrue(routeDeleted);
        }

        [TestMethod]
        public void DeleteNonExistingRoute()
        {
            var dbBridge = new DatabaseBridge();

            // Get routes from the database.
            var routes = dbBridge.ReadRoutes();
            var routeWithMaxID = routes.Max(rt => rt.ID);

            // Try to delete a route with a non-existent ID.
            var routeDeleted = dbBridge.DeleteRoute(++routeWithMaxID);

            // Assert that the route was deleted.
            Assert.IsFalse(routeDeleted);
        }
    }
}
