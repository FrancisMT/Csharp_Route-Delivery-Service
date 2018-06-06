using System;
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
                var pathFinder = new PathFinder(value.StartPoint, value.EndPoint, value.PreferCheapestPath);

                pathFinder.FindShortestPaths();

                var deliveryRouteDistances = pathFinder.GetEndPointDistances();

                value.RoutePath = pathFinder.GetDeliveryRoutePoints();
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
