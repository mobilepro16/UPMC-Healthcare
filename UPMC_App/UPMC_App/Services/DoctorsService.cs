using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.ServerDataModels;

namespace UPMC_App.Services
{
    public class DoctorsService : BaseNetoworkService
    {
        public DoctorsService(string session) : base(session) { }


        private List<ProviderSearchReqArrayItem> GetGeneralParamsList(string zip = "", bool showMyNetwork = true, bool isSpecKeyExist = false)
        {
            List<ProviderSearchReqArrayItem> reqParam = new List<ProviderSearchReqArrayItem>();
            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "ZIP",
                searchFieldType = 16,
                searchFieldValue = zip
            });
            //reqParam.Add(new ProviderSearchReqArrayItem()//1 если Speciality указано
            //{
            //    searchFieldName = "PROV_TYPE",
            //    searchFieldType = 16,
            //    searchFieldValue = isSpecKeyExist ? "1" : ""
            //});
            reqParam.Add(new ProviderSearchReqArrayItem()//100 если есть ЗИП
            {
                searchFieldName = "DISTANCE",
                searchFieldType = 16,
                searchFieldValue = string.IsNullOrEmpty(zip) ? "" : "100"
            });
            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "SHOWMYNETWORK",
                searchFieldType = 16,
                searchFieldValue = showMyNetwork
            });

            return reqParam;
        }


        #region Doctors search by name
        /// <returns></returns>
        public List<ProviderSearchReqArrayItem> GetRequestParams(string specialtyKey = "", string zip = "", string name = "", bool showMyNetwork = true)
        {
            List<ProviderSearchReqArrayItem> reqParam = GetGeneralParamsList(zip, showMyNetwork);
            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "SPECIALTY_KEY",
                searchFieldType = 18,
                searchFieldValue = specialtyKey
            });

            reqParam.Add(new ProviderSearchReqArrayItem()//1 если Speciality указано
            {
                searchFieldName = "PROV_TYPE",
                searchFieldType = 16,
                searchFieldValue = string.IsNullOrEmpty(specialtyKey) ? "" : "1"
            });
            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "PROV_KEY",
                searchFieldType = 11,
                searchFieldValue = ""
            });
            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "LASTNAME",
                searchFieldType = 16,
                searchFieldValue = name
            });


            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "PROVIDERKVALUE",
                searchFieldType = 16,
                searchFieldValue = 11
            });

            return reqParam;
        }
        public async Task<List<DoctorsListItem>> SearchDoctors(string memberId, List<ProviderSearchReqArrayItem> searchParams)
        {
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/ProviderSearchServiceV2/SearchProvider/{memberId}";//$"https://mtst.upmchp.com/MobileServices/MemberPortalService/GetPrescription/{memberId}";
            
            StringContent content = new StringContent(JsonConvert.SerializeObject(searchParams), Encoding.UTF8, "application/json");

            string res = await SearchItems(dataUrl, content);
            List<DoctorsListItem> ret = null;
            try
            {
                ret = JsonConvert.DeserializeObject<List<DoctorsListItem>>(res);
            }
            catch {; }

            return ret;
        }
        #endregion

        #region Pharmacy search by name
        /// <returns></returns>
        public List<ProviderSearchReqArrayItem> GetPharmacyRequestParams(string retailKey = "", string zip = "", string name = "", bool showMyNetwork = true)
        {
            List<ProviderSearchReqArrayItem> reqParam = GetGeneralParamsList(zip, showMyNetwork);
            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "RETAIL_BRANCH",
                searchFieldType = 16,
                searchFieldValue = ""
            });

            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "PHARM_NAME",
                searchFieldType = 16,
                searchFieldValue = name
            });

            return reqParam;
        }

        public async Task<List<PharmaciesListItem>> SearchPharmacies(string memberId, List<ProviderSearchReqArrayItem> searchParams)
        {
            //string dataUrl = $"https://mtst.upmchp.com/MobileServices/ProviderSearchServiceV2/SearchPharmacy/{memberId}";
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/ProviderSearchServiceV2/SearchPharmacy/{memberId}";

            StringContent content = new StringContent(JsonConvert.SerializeObject(searchParams), Encoding.UTF8, "application/json");

            string res = await SearchItems(dataUrl, content);
            List<PharmaciesListItem> ret = null;
            try
            {
                ret = JsonConvert.DeserializeObject<List<PharmaciesListItem>>(res);
            }
            catch {; }

            return ret;
        }
        #endregion

        #region Hospitals and Facility search by name
        /// <returns></returns>
        public List<ProviderSearchReqArrayItem> GetFacilityRequestParams(string serviceKey = "", string zip = "", string name = "", bool showMyNetwork = true)
        {
            List<ProviderSearchReqArrayItem> reqParam = GetGeneralParamsList(zip, showMyNetwork);
            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "TYPE_KEY",
                searchFieldType = 11,
                searchFieldValue = serviceKey
            });
            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "FACILITY_NAME",
                searchFieldType = 16,
                searchFieldValue = name
            });         

            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "PROVIDERKVALUE",
                searchFieldType = 16,
                searchFieldValue = 11
            });

            return reqParam;
        }
        public async Task<List<FacilitiesListItem>> SearchFacilities(string memberId, List<ProviderSearchReqArrayItem> searchParams)
        {
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/ProviderSearchServiceV2/Facilities/{memberId}";

            StringContent content = new StringContent(JsonConvert.SerializeObject(searchParams), Encoding.UTF8, "application/json");

            string res = await SearchItems(dataUrl, content);
            List<FacilitiesListItem> ret = null;
            try
            {
                ret = JsonConvert.DeserializeObject<List<FacilitiesListItem>>(res);
            }
            catch {; }

            return ret;
        }
        #endregion

        #region UCC search by name
        /// <returns></returns>
        public List<ProviderSearchReqArrayItem> GetUCCRequestParams(string serviceKey = "", string zip = "", string name = "", bool showMyNetwork = true)
        {
            List<ProviderSearchReqArrayItem> reqParam = GetGeneralParamsList(zip, showMyNetwork);
            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "TYPE_KEY",
                searchFieldType = 11,
                searchFieldValue = serviceKey
            });
            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "FACILITY_NAME",
                searchFieldType = 16,
                searchFieldValue = name
            });

            reqParam.Add(new ProviderSearchReqArrayItem()
            {
                searchFieldName = "PROVIDERKVALUE",
                searchFieldType = 16,
                searchFieldValue = 11
            });

            return reqParam;
        }
        public async Task<List<UCCFaciclitiesListItem>> SearchUCC(string memberId, List<ProviderSearchReqArrayItem> searchParams)
        {
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/ProviderSearchServiceV2/SearchUrgentCareFacilities/{memberId}";

            StringContent content = new StringContent(JsonConvert.SerializeObject(searchParams), Encoding.UTF8, "application/json");

            string res = await SearchItems(dataUrl, content);
            List<UCCFaciclitiesListItem> ret = null;
            try
            {
                ret = JsonConvert.DeserializeObject<List<UCCFaciclitiesListItem>>(res);
            }
            catch {; }

            return ret;
        }
        #endregion

        private async Task<string> SearchItems(string dataUrl, StringContent content)
        {
            string res = string.Empty;
            try
            {
                var response = await _clnt.PostAsync(new Uri(dataUrl), content);
                res = await response.Content.ReadAsStringAsync();
            }
            catch {; }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">1-PCP, 2-Common, 3-NonCommon</param>
        /// <returns></returns>
        public async Task<List<CategoryItem>> GetDoctorsSpecialty(int type=1)
        {
            //string dataUrl = $"https://mtst.upmchp.com/MobileServices/ProviderSearchServiceV2/SearchProvider/{memberId}";
            string dataUrl=string.Empty;
            switch (type)
            {
                case 1:
                    dataUrl = $"https://mtst.upmchp.com/MobileServices/ProviderSearchService/Specialties/PCP";
                    break;
                case 2:
                    dataUrl = $"https://mtst.upmchp.com/MobileServices/ProviderSearchService/Specialties/CommonSpecialties";
                    break;
                case 3:
                    dataUrl = $"https://mtst.upmchp.com/MobileServices/ProviderSearchService/Specialties/NonCommonSpecialties";
                    break;
                default:
                    break;
            }
            //string dataUrl = isCommon ? 
            //    $"https://mtst.upmchp.com/MobileServices/ProviderSearchService/Specialties/CommonSpecialties" :
            //    $"https://mtst.upmchp.com/MobileServices/ProviderSearchService/Specialties/NonCommonSpecialties";
            List<CategoryItem> ret = null;
            try
            {
                var response = await _clnt.GetAsync(dataUrl);
                var res = await response.Content.ReadAsStringAsync();

                ret = JsonConvert.DeserializeObject<List<CategoryItem>>(res);
            }
            catch {; }

            return ret;
        }

        public async Task<List<CategoryItem>> GetCategories(string categoryTypeURL)
        {
            string dataUrl = $"https://mtst.upmchp.com/MobileServices/ProviderSearchService/{categoryTypeURL}";
            List<CategoryItem> ret = null;
            try
            {
                var response = await _clnt.PostAsync(new Uri(dataUrl), null);
                var res = await response.Content.ReadAsStringAsync();
                ret = JsonConvert.DeserializeObject<List<CategoryItem>>(res);
            }
            catch {; }

            return ret;
        }
    }
}
