using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.CheckLaterLinksModuleDTOS
{
    public class CheckLaterLinkDto
    {
        public string CustomName { get; set; }

        public string SavedUrl { get; set; }

        public string CategoryName { get; set; }

        public string Username { get; set; }
    }
}