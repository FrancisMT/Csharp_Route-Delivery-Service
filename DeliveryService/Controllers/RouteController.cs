using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DeliveryService.Models;


namespace DeliveryService.Controllers
{
    public class RouteController : ApiController
    {
        // GET: api/Route
        public List<Route> Get()
        {
            var dbBridge = new DatabaseBridge();
            return dbBridge.ReadRoutes();
        }

        // GET: api/Route/5
        public Route Get(int id)
        {
            var dbBridge = new DatabaseBridge();
            return dbBridge.ReadRoute(id);
        }

        // POST: api/Route
        public HttpResponseMessage Post([FromBody]Route value)
        {
            var dbBridge = new DatabaseBridge();

            bool RouteCreated = dbBridge.CreateRoute(value);

            return RouteCreated ? Request.CreateResponse(HttpStatusCode.Created) : Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        // PUT: api/Route/5
        public HttpResponseMessage Put(long id, [FromBody]Route value)
        {
            var dbBridge = new DatabaseBridge();

            bool RouteUpdated = dbBridge.UpdateRoute(id, value);

            return RouteUpdated ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // DELETE: api/Route/5
        public HttpResponseMessage Delete(int id)
        {
            var dbBridge = new DatabaseBridge();

            bool RouteDeleted = dbBridge.DeleteRoute(id);

            return RouteDeleted ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
