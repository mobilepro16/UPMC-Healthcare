using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UPMC_App.ServerDataModels
{
    #region Added Data
    public class CategoryItem
    {
        [JsonProperty("_iKey")]
        public int IKey { get; set; }

        [JsonProperty("_sDesc")]
        public string SDesc { get; set; }

        public override string ToString()
        {
            return SDesc;
        }
    }
    #endregion

    #region Request Data
    public class ProviderSearchReqArrayItem
    {
        public string searchFieldName { get; set; }
        public int searchFieldType { get; set; }
        public object searchFieldValue { get; set; }
    }
    #endregion


    #region Response
    public class ProviderSearchItem
    {
        [JsonProperty("_Address1")]
        public string Address1 { get; set; }

        [JsonProperty("_Address2")]
        public string Address2 { get; set; }

        [JsonProperty("_City")]
        public string City { get; set; }

        [JsonProperty("_Distance")]
        public long Distance { get; set; }

        [JsonProperty("_EK")]
        public long Ek { get; set; }

        [JsonProperty("_FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("_FullName")]
        public string FullName { get; set; }

        [JsonProperty("_LK")]
        public long Lk { get; set; }

        [JsonProperty("_LastName")]
        public string LastName { get; set; }

        [JsonProperty("_Lat")]
        public double Lat { get; set; }

        [JsonProperty("_Lng")]
        public double Lng { get; set; }

        [JsonProperty("_OperatingHours")]
        public OperatingHours OperatingHours { get; set; }

        [JsonProperty("_PK")]
        public long Pk { get; set; }

        [JsonProperty("_Phone")]
        public string Phone { get; set; }

        [JsonProperty("_PlansAccepted")]
        public string[] PlansAccepted { get; set; }

        [JsonProperty("_Specialty")]
        public string Specialty { get; set; }

        [JsonProperty("_State")]
        public string State { get; set; }

        [JsonProperty("_Zip")]
        public string Zip { get; set; }
        

        public bool HasOperatingHours
        {
            get
            {
                if (OperatingHours != null)
                {
                    return ((OperatingHours.ViewHours != null) && (OperatingHours.ViewHours.Length > 0));
                }
                else
                    return false;
            }
        }

        public virtual bool IsPharmacyItem { get; }
        public virtual string DescriptionType { get; }
        public string FullAddressString => GetFullAddress();
        public virtual string FriendlyName { get; }
        public virtual string DescriptionDetails { get; }
        public string Description => GetDescription();

        public virtual string GetFullAddress()
        {
            return "";
        }
        public virtual string GetDescription()
        {
            return "";
        }
        public virtual string GetAddressWithoutCity()
        {
            return "";
        }
    }

    public class OperatingHours
    {
        [JsonProperty("_DisplayStatus")]
        public string DisplayStatus { get; set; }

        [JsonProperty("_ViewHours")]
        public ViewHour[] ViewHours { get; set; }
    }

    public class ViewHour
    {
        [JsonProperty("_CloseHourTime")]
        public string CloseHourTime { get; set; }

        [JsonProperty("_Day")]
        public long Day { get; set; }

        [JsonProperty("_DisplayDayText")]
        public string DisplayDayText { get; set; }

        [JsonProperty("_DisplayDayTimings")]
        public string DisplayDayTimings { get; set; }

        [JsonProperty("_IsValid")]
        public bool IsValid { get; set; }

        [JsonProperty("_OpenHourTime")]
        public string OpenHourTime { get; set; }
    }

    public class DoctorsListItem: ProviderSearchItem
    {
        public override bool IsPharmacyItem => false;
        public override string FriendlyName => FullName;
        public override string DescriptionType => "Specialty: ";
        public override string DescriptionDetails => Specialty;
        public override string GetFullAddress()
        {
            string ret = string.IsNullOrEmpty(Address1) ? "" : Address1;
            if (!String.IsNullOrEmpty(Address2))
                ret += $"\r\n{Address2}";
            if (!string.IsNullOrEmpty(City))
                ret += $"\r\n{City}, {State}";

            return ret;
        }
        public override string GetDescription()
        {
            return Specialty;
        }
        public override string GetAddressWithoutCity()
        {
            return $"{Address1} {Address2}";
        }
    }
    public class FacilitiesListItem : ProviderSearchItem
    {
        [JsonProperty("_FacilityName")]
        public string FacilityName { get; set; }

        public override bool IsPharmacyItem => false;
        public override string FriendlyName => FacilityName;
        public override string DescriptionType => "Services: ";
        public override string DescriptionDetails => Specialty;

        public override string GetFullAddress()
        {
            string ret = string.IsNullOrEmpty(Address1) ? "" : Address1;
            if (!String.IsNullOrEmpty(Address2))
                ret += $"\r\n{Address2}";
            if (!string.IsNullOrEmpty(City))
                ret += $"\r\n{City}, {State}";

            return ret;
        }
        public override string GetDescription()
        {
            return Specialty;
        }
        public override string GetAddressWithoutCity()
        {
            return $"{Address1} {Address2}";
        }
    }

    public class UCCFaciclitiesListItem : FacilitiesListItem
    {
        public override string DescriptionDetails => "Urgent Care";
        public override string GetDescription()
        {
            return $"{City}, {State}";
        }
    }
    public class PharmaciesListItem : ProviderSearchItem
    {
        [JsonProperty("_Address")]
        public string Address { get; set; }
        [JsonProperty("_Name")]
        public string Name { get; set; }
        [JsonProperty("_Is24hrs")]
        public bool Is24Hrs { get; set; }

        public override bool IsPharmacyItem => true;
        public override string FriendlyName => Name;
        public override string DescriptionType => "Open 24 hours: ";
        public override string DescriptionDetails => Is24Hrs ? "Yes" : "No";

        public override string GetFullAddress()
        {
            string ret = string.IsNullOrEmpty(Address) ? "" : Address;
            if (!string.IsNullOrEmpty(City))
                ret += $"\r\n{City}, {State}";

            return ret;
        }

        public override string GetDescription()
        {
            return $"{City}, {State}";
        }
        public override string GetAddressWithoutCity()
        {
            return Address;
        }
    }
    #endregion
}
