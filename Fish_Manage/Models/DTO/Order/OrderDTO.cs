using System.ComponentModel.DataAnnotations;

namespace Fish_Manage.Models.DTO.Order
{
    public class OrderDTO
    {
        public string OrderId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public string TotalAmount { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal? UnitPrice { get; set; }
    }
}
