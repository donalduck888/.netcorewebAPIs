using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Customer
    {
        [Key] public long id { get; set; }  

        public string name { get; set; } 
        public string vehi_id { get; set; } 
        public string username { get; set; }

        public string email { get; set; }

        public string phone { get; set; }


    }
}
