using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Vehicles
    {
        [Key]
        public int id { get; set; }  
        public string VehicleType { get; set; } 
        public string Nic { get; set; }
        public string CNo { get; set; }  
        public string FuelType { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IsuAmount { get; set; }
        public string UserName { get; set; }  

    }
}
