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
                var pathFinder = new PathFinder(value.StartPoint, value.EndPoint, /*save money*/false);
                pathFinder.FindShortestPaths();

                value.RoutePath = pathFinder.getDeliveryRoutePoints();

                return value;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}
