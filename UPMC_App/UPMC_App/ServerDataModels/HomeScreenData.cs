using System;
using System.Collections.Generic;
using System.Text;

namespace UPMC_App.ServerDataModels
{
    #region ID Cards
    public class IdCardInfo
    {
        public bool _IsViewClaims { get; set; }
        public string _MemberId { get; set; }
        public string _MemberName { get; set; }
        public string _MemberType { get; set; }

        public string Name => _MemberName;
        public string Type => _MemberType;
    }

    public class FamilyIdCardInfo
    {
        public string _idCardBackImageBase64 { get; set; }
        public string _idCardFrontImageBase64 { get; set; }
        public string _rxGroupCode { get; set; }
        public string cardType { get; set; }
        public string coverageDesc { get; set; }
        public string coverageType { get; set; }
        public string description { get; set; }
        public string eRVisit { get; set; }
        public string employerID { get; set; }
        public string groupNumber { get; set; }
        public bool hasPrescription { get; set; }
        public string inNetworkCare { get; set; }
        public string medicaidRecipientID { get; set; }
        public string memberID { get; set; }
        public string memberName { get; set; }
        public string network { get; set; }
        public string officeVisit { get; set; }
        public string outOfNetworkCare { get; set; }
        public string pcpName { get; set; }
        public string pcpPhone { get; set; }
        public string planName { get; set; }
        public string preventativeCare { get; set; }
        public string rx { get; set; }
        public string specialistVist { get; set; }
        public string subGroupNumber { get; set; }
    }
    #endregion

    #region Prescriptions
    public class PrescriptionListItem
    {
        public string _DrugName { get; set; }
        public double _DrugQuantity { get; set; }
        public string _PharmacyName { get; set; }
        public string _PrescriberName { get; set; }
        public string _ServiceDateString { get; set; }
        public double daySupply { get; set; }
        public string dosage { get; set; }
        public string prescriptionNumber { get; set; }
    }

    public class AddressInfoClass
    {
        public string addressLine { get; set; }
        public string city { get; set; }
        public string phone { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }
    public class PharmacyListItem
    {
        public AddressInfoClass addressInfo { get; set; }
        public bool isFavorite { get; set; }
        public string pharmacyName { get; set; }
    }

    public class PrescriptionResponseInfo
    {
        public List<PrescriptionListItem> currentPrescriptionList { get; set; }
        public bool isPharmacyListChangeable { get; set; }
        public string memberID { get; set; }
        public string memberName { get; set; }
        public List<PharmacyListItem> pharmacyList { get; set; }
        public List<PrescriptionListItem> prescriptionDetailsList { get; set; }
    }
    #endregion

    #region Claims
    public class ClaimInfo
    {
        public string allowedAmt { get; set; }
        public string amountDenied { get; set; }
        public string claimNumber { get; set; }
        public string claimStatus { get; set; }
        public string claimType { get; set; }
        public string coInsurance { get; set; }
        public string coPayment { get; set; }
        public string coveredMember { get; set; }
        public string coveredMemberType { get; set; }
        public string dateOfService { get; set; }
        public string deductible { get; set; }
        public string facility { get; set; }
        public string networkDiscount { get; set; }
        public string provider { get; set; }
        public string providerBilled { get; set; }
        public string servicesProvided { get; set; }
        public string totalResponsibility { get; set; }
    }
    #endregion
}
