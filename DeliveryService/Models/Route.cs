namespace DeliveryService.Models
{
    public class Route
    {
        public Route()
        {
            ID = -1;
            Cost = -1;
            Time = -1;
            StartPointID = -1;
            EndPointID = -1;
        }

        public Route(float cost, float time, long startPointID, long endPointID)
        {
            Cost = cost;
            Time = time;
            StartPointID = startPointID;
            EndPointID = endPointID;
        }

        public Route(long id, float cost, float time, long startPointID, long endPointID)
        {
            ID = id;
            Cost = cost;
            Time = time;
            StartPointID = startPointID;
            EndPointID = endPointID;
        }

        public long ID { get; set; }
        public float Cost { get; set; } // In €
        public float Time { get; set; } // In Seconds
        public long StartPointID { get; set; }
        public long EndPointID { get; set; }
    }
}