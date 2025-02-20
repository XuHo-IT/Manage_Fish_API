using Fish_Manage.Models.DTO.OrderDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fish_Manage.Models.DTO.Order
{
    public class OrderCreateDTO
    {
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal TotalAmount { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public List<OrderDetailCreateDTO> OrderDetails { get; set; }
    }
}
