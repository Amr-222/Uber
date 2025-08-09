using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uber.DAL.Enums;

namespace Uber.DAL.Entities
{
    public class Vehicle
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

       

        public string Brand { get; private set; }

        public string Model { get; private set; }

        public int Year { get; private set; }

        public string Plate { get; private set; }

        public string Color { get; private set; }

        public int SeatingCapacity {  get; private set; }

        public VehicleType Type { get; private set; }

        public Transmission Transmission { get; private set; } // Manual or Automatic

        public VehicleEngine VehicleEngine { get; private set; } // Gas or Electric



        public string Get_Type() {

            switch (Type)
            {
                case VehicleType.Car:
                    return "Car";
                    break;
                case VehicleType.Scooter:
                    return "Scooter";
                    break;
                    case VehicleType.Shuttle:
                    return "Shuttle";
                    break;

                default:
                    return "";
                    break;

            }

        }

        public Vehicle() { }

        public Vehicle(string brand, string model, int year, string plate, string color, int seatingCapacity, VehicleType type, Transmission transmission, VehicleEngine vehicleEngine)
        {
            Brand = brand;
            Model = model;
            Year = year;
            Plate = plate;
            Color = color;
            SeatingCapacity = seatingCapacity;
            Type = type;
            Transmission = transmission;
            VehicleEngine = vehicleEngine;
        }




        //License  **  

    }
}
