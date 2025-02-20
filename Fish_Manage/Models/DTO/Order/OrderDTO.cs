using System.ComponentModel.DataAnnotations;

namespace Fish_Manage.Models.DTO.Order
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        public string? UserId { get; set; }
        [Required]
        [StringLength(50)]

        public DateTime OrderDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Status { get; set; }
    }
}
