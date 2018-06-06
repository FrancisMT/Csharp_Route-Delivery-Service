using System.Collections.Generic;

namespace DeliveryService.Models
{
    public class DeliveryRoute
    {
        public DeliveryRoute()
        {
            // The quickest path is chosen by default.
            PreferCheapestPath = false;

            StartPoint = null;
            EndPoint = null;

            RoutePath = null;
            AverageTime = -1;
            AverageCost = -1;
        }
        
        public DeliveryRoute(bool saveMoney, string sourceName, string destinationName)
        {
            PreferCheapestPath = saveMoney;

            StartPoint = sourceName;
            EndPoint = destinationName;

            RoutePath = null;
            AverageTime = -1;
            AverageCost = -1;
        }

        public bool PreferCheapestPath { get; set; }

        public string StartPoint { get; set; }
        public string EndPoint { get; set; }

        public List<Point> RoutePath { get; set; }
        public float AverageTime { get; set; }
        public float AverageCost { get; set; }
    }
}