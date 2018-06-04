using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DeliveryService.Models;

namespace DeliveryService.Controllers
{
    public class DeliveryRouteController : ApiController
    {
        // POST: api/DeliveryRoute
        public DeliveryRoute Post([FromBody]DeliveryRoute value)
        {
            try
            {
                var pathFinder = new PathFinder(value.StartPoint, value.EndPoint, /*save money*/value.PreferCheapestPath);
                pathFinder.FindShortestPaths();

                value.RoutePath = pathFinder.getDeliveryRoutePoints();

                var deliveryRouteDistances = pathFinder.getEndPointDistances();

                value.AverageTime = !value.PreferCheapestPath ? deliveryRouteDistances.Item1 : deliveryRouteDistances.Item2;
                value.AverageCost = value.PreferCheapestPath ? deliveryRouteDistances.Item1 : deliveryRouteDistances.Item2;

                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DeliveryRoute Exception: {0}", ex);
                return null;
            }
            
        }
    }
}
