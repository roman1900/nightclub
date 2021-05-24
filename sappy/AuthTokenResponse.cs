using System;
using System.Collections.Generic;
using System.Text;

namespace sappy
{
    public class AuthTokenResponse
    {
        public string access_token;
        public string token_type;
        public string scope;
        public int expires_in;
    }
}
