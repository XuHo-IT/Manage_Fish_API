using System;
using System.Collections.Generic;

namespace Fish_Manage.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? OrderId { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? TransactionDate { get; set; }

    public decimal? Amount { get; set; }

    public string? Status { get; set; }

    public virtual Order? Order { get; set; }
}
