using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using UPMC_App.ServerDataModels;
using UPMC_App.Services;

namespace UPMC_App.ViewModels
{
    #region Presenters
    public class ClaimPresenter
    {
        public ClaimInfo _baseInfo { get; set; }
        public DateTime DateOfService { get; set; }


        public string MonthShort => DateOfService.ToString("MMM");
        public int DayShort => DateOfService.Day;
        public string Provider { get; set; }
        public string CoveredMember { get; set; }
        public string ForCoveredMemberString => $"For: {CoveredMember}";
        public string ClaimStatus { get; set; }
        public string ClaimStatusByText { get; set; }
        public string ClaimStatusImage { get; set; }
        public string Facility { get; set; }
        public string ClaimNumber { get; set; }

        public string ProviderBilled => _baseInfo.providerBilled;
        public string AmountAllowed => _baseInfo.allowedAmt;
        public string NetworkDiscount => _baseInfo.networkDiscount;
        public string AmountDenied => _baseInfo.amountDenied;
        public string CoPayment => _baseInfo.coPayment;
        public string Deductible => _baseInfo.deductible;
        public string CoInsurance => _baseInfo.coInsurance;
        public string TotalResponsibility => _baseInfo.totalResponsibility;
        public string ServicesProvided => _baseInfo.servicesProvided;



        public string DateFriendly => DateOfService.ToShortDateString();


        public bool IsClaimVisible => _baseInfo != null;
        

        public ClaimPresenter() { }
        public ClaimPresenter(ClaimInfo claim)
        {
            _baseInfo = claim;
            if (DateTime.TryParseExact(claim.dateOfService, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime date))
                DateOfService = date;
            else
                DateOfService = DateTime.MinValue;
            Provider = _baseInfo.provider;
            CoveredMember = _baseInfo.coveredMember;
            Facility = claim.facility;
            ClaimNumber = claim.claimNumber;

            switch (claim.claimStatus)
            {
                case "Paid":
                    ClaimStatus = "Paid";
                    ClaimStatusByText = " by UPMC Health Plan";
                    ClaimStatusImage = "icon_paidclaim.png";
                    break;
                case "In Process":
                    ClaimStatus = "In Process";
                    ClaimStatusByText = string.Empty;
                    ClaimStatusImage = "icon_inprocess_claim.png";
                    break;
                case "Denied":
                    ClaimStatus = "Denied";
                    ClaimStatusByText = " by UPMC Health Plan";
                    ClaimStatusImage = "icon_deniedclaim.png";
                    break;
                default:
                    break;
            }
        }
    }
    #endregion

    public class ClaimsViewModel : BaseViewModel
    {
        private bool _isMembersListUpdated = false;
        
        private ClaimPresenter _currentClaim;
        public ClaimPresenter CurrentClaim { get => _currentClaim; set =>SetProperty(ref _currentClaim, value); }
        
        private ObservableCollection<IdCardInfo> _coveredMembers;
        public ObservableCollection<IdCardInfo> CoveredMembers
        {
            get
            {
                if (_coveredMembers == null)
                    _coveredMembers = new ObservableCollection<IdCardInfo>();
                return _coveredMembers;
            }
        }

        

        private ObservableCollection<ClaimPresenter> _claimsList;
        public ObservableCollection<ClaimPresenter> ClaimsList
        {
            get
            {
                if (_claimsList == null)
                    _claimsList = new ObservableCollection<ClaimPresenter>();
                return _claimsList;
            }
        }

        public async void UpdateCoveredMembersList()
        {
            if (_isMembersListUpdated)
                return;

            IsProgressActive = true;

            var dataService = new HomeScreenDataService(ViewModelContainer.HomeVM.Session);
            var res = await dataService.GetClaimsCoveredMembers(ViewModelContainer.HomeVM.MemberId);
            if(res!=null)
            {
                _isMembersListUpdated = true;
                CoveredMembers.Clear();
                foreach(var item in res)
                {
                    CoveredMembers.Add(item);
                }
            }

            IsProgressActive = false;
        }

        public async void UpdateClaimsListForSelectMember(string selectedMember)
        {
            ClaimsList.Clear();
            IsProgressActive = true;

            var dataService = new HomeScreenDataService(ViewModelContainer.HomeVM.Session);
            var res = await dataService.GetClaimsListing(ViewModelContainer.HomeVM.MemberId, selectedMember);
            if (res != null)
            {
                foreach (var item in res)
                {
                    ClaimsList.Add(new ClaimPresenter(item));
                }
            }

            IsProgressActive = false;
        }
    }
}
