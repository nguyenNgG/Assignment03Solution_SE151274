using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class Member : IdentityUser
    {
        public Member()
        {
            Orders = new HashSet<Order>();
        }

        [Column(TypeName = "nvarchar(256)")]
        public string? FirstName { get; set; }

        [Column(TypeName = "nvarchar(256)")]
        public string? LastName { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
