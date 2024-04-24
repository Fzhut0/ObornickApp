using API.Entities.CheckLaterLinksModuleEntities;

namespace API.DTOs.CheckLaterLinksModuleDTOS
{
    public class CheckLaterLinkCategoryDto
    {
        public string CustomName { get; set; }

        public List<CheckLaterLink> Links { get; set; }

        public int CategoryId { get; set; }

        public string ParentCategoryName { get; set; }

        public int ParentCategoryId { get; set; }

        public bool IsSubcategory { get; set; }
    }
}