using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeliveryService.Tests
{
    [TestClass]
    public class PointTests
    {
        [TestMethod]
        public void AddNewPoint()
        {
            // 
            var dbBridge = new DatabaseBridge();
           
            // Add new point

            // Assert that it was created successfully

            // Delete point from db
        }

        [TestMethod]
        public void AddSamePointTwice()
        {
            // 
            var dbBridge = new DatabaseBridge();

            // Add new point

            // Add same point again

            // Assert that it was not created successfully

            // Delete point from db
        }

        [TestMethod]
        public void GetSinglePoint()
        {
            // 
            var dbBridge = new DatabaseBridge();

            // Add new point


            // Get that same point

            // Assert that point created is equal to point received

            // Delete point from db
        }

        [TestMethod]
        public void GetAllPoints()
        {
            // 
            var dbBridge = new DatabaseBridge();

            // Add new point
            // Add new differente point
            // Add new differente point

            // Get all points

            // Assert that created points are equal to received points 

            // Delete points from db
        }

        [TestMethod]
        public void UpdateExistingPoint()
        {
            // 
            var dbBridge = new DatabaseBridge();

            // Add new point

            // Get all points and find the one with same point name and get it's id.

            // Update same Point

            // Assert that the point was updated

            // Delete point from db
        }

        [TestMethod]
        public void UpdateNonExistingPoint()
        {
            // 
            var dbBridge = new DatabaseBridge();

            // Get all points
            // Get point with highest ID

            // Update non existing point with pointID = ID + 1 (ensures that point doesn't exist)

            // Assert that point was not updated.
        }

        [TestMethod]
        public void DeleteExistingPoint()
        {
            // 
            var dbBridge = new DatabaseBridge();

            // Add point

            // Get all points and find the one with same point name and get it's id.

            // Delete point with the id from the previous querry

            // Assert that point was deleted.
        }

        [TestMethod]
        public void DeleteNonExistingPoint()
        {
            // 
            var dbBridge = new DatabaseBridge();

            // Add point

            // Get all points and find the one with the highest ID

            // Delete point with the id = ID + 1

            // Assert that point was deleted because it doesn't exits.
        }

    }
}
