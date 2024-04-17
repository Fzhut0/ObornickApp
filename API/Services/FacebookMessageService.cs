using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;

namespace API.Services
{
    public class FacebookMessageService : IFacebookMessageService
    {
        public string ServiceApiKey { get; set; }

        public string SenderPageId { get; set; }

        public string RecipientId { get; set; }

        public FacebookMessageService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            if(environment.IsDevelopment())
            {
                ServiceApiKey = configuration["Facebook:ServiceApiKey"];
                SenderPageId = configuration["Facebook:SenderPageId"];
            }
            else
            {
                ServiceApiKey = Environment.GetEnvironmentVariable("FACEBOOK_APIKEY");
                SenderPageId = Environment.GetEnvironmentVariable("FACEBOOK_PAGEID");
            }
            
        }
    }
}