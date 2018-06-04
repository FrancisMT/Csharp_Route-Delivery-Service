using System.Net;
using System.Net.Http;
using System.Web.Http;
using DeliveryService.Models;
using System.Collections;
using System.Collections.Generic;

namespace DeliveryService.Controllers
{
    public class PointController : ApiController
    {
        // GET: api/Point
        public List<Point> Get()
        {
            var dbBridge = new DatabaseBridge();
            return dbBridge.ReadPoints();
        }

        // GET: api/Point/5
        public Point Get(int id)
        {
            var dbBridge = new DatabaseBridge();
            return dbBridge.ReadPoint(id);
        }

        // POST: api/Point
        public HttpResponseMessage Post([FromBody]Point value)
        {
            var dbBridge = new DatabaseBridge();

            bool pointCreated = dbBridge.CreatePoint(value);

            return pointCreated ? Request.CreateResponse(HttpStatusCode.Created) : Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        // PUT: api/Point/5
        public HttpResponseMessage Put(long id, [FromBody]Point value)
        {
            var dbBridge = new DatabaseBridge();

            bool pointUpdated = dbBridge.UpdatePoint(id, value);

            return pointUpdated ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // DELETE: api/Point/5
        public HttpResponseMessage Delete(int id)
        {
            var dbBridge = new DatabaseBridge();

            bool pointDeleted = dbBridge.DeletePoint(id);
            
            return pointDeleted ? Request.CreateResponse(HttpStatusCode.OK) : Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
