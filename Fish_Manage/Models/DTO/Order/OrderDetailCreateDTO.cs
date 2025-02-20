﻿using System.ComponentModel.DataAnnotations;

namespace Fish_Manage.Models.DTO.OrderDetail
{
    public class OrderDetailCreateDTO
    {
        [Required]
        public string ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
        public decimal UnitPrice { get; set; }
    }
}
