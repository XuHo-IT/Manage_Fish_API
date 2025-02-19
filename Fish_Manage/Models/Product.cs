using System;
using System.Collections.Generic;

namespace Fish_Manage.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int? OrderId { get; set; }

    public string? ProductName { get; set; }

    public decimal? Price { get; set; }

    public string? Category { get; set; }

    public string? Description { get; set; }

    public string? Supplier { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual Order? Order { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
