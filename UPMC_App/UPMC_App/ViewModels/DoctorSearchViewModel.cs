using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.ServerDataModels;
using UPMC_App.Services;
using Xamarin.Essentials;

namespace UPMC_App.ViewModels
{
    public class DoctorSearchViewModel : BaseViewModel
    {
        public const string PHARMACY_CAT_URL = "PharmacyRetailChains";
        public const string FACILITY_CAT_URL = "FacilityTypes";

        private DoctorsService _service;

        private bool _isByName = true;
        private bool _isPrimaryCarePhisician = true;
        private bool _isFilterByCoverage = true;
        private string _nameToFind = "";
        private string _zip = "";
        private ObservableCollection<ProviderSearchItem> _searchResults;
        private ProviderSearchItem _selectedProviderItem = new ProviderSearchItem();

        private List<CategoryItem> _doctorsSpecialtyPCP = new List<CategoryItem>(), _doctorsSpecialtySpecialist = new List<CategoryItem>();

        private ObservableCollection<CategoryItem> _categories;
        private CategoryItem _selectedCategory;

        public string SearchPageTitle { get; set; }
        public bool IsByName { get => _isByName; set { SetProperty(ref _isByName, value); OnPropertyChanged(nameof(IsSearchEnabled)); } }
        public bool IsPrimaryCarePhisician
        {
            get => _isPrimaryCarePhisician;
            set
            {
                if (_isPrimaryCarePhisician != value)
                {
                    SetProperty(ref _isPrimaryCarePhisician, value);
                    FillSpecialtyList();
                    OnPropertyChanged(nameof(IsSearchEnabled));
                }
            }
        }
        public bool IsFilterByCoverage { get => _isFilterByCoverage; set => SetProperty(ref _isFilterByCoverage, value); }

        public string WhatDoesItMeanText => @"<u>What does this mean?</u>";

        public string NameToFind
        {
            get => _nameToFind;
            set
            {
                SetProperty(ref _nameToFind, value);
                OnPropertyChanged(nameof(IsSearchEnabled));
                OnPropertyChanged(nameof(IsUrgentSearchEnabled));
            }
        }
        public bool IsSearchEnabled
        {
            get
            {
                if (IsByName)
                    return !string.IsNullOrEmpty(NameToFind);
                else
                    return SelectedCategory != null;
            }
        }
        public bool IsUrgentSearchEnabled => (!string.IsNullOrEmpty(NameToFind) || Zip.Length == 5);

        public string Zip
        {
            get => _zip;
            set { SetProperty(ref _zip, value); OnPropertyChanged(nameof(IsUrgentSearchEnabled)); }
        }
        public ObservableCollection<ProviderSearchItem> SearchResults
        {
            get
            {
                if (_searchResults == null)
                    _searchResults = new ObservableCollection<ProviderSearchItem>();
                return _searchResults;
            }
        }

        public ProviderSearchItem SelectedProviderItem { get => _selectedProviderItem; set => this.SetProperty(ref _selectedProviderItem, value); }

        public ObservableCollection<CategoryItem> Categories
        {
            get
            {
                if (_categories == null)
                    _categories = new ObservableCollection<CategoryItem>();
                return _categories;
            }
        }
        public CategoryItem SelectedCategory { get => _selectedCategory; set => SetProperty(ref _selectedCategory, value); }

        public DoctorSearchViewModel()
        {
            _service = new DoctorsService(ViewModelContainer.HomeVM.Session);
        }

        private void FillSpecialtyList()
        {
            Categories.Clear();
            var source = IsPrimaryCarePhisician ? _doctorsSpecialtyPCP : _doctorsSpecialtySpecialist;
            foreach (var item in source)
                Categories.Add(item);

            SelectedCategory = Categories.FirstOrDefault();
        }

        public async Task<string> TryGetZipByCurrentLocation()
        {
            string ret = string.Empty;
            IsProgressActive = true;
            //Get location
            Location location=null;
            try
            {
                location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    ret = "Location error. Try again later or specify ZIP manually.";
                }
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
                ret = "Geolocation not supported on device.";
            }
            catch (PermissionException)
            {
                // Handle permission exception
                ret = "You must give permission to use your location by the app.";
            }
            catch (Exception)
            {
                ret = "Location error. Try again later or specify ZIP manually.";
            }

            //Get ZIP by location
            if (location!=null)
            {
                try
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);

                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        Zip = placemark.PostalCode;
                    }
                }
                catch (FeatureNotSupportedException)
                {
                    // Feature not supported on device
                    ret = "Feature not supported on device.";
                }
                catch (Exception)
                {
                    ret = "Location error. Try again later or specify ZIP manually.";
                }
            }
            IsProgressActive = false;

            return ret;
        }

        public async Task SearchDoctors()
        {
            List<ProviderSearchReqArrayItem> reqParams;
            IsProgressActive = true;
            if (IsByName)
                reqParams = _service.GetRequestParams(string.Empty, Zip, NameToFind, IsFilterByCoverage);
            else
            {
                if (SelectedCategory != null)
                    reqParams = _service.GetRequestParams(SelectedCategory.IKey.ToString(), Zip, NameToFind, IsFilterByCoverage);
                else
                    return;
            }
            var lst = await _service.SearchDoctors(ViewModelContainer.HomeVM.MemberId, reqParams);
            IsProgressActive = false;

            SearchResults.Clear();
            if (lst != null)
            {
                foreach (var item in lst)
                {
                    SearchResults.Add(item);
                }
            }
        }

        public async Task SearchPharmacies()
        {
            List<ProviderSearchReqArrayItem> reqParams;
            IsProgressActive = true;
            if (IsByName)
                reqParams = _service.GetPharmacyRequestParams(string.Empty, Zip, NameToFind, IsFilterByCoverage);
            else
            {
                reqParams = _service.GetPharmacyRequestParams(string.Empty, Zip, NameToFind, IsFilterByCoverage);
            }
            var lst = await _service.SearchPharmacies(ViewModelContainer.HomeVM.MemberId, reqParams);
            IsProgressActive = false;

            SearchResults.Clear();
            if (lst != null)
            {
                foreach (var item in lst)
                {
                    SearchResults.Add(item);
                }
            }
        }

        public async Task SearchFacilities()
        {
            List<ProviderSearchReqArrayItem> reqParams;
            IsProgressActive = true;
            if (IsByName)
                reqParams = _service.GetFacilityRequestParams(string.Empty, Zip, NameToFind, IsFilterByCoverage);
            else
            {
                reqParams = _service.GetFacilityRequestParams(string.Empty, Zip, NameToFind, IsFilterByCoverage);
            }
            var lst = await _service.SearchFacilities(ViewModelContainer.HomeVM.MemberId, reqParams);
            IsProgressActive = false;

            SearchResults.Clear();
            if (lst != null)
            {
                foreach (var item in lst)
                {
                    SearchResults.Add(item);
                }
            }
        }

        public async Task SearchUCC()
        {
            List<ProviderSearchReqArrayItem> reqParams;
            IsProgressActive = true;
            //if (IsByName)
                reqParams = _service.GetUCCRequestParams(string.Empty, Zip, NameToFind, IsFilterByCoverage);
            //else
            //{
            //    reqParams = _service.GetUCCRequestParams(string.Empty, Zip, NameToFind, IsFilterByCoverage);
            //}
            var lst = await _service.SearchUCC(ViewModelContainer.HomeVM.MemberId, reqParams);
            IsProgressActive = false;

            SearchResults.Clear();
            if (lst != null)
            {
                foreach (var item in lst)
                {
                    SearchResults.Add(item);
                }
            }
        }

        public async void LoadDoctorsSpecialty()
        {
            IsProgressActive = true;
            _doctorsSpecialtyPCP = await _service.GetDoctorsSpecialty(1);
            var lstCommon = await _service.GetDoctorsSpecialty(2);
            var lstNonCommon = await _service.GetDoctorsSpecialty(3);

            _doctorsSpecialtySpecialist.Clear();
            foreach (var item in lstCommon)
            {
                _doctorsSpecialtySpecialist.Add(item);
                var toDel = lstNonCommon.FirstOrDefault(it => it.SDesc == item.SDesc);
                if (toDel != null)
                    lstNonCommon.Remove(toDel);
            }

            foreach (var item in lstNonCommon)
                _doctorsSpecialtySpecialist.Add(item);
           
            FillSpecialtyList();
            IsProgressActive = false;
        }

        public async void LoadCategories(string categoryTypeURL)
        {
            IsProgressActive = true;
            var res = await _service.GetCategories(categoryTypeURL);
            Categories.Clear();
            if (res != null)
            {
                foreach (var item in res)
                {
                    Categories.Add(item);
                }
                SelectedCategory = Categories.FirstOrDefault();
            }
            IsProgressActive = false;
        }
    }
}
