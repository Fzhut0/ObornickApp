using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.CheckLaterLinksModuleEntities
{
    public class CheckLaterLinkCategory
    {
        [Key]
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }

        public CheckLaterLinkCategory ParentCategory { get; set; }

        public ICollection<CheckLaterLinkCategory> Subcategories { get; set; }

        public ICollection<CheckLaterLink> CheckLaterLinks { get; set; }

        public int UserId { get; set; }

    }
}