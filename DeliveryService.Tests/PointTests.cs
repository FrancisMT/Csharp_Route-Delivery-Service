using System.Linq;
using DeliveryService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeliveryService.Tests
{
    [TestClass]
    public class PointTests
    {
        [TestMethod]
        public void AddNewPoint()
        {
            var dbBridge = new DatabaseBridge();

            const string pointName = "AddedPoint";
            Point pointToAdd = new Point
            {
                Name = pointName,
            };

            // Get points from the database.
            var points = dbBridge.ReadPoints();
            var pointWithSameName = points.FirstOrDefault(pt => pt.Name == pointName);

            // Try to add new point.
            bool pointCreated = dbBridge.CreatePoint(pointToAdd);

            if (pointWithSameName == null)
            {
                // Point should be created successfully.
                Assert.IsTrue(pointCreated);
            }
            else
            {
                // Point should already exists.
                Assert.IsFalse(pointCreated);
            }

        }

        [TestMethod]
        public void GetPoint()
        {
            var dbBridge = new DatabaseBridge();

            const string pointName = "PointToGet";
            Point pointToAdd = new Point
            {
                Name = pointName,
            };

            // Try to add new point.
            bool pointCreated = dbBridge.CreatePoint(pointToAdd);

            // Get points from the database.
            var points = dbBridge.ReadPoints();
            var pointWithSameName = points.FirstOrDefault(pt => pt.Name == pointName);

            // Assert that the point exists in the database,
            // either if the point was added during the test or if it existed in the database prior to the test.
            Assert.AreNotEqual(pointWithSameName, null);
        }

        [TestMethod]
        public void UpdateExistingPoint()
        {
            var dbBridge = new DatabaseBridge();

            // Get points from the database.
            var points = dbBridge.ReadPoints();
            // Get point with highest ID.
            var pointWithMaxID = points.Max(pt => pt.ID);

            Point pointToUpdate = new Point
            {
                Name = "UpdatedPointName",
            };

            // Try to update point.
            bool pointUpdated = dbBridge.UpdatePoint(pointWithMaxID, pointToUpdate);

            // Assert that the point was updated.
            Assert.IsTrue(pointUpdated);
        }

        [TestMethod]
        public void UpdateNonExistingPoint()
        {
            var dbBridge = new DatabaseBridge();

            // Get points from the database.
            var points = dbBridge.ReadPoints();
            // Get point with highest ID.
            var pointWithMaxID = points.Max(pt => pt.ID);

            Point pointToUpdate = new Point
            {
                Name = "UpdatedPointName",
            };

            // Try to update a point with non-existent ID.
            bool pointUpdated = dbBridge.UpdatePoint(++pointWithMaxID, pointToUpdate);

            // Assert that point was not updated.
            Assert.IsFalse(pointUpdated);
        }

        [TestMethod]
        public void DeleteExistingPoint()
        {
            var dbBridge = new DatabaseBridge();

            // Get points from the database.
            var points = dbBridge.ReadPoints();
            // Get point with highest ID.
            var pointWithMaxID = points.Max(pt => pt.ID);

            // Try to delete point.
            bool pointDeleted = dbBridge.DeletePoint(pointWithMaxID);

            // Assert that the point was updated.
            Assert.IsTrue(pointDeleted);
        }

        [TestMethod]
        public void DeleteNonExistingPoint()
        {
            var dbBridge = new DatabaseBridge();

            // Get points from the database.
            var points = dbBridge.ReadPoints();
            // Get point with highest ID.
            var pointWithMaxID = points.Max(pt => pt.ID);

            // Try to delete a point with a non-existent ID.
            bool pointDeleted = dbBridge.DeletePoint(++pointWithMaxID);

            // Assert that point was not deleted because it doesn't exits.
            Assert.IsFalse(pointDeleted);
        }
    }
}
