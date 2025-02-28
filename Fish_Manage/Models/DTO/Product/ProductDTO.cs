using System.ComponentModel.DataAnnotations;

namespace Fish_Manage.Models.DTO.Product
{
    public class ProductDTO
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string OrderId { get; set; }
        public string ProductName { get; set; }

        public decimal? Price { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string Supplier { get; set; }
        public string ImageURl { get; set; }

    }
}
