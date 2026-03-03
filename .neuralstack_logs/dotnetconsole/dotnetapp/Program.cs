using System;
using System.Collections.Generic;
using System.Globalization;
using dotnetapp.Models;

namespace dotnetapp
{
    public class Program
    {
        private static List<Transport> transports = new List<Transport>();
        private static int nextId = 1;

        public static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== Transportation Management ===");
                Console.WriteLine("1. Create Transport");
                Console.WriteLine("2. List All Transports");
                Console.WriteLine("3. View Transport by Id");
                Console.WriteLine("4. Update Transport");
                Console.WriteLine("5. Delete Transport");
                Console.WriteLine("6. Search by License Plate");
                Console.WriteLine("7. Exit");
                Console.Write("Select an option: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1": CreateTransport(); break;
                    case "2": ListTransports(); break;
                    case "3": ViewTransport(); break;
                    case "4": UpdateTransport(); break;
                    case "5": DeleteTransport(); break;
                    case "6": SearchByLicensePlate(); break;
                    case "7": exit = true; break;
                    default: Console.WriteLine("Invalid option. Please try again."); break;
                }
            }

            Console.WriteLine("Exiting application. Goodbye!");
        }

        private static void CreateTransport()
        {
            Console.WriteLine("\n--- Create Transport ---");
            var t = new Transport();
            t.Id = nextId++;

            t.VehicleType = ReadRequiredString("Vehicle Type (e.g., Bus, Truck, Car): ");
            t.LicensePlate = ReadRequiredString("License Plate: ");
            t.Manufacturer = ReadRequiredString("Manufacturer: ");
            t.Model = ReadRequiredString("Model: ");
            t.Year = ReadIntInRange("Year: ", 1900, DateTime.Now.Year);
            t.Capacity = ReadIntInRange("Capacity (number of passengers / payload): ", 0, 100000);
            t.Status = ReadRequiredString("Status (Active, In Maintenance, Retired): ");
            t.LastMaintenanceDate = ReadDate("Last Maintenance Date (yyyy-MM-dd): ");

            transports.Add(t);
            Console.WriteLine($"Transport created with Id: {t.Id}");
        }

        private static void ListTransports()
        {
            Console.WriteLine("\n--- All Transports ---");
            if (transports.Count == 0)
            {
                Console.WriteLine("No transports found.");
                return;
            }

            foreach (var t in transports)
            {
                PrintTransportSummary(t);
            }
        }

        private static void ViewTransport()
        {
            Console.WriteLine("\n--- View Transport ---");
            int id = ReadInt("Enter Id: ");
            var t = transports.Find(x => x.Id == id);
            if (t == null)
            {
                Console.WriteLine("Transport not found.");
                return;
            }
            PrintTransportDetails(t);
        }

        private static void UpdateTransport()
        {
            Console.WriteLine("\n--- Update Transport ---");
            int id = ReadInt("Enter Id to update: ");
            var t = transports.Find(x => x.Id == id);
            if (t == null)
            {
                Console.WriteLine("Transport not found.");
                return;
            }

            Console.WriteLine("Leave blank to keep existing value.");
            string v;

            v = ReadStringAllowEmpty($"Vehicle Type ({t.VehicleType}): ");
            if (!string.IsNullOrWhiteSpace(v)) t.VehicleType = v;

            v = ReadStringAllowEmpty($"License Plate ({t.LicensePlate}): ");
            if (!string.IsNullOrWhiteSpace(v)) t.LicensePlate = v;

            v = ReadStringAllowEmpty($"Manufacturer ({t.Manufacturer}): ");
            if (!string.IsNullOrWhiteSpace(v)) t.Manufacturer = v;

            v = ReadStringAllowEmpty($"Model ({t.Model}): ");
            if (!string.IsNullOrWhiteSpace(v)) t.Model = v;

            v = ReadStringAllowEmpty($"Year ({t.Year}): ");
            if (!string.IsNullOrWhiteSpace(v))
            {
                if (int.TryParse(v, out int y) && y >= 1900 && y <= DateTime.Now.Year)
                    t.Year = y;
                else
                    Console.WriteLine("Invalid year entered. Keeping existing value.");
            }

            v = ReadStringAllowEmpty($"Capacity ({t.Capacity}): ");
            if (!string.IsNullOrWhiteSpace(v))
            {
                if (int.TryParse(v, out int c) && c >= 0)
                    t.Capacity = c;
                else
                    Console.WriteLine("Invalid capacity entered. Keeping existing value.");
            }

            v = ReadStringAllowEmpty($"Status ({t.Status}): ");
            if (!string.IsNullOrWhiteSpace(v)) t.Status = v;

            v = ReadStringAllowEmpty($"Last Maintenance Date ({t.LastMaintenanceDate:yyyy-MM-dd}): ");
            if (!string.IsNullOrWhiteSpace(v))
            {
                if (DateTime.TryParseExact(v, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d))
                    t.LastMaintenanceDate = d;
                else
                    Console.WriteLine("Invalid date format. Keeping existing value.");
            }

            Console.WriteLine("Transport updated.");
        }

        private static void DeleteTransport()
        {
            Console.WriteLine("\n--- Delete Transport ---");
            int id = ReadInt("Enter Id to delete: ");
            var t = transports.Find(x => x.Id == id);
            if (t == null)
            {
                Console.WriteLine("Transport not found.");
                return;
            }

            transports.Remove(t);
            Console.WriteLine("Transport deleted.");
        }

        private static void SearchByLicensePlate()
        {
            Console.WriteLine("\n--- Search by License Plate ---");
            string lp = ReadRequiredString("Enter License Plate to search: ");
            var results = transports.FindAll(x => string.Equals(x.LicensePlate, lp, StringComparison.OrdinalIgnoreCase));
            if (results.Count == 0)
            {
                Console.WriteLine("No transports found with that license plate.");
                return;
            }
            foreach (var t in results) PrintTransportSummary(t);
        }

        // ----------------- Helpers -----------------
        private static string ReadRequiredString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
                Console.WriteLine("This field is required.");
            }
        }

        private static string ReadStringAllowEmpty(string prompt)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            return s == null ? string.Empty : s.Trim();
        }

        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (int.TryParse(s, out int val)) return val;
                Console.WriteLine("Please enter a valid integer.");
            }
        }

        private static int ReadIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (int.TryParse(s, out int val) && val >= min && val <= max) return val;
                Console.WriteLine($"Please enter an integer between {min} and {max}.");
            }
        }

        private static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (DateTime.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d)) return d;
                Console.WriteLine("Please enter a date in yyyy-MM-dd format.");
            }
        }

        private static void PrintTransportSummary(Transport t)
        {
            Console.WriteLine($"Id: {t.Id} | {t.VehicleType} | {t.LicensePlate} | {t.Manufacturer} {t.Model} | Year: {t.Year} | Capacity: {t.Capacity} | Status: {t.Status}");
        }

        private static void PrintTransportDetails(Transport t)
        {
            Console.WriteLine($"\nTransport Details (Id: {t.Id})");
            Console.WriteLine($"VehicleType: {t.VehicleType}");
            Console.WriteLine($"LicensePlate: {t.LicensePlate}");
            Console.WriteLine($"Manufacturer: {t.Manufacturer}");
            Console.WriteLine($"Model: {t.Model}");
            Console.WriteLine($"Year: {t.Year}");
            Console.WriteLine($"Capacity: {t.Capacity}");
            Console.WriteLine($"Status: {t.Status}");
            Console.WriteLine($"LastMaintenanceDate: {t.LastMaintenanceDate:yyyy-MM-dd}");
        }
    }
}
