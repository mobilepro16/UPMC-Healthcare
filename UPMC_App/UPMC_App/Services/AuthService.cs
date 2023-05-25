using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.ServerDataModels;

namespace UPMC_App.Services
{
    public class AuthService
    {
        private readonly HttpClient _clnt;

        public AuthService()
        {
            
            _clnt = new HttpClient();
            //_clnt.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "PrivateKey:SjrTgyLSax91Wg38QLjj1OkjfNwLkgPcAh7QPSIZpR1m93tZOMiPBD9QqFBLVrXvI5mxX6xmWxJ5Ac4pZmeAZnrp33aYt9ubdQJaVgxSOhXpt2Ptn44mquMtxXr27VSessionKey:");
            _clnt.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", $"PrivateKey:{ViewModelContainer.PrivateKey}SessionKey:");

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public async Task<Api_AuthResponse> AuthorizeWithPassword(string username, string pass)
        {
            var byteArray = new UTF8Encoding().GetBytes(username);
            string usr64 = Convert.ToBase64String(byteArray);
            byteArray = new UTF8Encoding().GetBytes(pass);
            string pass64 = Convert.ToBase64String(byteArray);

            string authUrl = $"https://mtst.upmchp.com/MobileServices/AuthenticationService/mAuthenticate/{usr64}/{pass64}";
            StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");

            var response = await _clnt.PostAsync(new Uri(authUrl), null);
            var res = await response.Content.ReadAsStringAsync();

            Api_AuthResponse ret = null;
            try
            {
                ret = JsonConvert.DeserializeObject<Api_AuthResponse>(res);
            }
            catch {; }

            return ret;
        }

        public async Task<Api_AuthResponse> AuthorizeWithPin(string username, string pin)
        {
            string authUrl = $"https://mtst.upmchp.com/MobileServices/SecurityService/mPinAuthenticate/Pin/{username}/{pin}";
            StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");

            Api_AuthResponse ret = null;

            try
            {
                var response = await _clnt.PostAsync(new Uri(authUrl), null);
                var res = await response.Content.ReadAsStringAsync();

                ret = JsonConvert.DeserializeObject<Api_AuthResponse>(res);
            }
            catch {; }

            return ret;
        }
    }
}
