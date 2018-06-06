using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using DeliveryService.Models;

namespace DeliveryService
{
    public class DatabaseBridge
    {
        private MySql.Data.MySqlClient.MySqlConnection dbConnection;

        public DatabaseBridge()
        {
            string connectionString = "server=127.0.0.1;uid=Admin;pwd=unsafepassword;database=deliveryservice";

            try
            {
                dbConnection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);

                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                throw (ex);
            }
        }

        #region Points
        public bool CreatePoint(Point pointToCreate)
        {
            // Only create point if it doesn't already exist.
            if (ReadPoint(pointToCreate.ID) == null)
            {
                string createPointSqlQuery = "INSERT INTO points (POINT_NAME) VALUES('" + pointToCreate.Name + "')";
                var sqlCreatePointCommand = new MySql.Data.MySqlClient.MySqlCommand(createPointSqlQuery, dbConnection);

                try
                {
                    sqlCreatePointCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CreatePoint Exception: {0}", ex);
                    return false;
                }

                pointToCreate.ID = sqlCreatePointCommand.LastInsertedId;
                return true;
            }
            else
            {
                return false;
            }
        }

        public Point ReadPoint(long pointToReadID)
        {
            string readPointSqlQuery = "SELECT * FROM points WHERE POINT_ID = " + pointToReadID.ToString();
            var sqlReadPointCommand = new MySql.Data.MySqlClient.MySqlCommand(readPointSqlQuery, dbConnection);

            // Force the reader to be closed and disposed at the end of the scope.
            // Avoids using the same connection for both DataReader and ExecuteNonQuery (when applied).
            using (var mySQLReader = sqlReadPointCommand.ExecuteReader())
            {
                if (mySQLReader.Read())
                {
                    var pointToGet = new Point
                    {
                        ID = mySQLReader.GetInt32(0),
                        Name = mySQLReader.GetString(1),
                    };
                    return pointToGet;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Point> ReadPoints()
        {
            string readPointsSqlQuery = "SELECT * FROM points";
            var sqlReadPointsCommand = new MySql.Data.MySqlClient.MySqlCommand(readPointsSqlQuery, dbConnection);

            var pointList = new List<Point>();

            using (var mySQLReader = sqlReadPointsCommand.ExecuteReader())
            {
                while (mySQLReader.Read())
                {
                    Point pointToGet = new Point
                    {
                        ID = mySQLReader.GetInt32(0),
                        Name = mySQLReader.GetString(1),
                    };
                    pointList.Add(pointToGet);
                }

                return pointList;
            }
        }

        public bool UpdatePoint(long pointToUpdateID, Point pointToUpdate)
        {
            // Only update point if it already exists.
            if (ReadPoint(pointToUpdateID) != null)
            {
                string updatePointSqlQuery = "UPDATE points SET POINT_NAME='" + pointToUpdate.Name + "' WHERE  POINT_ID=" + pointToUpdateID.ToString();
                var sqlUpdatePointCommand = new MySql.Data.MySqlClient.MySqlCommand(updatePointSqlQuery, dbConnection);

                try
                {
                    sqlUpdatePointCommand.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("UpdatePoint Exception: {0}", ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool DeletePoint(long pointToDeleteID)
        {
            // Only delete point if it already exists.
            if (ReadPoint(pointToDeleteID) != null)
            {
                string deletePointSqlQuery = "DELETE FROM points WHERE POINT_ID = " + pointToDeleteID.ToString();
                var sqlDeleteCommand = new MySql.Data.MySqlClient.MySqlCommand(deletePointSqlQuery, dbConnection);

                try
                {
                    sqlDeleteCommand.ExecuteNonQuery();

                    // Delete routes connected to this point.
                    deleteRoutesConnectedToPoint(pointToDeleteID);

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DeletePoint Exception: {0}", ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Routes

        // Check if both start and end points of the route exist.
        private bool checkRouteEligibility(long StartPointID, long EndPointID)
        {
            bool
                startPointExists = !(ReadPoint(StartPointID) == null),
                endPointExists = !(ReadPoint(EndPointID) == null);

            return startPointExists && endPointExists;
        }

        public bool CreateRoute(Route RouteToCreate)
        {
            // Get routes from the database.
            var routes = ReadRoutes();
            var routeWithSameEndPoints = routes.FirstOrDefault(rt => rt.StartPointID == RouteToCreate.StartPointID && rt.EndPointID == RouteToCreate.EndPointID);

            bool
             routeExists = routeWithSameEndPoints != null,
             routeEligibleForCreation = checkRouteEligibility(RouteToCreate.StartPointID, RouteToCreate.EndPointID);

            // Only create Route if it doesn't exists and is eligible for creation.
            if (!routeExists && routeEligibleForCreation)
            {
                string createRouteSqlQuery = "INSERT INTO Routes (ROUTE_COST, ROUTE_TIME, START_POINT_ID, END_POINT_ID) VALUES('" + RouteToCreate.Cost + "','" + RouteToCreate.Time + "','" + RouteToCreate.StartPointID + "','" + RouteToCreate.EndPointID + "')";
                var sqlCreateRouteCommand = new MySql.Data.MySqlClient.MySqlCommand(createRouteSqlQuery, dbConnection);

                try
                {
                    sqlCreateRouteCommand.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CreateRoute Exception: {0}", ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Route ReadRoute(long RouteToReadID)
        {
            string readRouteSqlQuery = "SELECT * FROM Routes WHERE Route_ID = " + RouteToReadID.ToString();
            var sqlReadRouteCommand = new MySql.Data.MySqlClient.MySqlCommand(readRouteSqlQuery, dbConnection);

            using (var mySQLReader = sqlReadRouteCommand.ExecuteReader())
            {
                if (mySQLReader.Read())
                {
                    var RouteToGet = new Route
                    {
                        ID = mySQLReader.GetInt32(0),
                        Cost = mySQLReader.GetFloat(1),
                        Time = mySQLReader.GetFloat(2),
                        StartPointID = mySQLReader.GetInt32(3),
                        EndPointID = mySQLReader.GetInt32(4),
                    };
                    return RouteToGet;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Route> ReadRoutes()
        {
            string readRoutesSqlQuery = "SELECT * FROM Routes";
            var sqlReadRoutesCommand = new MySql.Data.MySqlClient.MySqlCommand(readRoutesSqlQuery, dbConnection);

            var RouteList = new List<Route>();

            using (var mySQLReader = sqlReadRoutesCommand.ExecuteReader())
            {
                while (mySQLReader.Read())
                {
                    Route RouteToGet = new Route
                    {
                        ID = mySQLReader.GetInt32(0),
                        Cost = mySQLReader.GetFloat(1),
                        Time = mySQLReader.GetFloat(2),
                        StartPointID = mySQLReader.GetInt32(3),
                        EndPointID = mySQLReader.GetInt32(4),
                    };
                    RouteList.Add(RouteToGet);
                }

                return RouteList;
            }
        }

        private List<Route> readRoutesConnectedToPoint(long PointID)
        {
            string readRoutesSqlQuery = "SELECT * FROM Routes WHERE START_POINT_ID='" + PointID + "' OR END_POINT_ID='" + PointID + "'";
            var sqlReadRoutesCommand = new MySql.Data.MySqlClient.MySqlCommand(readRoutesSqlQuery, dbConnection);

            var RouteList = new List<Route>();

            using (var mySQLReader = sqlReadRoutesCommand.ExecuteReader())
            {
                while (mySQLReader.Read())
                {
                    Route RouteToGet = new Route
                    {
                        ID = mySQLReader.GetInt32(0),
                        Cost = mySQLReader.GetFloat(1),
                        Time = mySQLReader.GetFloat(2),
                        StartPointID = mySQLReader.GetInt32(3),
                        EndPointID = mySQLReader.GetInt32(4),
                    };
                    RouteList.Add(RouteToGet);
                }

                return RouteList;
            }
        }

        public bool UpdateRoute(long RouteToUpdateID, Route RouteToUpdate)
        {
            bool
                routeExists = ReadRoute(RouteToUpdateID) != null,
                routeEligibleForUpdate = checkRouteEligibility(RouteToUpdate.StartPointID, RouteToUpdate.EndPointID);

            // Only update Route if it already exists and is eligible for update.
            if (routeExists && routeEligibleForUpdate)
            {
                string updateRouteSqlQuery = "UPDATE Routes SET ROUTE_COST='" + RouteToUpdate.Cost + "', ROUTE_TIME='" + RouteToUpdate.Time + "', START_POINT_ID='" + RouteToUpdate.StartPointID + "', END_POINT_ID='" + RouteToUpdate.EndPointID + "' WHERE  Route_ID=" + RouteToUpdateID.ToString();
                var sqlUpdateRouteCommand = new MySql.Data.MySqlClient.MySqlCommand(updateRouteSqlQuery, dbConnection);

                try
                {
                    sqlUpdateRouteCommand.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("UpdateRoute Exception: {0}", ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool DeleteRoute(long RouteToDeleteID)
        {
            // Only delete Route if it already exists.
            if (ReadRoute(RouteToDeleteID) != null)
            {
                string deleteRouteSqlQuery = "DELETE FROM Routes WHERE Route_ID = " + RouteToDeleteID.ToString();
                var sqlDeleteCommand = new MySql.Data.MySqlClient.MySqlCommand(deleteRouteSqlQuery, dbConnection);

                try
                {
                    sqlDeleteCommand.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DeleteRoute Exception: {0}", ex);
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        private void deleteRoutesConnectedToPoint(long PointID)
        {
            var routesToDelete = readRoutesConnectedToPoint(PointID);

            foreach (Route routeToDelete in routesToDelete)
            {
                DeleteRoute(routeToDelete.ID);
            }
        }

        #endregion
    }
}