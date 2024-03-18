using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IFacebookMessageService
    {
        public string ServiceApiKey { get;}
        public string SenderPageId { get;}
        public string RecipientId { get;}
    }
}