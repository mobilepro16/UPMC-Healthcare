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
    public class PrescriptionsService:BaseNetoworkService
    {
        public PrescriptionsService(string session):base(session)
        {

        }

        #region Prescriptions
        public async Task<List<PrescriptionResponseInfo>> GetPrescriptions(string memberId)
        {
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/MemberPortalService/GetPrescription/{memberId}";
            //StringContent content = new StringContent(String.Empty, Encoding.UTF8, "application/json");

            List<PrescriptionResponseInfo> ret = null;
            try
            {
                var response = await _clnt.GetAsync(new Uri(dataUrl));
                var res = await response.Content.ReadAsStringAsync();

                ret = JsonConvert.DeserializeObject<List<PrescriptionResponseInfo>>(res);
            }
            catch {; }

            return ret;
        }
        #endregion
    }
}
