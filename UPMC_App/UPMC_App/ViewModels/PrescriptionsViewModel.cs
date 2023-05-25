using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.Services;

namespace UPMC_App.ViewModels
{
    public class PrescriptionItem
    {
        public string DrugName { get; set; }
        public string PrescriptionNumberString { get; set; }
        public string DosageString { get; set; }
        public string SupplyString { get; set; }
        public int Supply { get; set; }
        public DateTime DateRefill { get; set; }
        public string DateString { get; set; }
        public string Pharmacy { get; set; }
        public string DateFriendly => DateRefill.ToShortDateString();
    }

    public class PharmacyItem : BindableBase
    {
        public string Name { get; set; }
        public bool IsFavorite { get; set; }

        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string FullCityString => $"{City}, {State} {Zip}";
        public string FavImage => IsFavorite ? "baseline_star_black_36.png" : "baseline_star_border_black_36.png";
    }


    public class PrescriptionsViewModel : BaseViewModel
    {
        bool _isPrescriptionsListUpdated = false;

        #region Current prescriptions
        private ObservableCollection<PrescriptionItem> _currentPrescriptions;
        public ObservableCollection<PrescriptionItem> CurrentPrescriptions
        {
            get
            {
                if (_currentPrescriptions == null)
                    _currentPrescriptions = new ObservableCollection<PrescriptionItem>();
                return _currentPrescriptions;
            }
        }
        public IEnumerable<PrescriptionItem> FirstCurrentPrescriptions => CurrentPrescriptions.Take(5);
        public bool IsCurrentPrescriptionsEmpty => (CurrentPrescriptions.Count == 0);

        private int _currentPrescriptionsPosition;
        public int CurrentPrescriptionsPosition { get => _currentPrescriptionsPosition; set => SetProperty(ref _currentPrescriptionsPosition, value); }
        #endregion

        #region Prescriptions history
        private ObservableCollection<PrescriptionItem> _prescriptionsHistory;
        public ObservableCollection<PrescriptionItem> PrescriptionsHistory
        {
            get
            {
                if (_prescriptionsHistory == null)
                    _prescriptionsHistory = new ObservableCollection<PrescriptionItem>();
                return _prescriptionsHistory;
            }
        }
        public IEnumerable<PrescriptionItem> FirstPrescriptionsHistory => PrescriptionsHistory.Take(5);
        public bool IsPrescriptionsHistoryEmpty => (PrescriptionsHistory.Count == 0);

        private int _currentHistoryPosition;
        public int CurrentHistoryPosition { get => _currentHistoryPosition; set => SetProperty(ref _currentHistoryPosition, value); }
        #endregion

        #region Pharmacies
        private ObservableCollection<PharmacyItem> _pharmacies;
        public ObservableCollection<PharmacyItem> Pharmacies
        {
            get
            {
                if (_pharmacies == null)
                    _pharmacies = new ObservableCollection<PharmacyItem>();
                return _pharmacies;
            }
        }

        public IEnumerable<PharmacyItem> FirstPharmacies => Pharmacies.Take(5);

        private int _currentPharmaciesPosition;
        public int CurrentPharmaciesPosition { get => _currentPharmaciesPosition; set => SetProperty(ref _currentPharmaciesPosition, value); }

        public bool IsPharmaciesEmpty => (Pharmacies.Count == 0);
        #endregion

        #region Current prescriptions details
        public bool IsDetailsForHistory { get; set; }
        private bool _isSortByDate = true;
        public bool IsSortByDate
        {
            get => _isSortByDate;
            set
            {
                SetProperty(ref _isSortByDate, value);
                OnPropertyChanged(nameof(DetailsList));
            }
        }
        public IOrderedEnumerable<PrescriptionItem> DetailsList
        {
            get
            {
                if (IsDetailsForHistory)
                {
                    if (IsSortByDate)
                        return PrescriptionsHistory.OrderByDescending(it => it.DateRefill);
                    else
                        return PrescriptionsHistory.OrderBy(it => it.DrugName);
                }
                else
                {
                    if (IsSortByDate)
                        return CurrentPrescriptions.OrderByDescending(it => it.DateRefill);
                    else
                        return CurrentPrescriptions.OrderBy(it => it.DrugName);
                }
            }
        }

        private string _currentDrugName;
        public string CurrentDrugName
        {
            get => _currentDrugName;
            set
            {
                SetProperty(ref _currentDrugName, value);
                OnPropertyChanged(nameof(CurrentDrugList));
                OnPropertyChanged(nameof(CurrentDrugHistoryTitle));
            }
        }

        public IOrderedEnumerable<PrescriptionItem> CurrentDrugList
        {
            get
            {
                if (IsDetailsForHistory)
                {
                    return PrescriptionsHistory.Where(it=>it.DrugName==CurrentDrugName).OrderBy(it => it.DateRefill);
                }
                else
                {
                    return CurrentPrescriptions.Where(it => it.DrugName == CurrentDrugName).OrderBy(it => it.DateRefill);
                }
            }
        }

        public string HistoryPageTitle
        {
            get
            {
                if (IsDetailsForHistory)
                    return "Prescription History";
                else
                    return "Current Prescriptions";
            }
        }

        public string HistoryResultsTitle
        {
            get
            {
                int cnt = DetailsList.Count();
                if (cnt == 1)
                    return "1 RESULT SHOWN";
                else
                    return $"{cnt} RESULTS SHOWN";
            }
        }

        public string CurrentDrugHistoryTitle => $"{CurrentDrugName} History";
        #endregion

        public async void UpdatePrescriptionsList(string memberId)
        {
            if (_isPrescriptionsListUpdated)
                return;

            IsProgressActive = true;

            await LoadPrescriptions(memberId);
            _isPrescriptionsListUpdated = true;

            IsProgressActive = false;
        }

        private async Task LoadPrescriptions(string memberId)
        {
            PrescriptionsService dataService = new PrescriptionsService(ViewModelContainer.HomeVM.Session);
            var prescriptions = await dataService.GetPrescriptions(memberId);
            CurrentPrescriptions.Clear();
            Pharmacies.Clear();
            PrescriptionsHistory.Clear();
            foreach(var container in prescriptions)
            {
                foreach(var item in container.currentPrescriptionList)
                {
                    var prescr = new PrescriptionItem()
                    {
                        DrugName = item._DrugName,
                        PrescriptionNumberString = $"Rx #: {item.prescriptionNumber}",
                        Pharmacy = item._PharmacyName,
                        DateString = $"{item._ServiceDateString} date filled",
                        DosageString = item.dosage,
                        SupplyString = $"{item.daySupply} count",
                        Supply = (int)item.daySupply
                    };
                    if (DateTime.TryParseExact(item._ServiceDateString, "M/d/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime date))
                        prescr.DateRefill = date;
                    else
                        prescr.DateRefill = DateTime.MinValue;
                    CurrentPrescriptions.Add(prescr);
                }
                OnPropertyChanged(nameof(FirstCurrentPrescriptions));
                OnPropertyChanged(nameof(IsCurrentPrescriptionsEmpty));

                foreach(var item in container.pharmacyList)
                {
                    Pharmacies.Add(new PharmacyItem()
                    {
                        Name = item.pharmacyName,
                        IsFavorite = item.isFavorite,
                        AddressLine = item.addressInfo.addressLine,
                        City = item.addressInfo.city,
                        State = item.addressInfo.state,
                        Zip = item.addressInfo.zip,
                        Phone = item.addressInfo.phone
                    });
                }
                OnPropertyChanged(nameof(FirstPharmacies));
                OnPropertyChanged(nameof(IsPharmaciesEmpty));

                foreach (var item in container.prescriptionDetailsList)
                {
                    var prescr = new PrescriptionItem()
                    {
                        DrugName = item._DrugName,
                        PrescriptionNumberString = $"Rx #: {item.prescriptionNumber}",
                        Pharmacy = item._PharmacyName,
                        DateString = $"{item._ServiceDateString} date filled",
                        DosageString = item.dosage,
                        SupplyString = $"{item.daySupply} count",
                        Supply = (int)item.daySupply
                    };
                    if (DateTime.TryParseExact(item._ServiceDateString, "M/d/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime date))
                        prescr.DateRefill = date;
                    else
                        prescr.DateRefill = DateTime.MinValue;
                    PrescriptionsHistory.Add(prescr);
                }
                OnPropertyChanged(nameof(FirstPrescriptionsHistory));
                OnPropertyChanged(nameof(IsPrescriptionsHistoryEmpty));
            }
        }
    }
}
