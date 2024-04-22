using System.ComponentModel.DataAnnotations;

namespace API.Entities.CheckLaterLinksModuleEntities
{
    public class CheckLaterLink
    {
        [Key]
        public int LinkId { get; set; }

        public string CustomName { get; set; }

        public string SavedUrl { get; set; }

        public int CategoryId { get; set; }

        public int UserId { get; set; }

    }
}