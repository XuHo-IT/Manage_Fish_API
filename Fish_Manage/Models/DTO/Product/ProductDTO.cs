using System.ComponentModel.DataAnnotations;

namespace Fish_Manage.Models.DTO.Product
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        public int? OrderId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        public decimal? Price { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string Supplier { get; set; }
    }
}
