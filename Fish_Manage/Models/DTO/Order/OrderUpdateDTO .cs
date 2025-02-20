using System.ComponentModel.DataAnnotations;

namespace Fish_Manage.Models.DTO.Order
{
    public class OrderUpdateDTO
    {
        public int OrderId { get; set; }
        [Required]
        [StringLength(50)]


        public DateTime OrderDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Status { get; set; }

        public virtual User? Customer { get; set; }
    }
}
