using System;
using System.Collections.Generic;

namespace Fish_Manage.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public virtual User? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
