using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FacebookMessageController : BaseApiController
    {
        private readonly IFacebookMessageService _facebookMessageService;

        public FacebookMessageController(IFacebookMessageService facebookMessageService)
        {
            _facebookMessageService = facebookMessageService;
        }


        [HttpPost]
        public async Task<ActionResult> SendMessage(string msg)
        {
            var pageId = _facebookMessageService.SenderPageId;
            var recipientId = _facebookMessageService.RecipientId;
            var accessApiKey = _facebookMessageService.ServiceApiKey;

            using (var httpClient = new HttpClient())
            {
                var apiUrl = $"https://graph.facebook.com/v19.0/{pageId}/messages?recipient={{'id':'{recipientId}'}}&messaging_type=RESPONSE&message={{'text':'{msg}'}}&access_token={accessApiKey}";

                var response = await httpClient.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    return Ok("Message sent");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return BadRequest(errorMessage);
                }
            }
        }
        
    }
}