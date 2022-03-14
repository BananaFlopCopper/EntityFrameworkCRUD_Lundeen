using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCRUD_Lundeen.Models
{
    public partial class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(40)]
        public string LastName { get; set; } = null!;
        [MaxLength(40)]
        public string? City { get; set; }
        [MaxLength(40)]
        public string? Country { get; set; }
        [MaxLength(20)]
        public string? Phone { get; set; }
    }
}
