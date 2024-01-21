using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Tutorial 
    {
        [Key] public int id { get; set; } 

        public string title { get; set; } 
        public string description { get; set; }
        public bool published { get; set; } 
      
    }
}
