using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.CheckLaterLinksModuleEntities
{
    public class CheckLaterLink
    {
        [Key]
        public int LinkId { get; set; }

        public string CustomName { get; set; }

        public string SavedUrl { get; set; }

        public int CategoryId { get; set; }
    }
}