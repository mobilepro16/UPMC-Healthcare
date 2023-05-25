using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.Services;
using Xamarin.Forms;

namespace UPMC_App.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public event EventHandler LoggedOut;
        #region Service properties
        public Assembly SvgAssembly => typeof(App).GetTypeInfo().Assembly;

        private double _chapterTitleFontSize= Device.GetNamedSize(NamedSize.Default, typeof(Label));
        public double ChapterTitleFontSize { get => _chapterTitleFontSize; set =>SetProperty(ref _chapterTitleFontSize, value); }
        #endregion

        private string _currentSession = "";
        private string _memberName;
        private string _memberId="";
        private string _welcomeText="";
        private bool _isMemberIdVisible = false;
        private bool _isIdBlockLoading;
        private bool _isClaimBlockLoading;
        private bool _isPrescriptionBlockLoading;

        public string MemberId => _memberId;
        public string Session => _currentSession;

        #region IdBlock
        public string MemberName { get => _memberName; set { SetProperty(ref _memberName, value); OnPropertyChanged(nameof(WelcomeTitle)); } }
        public string WelcomeTitle { get => $"{_welcomeText}, {(String.IsNullOrEmpty(_memberName) ? "NOT_NAME" : _memberName)}"; }
        public bool IsMemberIdVisible
        {
            get => _isMemberIdVisible;
            set
            {
                SetProperty(ref _isMemberIdVisible, value);
                OnPropertyChanged(nameof(MemberIdFriendly));
                OnPropertyChanged(nameof(ShowMemberIdText));
            }
        }
        public string MemberIdFriendly
        {
            get
            {
                if (IsMemberIdVisible)
                    return _memberId;
                else
                {
                    if (_memberId.Length > 8)
                        return $"*******{_memberId.Substring(7)}";
                    else
                        return "*******";
                }
            }
        }
        public string ShowMemberIdText { get => IsMemberIdVisible ? "Hide" : "Show"; }
        public bool IsIdBlockLoading { get => _isIdBlockLoading; set =>SetProperty(ref _isIdBlockLoading, value); }
        #endregion

        #region Cards
        private ObservableCollection<CardPresenter> _idCards;
        public ObservableCollection<CardPresenter> IdCards
        {
            get
            {
                if (_idCards == null)
                    _idCards = new ObservableCollection<CardPresenter>();
                return _idCards;
            }
        }

        public CardPresenter GetCardPresenter(int cardIndex = 0)
        {
            if (IdCards.Count > cardIndex)
                return IdCards[cardIndex];
            else
                return new CardPresenter();
        }
        #endregion

        #region ClaimBlock
        private ClaimPresenter _firstClaim;
        private ClaimPresenter _secondClaim;
        public bool HasClaims => (FirstClaim._baseInfo != null);
        public bool HasSecondClaim => (HasClaims && (SecondClaim._baseInfo != null));
        public bool IsClaimBlockLoading { get => _isClaimBlockLoading; set => SetProperty(ref _isClaimBlockLoading, value); }
        public ClaimPresenter FirstClaim
        {
            get
            {
                if (_firstClaim == null)
                    _firstClaim = new ClaimPresenter();
                return _firstClaim;
            }
            set
            {
                SetProperty(ref _firstClaim, value);
                OnPropertyChanged(nameof(HasClaims));
            }
        }
        public ClaimPresenter SecondClaim {
            get
            {
                if (_secondClaim == null)
                    _secondClaim = new ClaimPresenter();
                return _secondClaim;
            }
            set
            {
                SetProperty(ref _secondClaim, value);
                OnPropertyChanged(nameof(HasClaims));
                OnPropertyChanged(nameof(HasSecondClaim));
            }
        }
        #endregion

        #region PrescriptionBlock
        public bool IsPrescriptionBlockLoading { get => _isPrescriptionBlockLoading; set => SetProperty(ref _isPrescriptionBlockLoading, value); }
        #endregion

        public void FillDataFromAuthResult(ServerDataModels.Api_AuthResponse response)
        {
            var userData = response.UserData != null ? response.UserData : response.UserDataNew;
            _welcomeText = userData.welcomeText;
            MemberName = userData.memberName;

            _memberId = userData.memberid;
            OnPropertyChanged(nameof(MemberIdFriendly));

            _currentSession = userData.sessionKey;

            ViewModelContainer.MenuVM.FillMenuFromAuthResponse(response.Menu);

            LoadHomeScreenData();
        }

        private void LoadHomeScreenData()
        {
            if (String.IsNullOrEmpty(_currentSession))
                return;

            IsIdBlockLoading = IsClaimBlockLoading = true;
            var dataService = new HomeScreenDataService(_currentSession);

            LoadIdBlock(dataService);
            LoadClaimBlock(dataService);
            //LoadProviders();
        }

        private async void LoadIdBlock(HomeScreenDataService dataService)
        {
            var idCards = await dataService.GetIdCards(_memberId);
            if((idCards!=null)&&(idCards.Count>0))
            {
                MemberName = idCards[0]._MemberName.Trim();
            }
            
            IsIdBlockLoading = false;
        }
        private async void LoadClaimBlock(HomeScreenDataService dataService)
        {
            //var res = await dataService.GetClaimsListing(_memberId, _memberId);
            //var res = await dataService.GetClaimsCoveredMembers(_memberId);
            var res = await dataService.GetRecentClaimsData(_memberId);
            if(res!=null)
            {
                if (res.Count > 0)
                    FirstClaim = new ClaimPresenter(res[0]);

                if (res.Count > 1)
                    SecondClaim = new ClaimPresenter(res[1]);
            }


            IsClaimBlockLoading = false;
        }

        public async Task LoadFamilyCards()
        {
            var dataService = new HomeScreenDataService(_currentSession);
            var familyCards = await dataService.GetFamilyIdCards(_memberId);
            IdCards.Clear();
            if (familyCards != null)
            {
                foreach (var item in familyCards)
                {
                    IdCards.Add(new CardPresenter(item));
                }
            }
        }

        public void OpenPageFromHomePage(string pageToken = "")
        {

        }

        public void Logout()
        {
            _currentSession = "";
            CrossSettings.Current.Remove("storedUsername");
            CrossSettings.Current.Remove("storedPass");

            if (LoggedOut != null)
                LoggedOut.Invoke(this, null);
        }
    }
}
