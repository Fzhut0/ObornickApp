using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.IdentityModel.Tokens;

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
            var accessApiKey = _facebookMessageService.ServiceApiKey;

            var recipientId = await _uow.UserRepository.GetUserMessageRecipientIdByUsername(User.Identity.Name);

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

        [HttpGet("getconversations")]
        public async Task<ActionResult> GetConversations()
        {
            var pageId = _facebookMessageService.SenderPageId;
            var accessApiKey = _facebookMessageService.ServiceApiKey;

            using (var httpClient = new HttpClient())
            {
                var apiUrl = $"https://graph.facebook.com/v19.0/{pageId}/conversations?fields=participants&access_token={accessApiKey}";

                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseAsString = await response.Content.ReadAsStringAsync();
                    var conversations = MessageConversationReader.FromJson(responseAsString);

                    var participantList = new List<string>();

                    foreach(var conv in conversations.Data)
                    {
                        foreach(var participant in conv.Participants.Data)
                        {
                            if(participant.Id == pageId)
                            {
                                continue;
                            }
                            participantList.Add(participant.Name);
                        }
                    }      
                    return Ok(participantList);
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