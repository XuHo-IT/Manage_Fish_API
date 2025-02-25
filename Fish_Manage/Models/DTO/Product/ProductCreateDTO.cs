using System.ComponentModel.DataAnnotations;

namespace Fish_Manage.Models.DTO.Product
{
    public class ProductCreateDTO
    {
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        public decimal? Price { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Supplier { get; set; }

    }
}
