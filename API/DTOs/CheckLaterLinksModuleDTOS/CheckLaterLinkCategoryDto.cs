using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.CheckLaterLinksModuleEntities;

namespace API.DTOs.CheckLaterLinksModuleDTOS
{
    public class CheckLaterLinkCategoryDto
    {
        public string CustomName { get; set; }

        public List<CheckLaterLink> Links { get; set; }
    }
}