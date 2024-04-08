using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.CheckLaterLinksModuleEntities;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }

        public string MessageServiceRecipientId { get; set; }

        public ICollection<CheckLaterLinkCategory> Categories { get; set; }

        public ICollection<CheckLaterLink> Links { get; set; }
    }
}