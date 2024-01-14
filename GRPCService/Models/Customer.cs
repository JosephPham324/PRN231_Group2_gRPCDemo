using System.ComponentModel.DataAnnotations;

namespace GRPCService.Models
{
    public class Customer
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
