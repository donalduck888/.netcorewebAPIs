using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Approve
    {

        [Key] public int id { get; set; }
        public DateTime reqdate { get; set; }
        public DateTime tokento { get; set; } 
         
        public string isuamt { get; set; }

        public string vehicletype { get; set; }

        public string username { get; set; } 

       public int T_id { get; set; }


    }
}
