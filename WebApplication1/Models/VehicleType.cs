using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class VehicleType
    {
        [Key] public int Id { get; set; }

        public string Vehicletype { get; set; } 
        public int IsuAmount { get; set; } 
    }
}
