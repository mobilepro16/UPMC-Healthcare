using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.ServerDataModels;

namespace UPMC_App.Services
{
    public class ProvidersServices : BaseNetoworkService
    {
        public ProvidersServices(string session) : base(session) { }

        public async Task<List<ProviderHistoryItem>> GetProviders(string memberId)
        {
            //string dataUrl = $"https://mtst.upmchp.com/MobileServices/MemberPortalService/GetMemberProviderStatus/Doctor/{memberId}";
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/PhrService/Doctors/members/{memberId}";
            //StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");

            List<ProviderHistoryItem> ret = null;
            try
            {
                var response = await _clnt.GetAsync(new Uri(dataUrl));
                var res = await response.Content.ReadAsStringAsync();

                ret = JsonConvert.DeserializeObject<List<ProviderHistoryItem>>(res);
            }
            catch {; }

            if(ret!=null)
            {
                foreach (var item in ret)
                    item.TrySetDateOfService();
            }

            return ret;
        }

        public async Task<List<ProviderHistoryItem>> GetMyProviders(string memberId)
        {
            //string dataUrl = $"https://mtst.upmchp.com/MobileServices/MemberPortalService/GetMemberProviderStatus/Doctor/{memberId}";
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/MemberPortalService/MyHealthCircles/ProviderSeen/members/{memberId}";
            //StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");

            List<ProviderHistoryItem> ret = null;
            try
            {
                var response = await _clnt.GetAsync(new Uri(dataUrl));
                var res = await response.Content.ReadAsStringAsync();

                ret = JsonConvert.DeserializeObject<List<ProviderHistoryItem>>(res);
            }
            catch {; }

            if (ret != null)
            {
                foreach (var item in ret)
                    item.TrySetDateOfService();
            }

            return ret;
        }

        public async Task<bool> DeleteProvider(string memberId, string providerKey)
        {
            //string dataUrl = $"https://mtst.upmchp.com/MobileServices/MemberPortalService/GetMemberProviderStatus/Doctor/{memberId}";
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/MemberPortalService/MyHealthCircles/ProviderSeen/Delete/members/{providerKey}/{memberId}";
            //StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");

            //List<ProviderHistoryItem> ret = null;
            try
            {
                var response = await _clnt.GetAsync(new Uri(dataUrl));
                var res = await response.Content.ReadAsStringAsync();

                //ret = JsonConvert.DeserializeObject<List<ProviderHistoryItem>>(res);
            }
            catch {; }

            return true;
        }
    }
}
