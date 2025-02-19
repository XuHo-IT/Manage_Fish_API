using System;
using System.Collections.Generic;

namespace Fish_Manage.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public string? StorageLocation { get; set; }

    public virtual Product? Product { get; set; }
}
