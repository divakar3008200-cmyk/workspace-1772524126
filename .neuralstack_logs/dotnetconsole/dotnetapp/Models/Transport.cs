using System;

namespace dotnetapp.Models
{
    public class Transport
    {
        public int Id { get; set; }
        public string VehicleType { get; set; } // e.g., Bus, Truck, Car
        public string LicensePlate { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Capacity { get; set; } // number of passengers or payload capacity
        public string Status { get; set; } // e.g., Active, In Maintenance, Retired
        public DateTime LastMaintenanceDate { get; set; }
    }
}