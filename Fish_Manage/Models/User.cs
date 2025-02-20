using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fish_Manage.Models
{
    public partial class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserId { get; set; }

        public string? UserName { get; set; }
        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? ZipCode { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public int? RoleUser { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
