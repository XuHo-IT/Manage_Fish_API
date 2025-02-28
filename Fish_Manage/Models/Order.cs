using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fish_Manage.Models
{
    public partial class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string OrderId { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public string TotalAmount { get; set; }

        public string PaymentMethod { get; set; }
        [ForeignKey("ProductId")]
        public string? ProductId { get; set; }

        public int? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }
    }
}
