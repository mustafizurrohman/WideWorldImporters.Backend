using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WideWorldImporters.AuthenticationProvider.Database
{
    public partial class Users
    {
        public Users()
        {
            UsersRoles = new HashSet<UsersRoles>();
        }

        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? PasswordExpiresOn { get; set; }

        public virtual ICollection<UsersRoles> UsersRoles { get; set; }
    }
}
