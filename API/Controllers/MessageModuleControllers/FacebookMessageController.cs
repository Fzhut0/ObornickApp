using System.Web;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FacebookMessageController : BaseApiController
    {
        private readonly IFacebookMessageService _facebookMessageService;
        private readonly IUnitOfWork _uow;

        public FacebookMessageController(IFacebookMessageService facebookMessageService, IUnitOfWork uow)
        {
            _facebookMessageService = facebookMessageService;
            _uow = uow;
        }


        [HttpPost("sendmessage")]
        public async Task<ActionResult> SendMessage([FromBody]MessageDto messageDto)
        {
            var pageId = _facebookMessageService.SenderPageId;
            var recipientId = "";
            var accessApiKey = _facebookMessageService.ServiceApiKey;

            recipientId = await _uow.UserRepository.GetUserMessageRecipientIdByUsername(messageDto.MessageRecipientUsername);

            var msg = messageDto.Message;

            msg = HttpUtility.UrlDecode(msg);

            if(recipientId == null)
            {
                return BadRequest("no recipient id");
            }

            if(msg == null)
            {
                return BadRequest("message is null");
            }

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

        
        [HttpPost]
        public async Task<ActionResult> SetRecipientIdForMessaging(string username, string id)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(username);

            user.MessageServiceRecipientId = id;

            await _uow.Complete();

            return Ok();
        }
        
    }
}