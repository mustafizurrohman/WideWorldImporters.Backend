using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WideWorldImporters.AuthenticationProvider.Database
{
    public partial class Roles
    {
        public Roles()
        {
            UsersRoles = new HashSet<UsersRoles>();
        }

        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid RoleId { get; set; }
        public string Role { get; set; }
        public bool IsAdmin { get; set; }

        public virtual ICollection<UsersRoles> UsersRoles { get; set; }
    }
}
