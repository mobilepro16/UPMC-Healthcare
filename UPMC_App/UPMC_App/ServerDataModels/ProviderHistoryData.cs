using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UPMC_App.ServerDataModels
{
    public class ProviderHistoryItem
    {
        [JsonProperty("_AddedBy")]
        public string AddedBy { get; set; }

        [JsonProperty("_Address")]
        public string Address { get; set; }

        [JsonProperty("_DateAdded")]
        public string DateAdded { get; set; }

        [JsonProperty("_IsFavorite")]
        public bool IsFavorite { get; set; }

        [JsonProperty("_LocationKey")]
        public long LocationKey { get; set; }

        [JsonProperty("_MemberId")]
        public string MemberId { get; set; }

        [JsonProperty("_PhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("_ProviderID")]
        public long ProviderId { get; set; }

        [JsonProperty("_ProviderKey")]
        public long ProviderKey { get; set; }

        [JsonProperty("_ProviderName")]
        public string ProviderName { get; set; }

        [JsonProperty("_ServiceDate")]
        public string ServiceDate { get; set; }

        [JsonProperty("_ServiceDateString")]
        public string ServiceDateString { get; set; }

        [JsonProperty("_Specialty")]
        public string Specialty { get; set; }

        [JsonProperty("_facilityKey")]
        public long FacilityKey { get; set; }

        [JsonProperty("key")]
        public long Key { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        public DateTime DateOfService { get; set; }

        public bool IsVisitHistoryEnabled { get => string.IsNullOrEmpty(AddedBy); }
        public bool IsOfficeInfoEnabled { get => ProviderKey >= 1; }
        public string ImgVisitHistory { get => string.IsNullOrEmpty(AddedBy) ? "history_material.png" : "history_material_gray.png"; }
        public string ImgOfficeInfo { get => ProviderKey<1 ? "info_material_gray.png" : "info_material.png"; }
        /// <summary>
        /// 0 - regular, 1 - button, 2 - info frame
        /// </summary>
        public int ItemType { get; set; } = 0;

        public void TrySetDateOfService()
        {
            if (DateTime.TryParseExact(ServiceDateString, "d/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dt))
                DateOfService = dt;
            else
                DateOfService = DateTime.MinValue;
        }
    }
}
