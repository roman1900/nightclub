using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Text.Json;


namespace sappy
{
    public class Spotify
    {
        private string _client_id { get; set; }
        private string _client_secret { get; set; }
        private string _redirect_uri { get; set; }

        public string state { get; private set; }
        private static readonly HttpClient h = new HttpClient();


        private const string AuthEndPoint = "https://accounts.spotify.com/authorize";
        private const string TokenEndPoint = "https://accounts.spotify.com/api/token";

        [Flags]
        public enum Scopes
        {

            ugc_image_upload = 1 << 0,

            user_read_recently_played = 1 << 1,

            user_top_read = 1 << 2,

            user_read_playback_position = 1 << 3,

            user_read_playback_state = 1 << 4,

            user_modify_playback_state = 1 << 5,

            user_read_currently_playing = 1 << 6,

            app_remote_control = 1 << 7,

            streaming = 1 << 8,

            playlist_modify_public = 1 << 9,

            playlist_modify_private = 1 << 10,

            playlist_read_private = 1 << 11,

            playlist_read_collaborative = 1 << 12,

            user_follow_modify = 1 << 13,

            user_follow_read = 1 << 14,

            user_library_modify = 1 << 15,

            user_library_read = 1 << 16,

            user_read_email = 1 << 17,

            user_read_private = 1 << 18,
        }

        public Spotify(string client_id, string client_secret, string redirect_uri)
        {
            _client_id = client_id;
            _client_secret = client_secret;
            _redirect_uri = HttpUtility.UrlEncode(redirect_uri);
        }

        public Uri Authorise(Scopes scope)
        {
            state = Guid.NewGuid().ToString();
            UriBuilder u = new UriBuilder(AuthEndPoint);
            u.Query = u.Query
                + "client_id=" + _client_id
                + "&response_type=token"
                + "&redirect_uri=" + _redirect_uri
                + "&state=" + state;
           
            if (scope>0)
            {
                u.Query = u.Query + "&scope=" + scope.ToString().Replace(",", "").Replace('_', '-');
            }

            Debug.WriteLine(u.Uri.AbsoluteUri);
            return u.Uri;
        }

        public async Task<AuthTokenResponse> RequestTokens(string code)
        {
            string grant_type = "authorization_code";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", grant_type),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", _redirect_uri)
            });
            
            content.Headers.Add("Authorization", Base64Encode($"{_client_id}:{_client_secret}"));
            HttpResponseMessage hr = await h.PostAsync(TokenEndPoint, content);
            return JsonSerializer.Deserialize<AuthTokenResponse>(hr.Content.ToString());
        }

        public bool VerifyState(string state)
        {
            return state.Equals(this.state); 
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
