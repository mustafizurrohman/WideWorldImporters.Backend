using System;
using System.Collections.Generic;

namespace WideWorldImporters.AuthenticationProvider.Database
{
    public partial class UsersRoles
    {
        public Guid UsersRoleId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public virtual Roles Role { get; set; }
        public virtual Users User { get; set; }
    }
}
