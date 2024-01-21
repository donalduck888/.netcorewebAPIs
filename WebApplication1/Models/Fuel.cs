using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Fuel
    {
        [Key] public int id { get; set; }

        public string fuelstationname { get; set; }
        public string district { get; set; }
        public string addresse { get; set; }

        public string fuelamt { get; set; }

        public string email { get; set; }

        public string PhoneNumber { get; set; } 
    }
}
