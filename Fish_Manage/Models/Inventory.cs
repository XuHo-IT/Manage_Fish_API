using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fish_Manage.Models
{
    public partial class Inventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryId { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string? StorageLocation { get; set; }

        public virtual Product? Product { get; set; }
    }
}
