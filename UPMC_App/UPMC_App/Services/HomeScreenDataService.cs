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
    public class HomeScreenDataService
    {
        private readonly HttpClient _clnt;

        public HomeScreenDataService()
        {
            _clnt = new HttpClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }
        public HomeScreenDataService(string session)
        {
            _clnt = new HttpClient();
            SetSesionKey(session);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }
        public void SetSesionKey(string session)
        {
            _clnt.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", $"PrivateKey:{ViewModelContainer.PrivateKey}SessionKey:{session}");
        }

        #region Claims
        public async Task<List<ClaimInfo>> GetRecentClaimsData(string memberId)
        {
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/HomeService/Claims/{memberId}";
            StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");

            List<ClaimInfo> ret = null;
            try
            {
                var response = await _clnt.PostAsync(new Uri(dataUrl), null); 
                var res = await response.Content.ReadAsStringAsync();

                ret = JsonConvert.DeserializeObject<List<ClaimInfo>>(res);
            }
            catch {; }

            return ret;
        }
        #endregion

        #region ID Cards
        public async Task<List<IdCardInfo>> GetIdCards(string memberId)
        {
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/MemberPortalService/IDCards/GetFamilyMembers/{memberId}";
            StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");

            List<IdCardInfo> ret = null;
            try
            {
                var response = await _clnt.GetAsync(new Uri(dataUrl));
                var res = await response.Content.ReadAsStringAsync();

                ret = JsonConvert.DeserializeObject<List<IdCardInfo>>(res);
            }
            catch {; }

            return ret;
        }
        #endregion

        #region Claims-CoveredMembers
        public async Task<List<IdCardInfo>> GetClaimsCoveredMembers(string memberId)
        {
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/MemberPortalService/Claims/CoveredMembers/members/{memberId}";
            StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");

            List<IdCardInfo> ret = null;
            try
            {
                var response = await _clnt.GetAsync(new Uri(dataUrl));
                var res = await response.Content.ReadAsStringAsync();

                ret = JsonConvert.DeserializeObject<List<IdCardInfo>>(res);
            }
            catch {; }

            return ret;
        }
        public async Task<List<ClaimInfo>> GetClaimsListing(string memberId, string claimCoveredMember)
        {
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/MemberPortalService/Claims/members/{memberId}/0/{claimCoveredMember}";
            //StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");

            List<ClaimInfo> ret = null;
            try
            {
                var response = await _clnt.GetAsync(new Uri(dataUrl));
                var res = await response.Content.ReadAsStringAsync();

                ret = JsonConvert.DeserializeObject<List<ClaimInfo>>(res);
            }
            catch {; }

            return ret;
        }
        #endregion

        #region Family-ID Cards
        public async Task<List<FamilyIdCardInfo>> GetFamilyIdCards(string memberId)
        {
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/MemberPortalService/IDCards/VirtualIDCards/{memberId}/{memberId}";

            List<FamilyIdCardInfo> ret = null;
            try
            {
                var response = await _clnt.GetAsync(new Uri(dataUrl));
                var res = await response.Content.ReadAsStringAsync();

                ret = JsonConvert.DeserializeObject<List<FamilyIdCardInfo>>(res);
            }
            catch {; }

            return ret;
        }
        #endregion
    }
}
