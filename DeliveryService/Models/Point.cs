namespace DeliveryService.Models
{
    public class Point
    {
        public Point()
        {
            ID = -1;
            Name = null;
        }

        public Point(string name)
        {
            Name = name;
        }

        public Point(long id, string name)
        {
            ID = id;
            Name = name;
        }

        public long ID { get; set; }
        public string Name { get; set; }
    }
}