using System.ComponentModel.DataAnnotations;

namespace GuoGuoCommunity.Domain.Models
{
    public class Test
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
