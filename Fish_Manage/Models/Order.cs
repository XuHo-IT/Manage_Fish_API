using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fish_Manage.Models
{
    public partial class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public string? UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Status { get; set; }

        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
