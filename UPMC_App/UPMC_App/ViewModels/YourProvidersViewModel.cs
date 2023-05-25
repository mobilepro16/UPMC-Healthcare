using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using UPMC_App.ServerDataModels;
using UPMC_App.Services;

namespace UPMC_App.ViewModels
{
    public class YourProvidersViewModel : BaseViewModel
    {
        private bool _isSortLastVisit=true;
        private bool _isSortName;
        private bool _isSortSpecialty;
        private bool _isProviderListUpdated = false;
        private bool _isEditEnabled = false;
        private string _memberId = "";

        ProvidersServices _dataService = new ProvidersServices(ViewModelContainer.HomeVM.Session);


        private ObservableCollection<ProviderHistoryItem> _providersList;

        #region Sorting
        public bool IsSortLastVisit
        {
            get => _isSortLastVisit; set
            {
                SetProperty(ref _isSortLastVisit, value);
                if (value)
                {
                    IsSortName = false;
                    IsSortSpecialty = false;
                }
            }
        }
        public bool IsSortName
        {
            get => _isSortName;
            set
            {
                SetProperty(ref _isSortName, value);
                if (value)
                {
                    IsSortLastVisit = false;
                    IsSortSpecialty = false;
                }
            }
        }
        public bool IsSortSpecialty
        {
            get => _isSortSpecialty;
            set
            {
                SetProperty(ref _isSortSpecialty, value);
                if (value)
                {
                    IsSortLastVisit = false;
                    IsSortName = false;
                }
            }
        }
        #endregion

        public ObservableCollection<ProviderHistoryItem> ProvidersList
        {
            get
            {
                if (_providersList == null)
                    _providersList = new ObservableCollection<ProviderHistoryItem>();
                return _providersList;
            }
        }

        public bool IsEditEnabled { get => _isEditEnabled; set => SetProperty(ref _isEditEnabled, value); }

        public async void UpdateProvidersList(string memberId)
        {
            _memberId = memberId;
            if (_isProviderListUpdated)
                return;

            IsProgressActive = true;

            await LoadProvidersList(memberId);
            _isProviderListUpdated = true;

            IsProgressActive = false;
        }

        private async Task LoadProvidersList(string memberId)
        {
            
            //var providers = await dataService.GetProviders(memberId);
            var providers = await _dataService.GetMyProviders(memberId); 

            if (providers!=null)
            {
                if (IsSortLastVisit)
                    providers = providers.OrderByDescending(it => it.DateOfService).ToList();
                else if (IsSortName)
                    providers = providers.OrderBy(it => it.ProviderName).ToList();
                else
                    providers = providers.OrderBy(it => it.Specialty).ToList();

                ProvidersList.Clear();
                foreach (var item in providers)
                    ProvidersList.Add(item);
                ProvidersList.Add(new ProviderHistoryItem() { ItemType = 1 });
                ProvidersList.Add(new ProviderHistoryItem() { ItemType = 2 });
            }
            else
            {
                //ErrorNotify();
            }
        }

        public void SortProvidersList()
        {
            IEnumerable<ProviderHistoryItem> data;
            if (IsSortLastVisit)
                data = ProvidersList.Where(it=>it.ItemType==0).OrderByDescending(it => it.DateOfService);
            else if (IsSortName)
                data = ProvidersList.Where(it => it.ItemType == 0).OrderBy(it => it.ProviderName);
            else
                data = ProvidersList.Where(it => it.ItemType == 0).OrderBy(it => it.Specialty);

            ProvidersList.Clear();
            foreach(var item in data)
                ProvidersList.Add(item);
            ProvidersList.Add(new ProviderHistoryItem() { ItemType = 1 });
            ProvidersList.Add(new ProviderHistoryItem() { ItemType = 2 });
        }

        public async Task<bool> DeleteProvider(string providerKey)
        {
            var res = await _dataService.DeleteProvider(_memberId, providerKey);
            return res;
        }
    }
}
