using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UPMC_App.ServerDataModels;
using Xamarin.Forms;

namespace UPMC_App.ViewModels
{
    public class CardPresenter : BindableBase
    {
        #region Inner Classes
        public class PersonalDataOnCardItem
        {
            public string Data { get; set; }
            public double HorizontalOffset { get; set; }
            public double VerticalOffset { get; set; }
            public double MaskWidth { get; set; }
            public double MaskHeight { get; set; }
            /// <summary>
            /// 'bold' or empty
            /// </summary>
            public string TextStyle { get; set; }
        }
        #endregion

        public static int CardImageWidth = 768;
        public static int CardImageHeight = 531;

        private FamilyIdCardInfo _cardSource;
        //private byte[] _base64StreamFront, _base64StreamBack;
        public bool IsImage64String = false;
        public bool IsVisible = true;

        private string _cardImageFile_Front, _cardImageFile_Back;

        private bool _isShowFront = true;
        public bool IsShowFront
        {
            get => _isShowFront;
            set
            {
                SetProperty(ref _isShowFront, value);
                //OnPropertyChanged(nameof(CardImageDynamic));
                OnPropertyChanged(nameof(CurrentCardImage));
            }
        }

        public ImageSource CardImageDynamic
        {
            get
            {
                if (IsShowFront)
                {
                    var buf = Convert.FromBase64String(_cardSource._idCardFrontImageBase64);
                    return ImageSource.FromStream(() => new MemoryStream(buf));
                }
                else
                {
                    var buf = Convert.FromBase64String(_cardSource._idCardBackImageBase64);
                    return ImageSource.FromStream(() => new MemoryStream(buf));
                }
            }
        }

        public object CurrentCardImage
        {
            get
            {
                if (IsImage64String)
                    return CardImageDynamic;
                else
                {
                    if (IsShowFront)
                        return _cardImageFile_Front;
                    else
                        return _cardImageFile_Back;
                }
            }
        }

        public List<PersonalDataOnCardItem> PersonalDataList = new List<PersonalDataOnCardItem>();
        public List<PersonalDataOnCardItem> MaskFrontDataList = new List<PersonalDataOnCardItem>();
        public List<PersonalDataOnCardItem> MaskBackDataList = new List<PersonalDataOnCardItem>();

        public string CardTypeFriendly => _cardSource.cardType.ToUpper();
        public string Description => _cardSource.description;

        public CardPresenter() { IsVisible = false; }
        public CardPresenter(FamilyIdCardInfo card)
        {
            _cardSource = card;
            FillDataFromSource();
        }

        private void FillDataFromSource()
        {
            IsImage64String = !String.IsNullOrEmpty(_cardSource._idCardFrontImageBase64);

            //установить соответствие изображений карт
            if (!IsImage64String)
            {
                SetResourceImageForCard();
            }
        }

        private void SetResourceImageForCard()
        {
            switch (_cardSource.cardType)
            {
                case "Assist America":
                    _cardImageFile_Front = "card_Assist_America_frontSide.png";
                    _cardImageFile_Back = "card_Assist_America_backSide.png";
                    #region Masks
                    MaskFrontDataList.Add(new PersonalDataOnCardItem()
                    {
                        HorizontalOffset = 42,
                        VerticalOffset = 42,
                        MaskWidth = 704,
                        MaskHeight = 96,
                        Data = "tel:+1-800-872-1414"
                    });
                    MaskFrontDataList.Add(new PersonalDataOnCardItem()
                    {
                        HorizontalOffset = 242,
                        VerticalOffset = 156,
                        MaskWidth = 474,
                        MaskHeight = 44,
                        Data = "tel:+1-609-987-1234"
                    });
                    MaskFrontDataList.Add(new PersonalDataOnCardItem()
                    {
                        HorizontalOffset = 700,
                        VerticalOffset = 234,
                        MaskWidth = 54,
                        MaskHeight = 54,
                        Data = "showmsg:Assist America|If medical assistance is needed while traveling more than 100 miles from home, a single call to Assist America connects you with experienced medical and crisis response personnel who will make sure you receive appropriate medical care anywhere in the United States or in another country. Assist America provides immediate connections to doctors, hospitals, pharmacies, as well as medical referrals, monitoring, evacuations, repatriation, and more."
                    });
                    #endregion
                    break;
                case "Dental <i>Advantage</i> ID Card":
                    _cardImageFile_Front = "card_Dental_advantage_frontSide.png";
                    _cardImageFile_Back = "card_Dental_advantage_backSide.png";

                    #region Personal data
                    PersonalDataList.Add(new PersonalDataOnCardItem()
                    {
                        HorizontalOffset = 288,
                        VerticalOffset = 101,
                        TextStyle = "bold",
                        Data = _cardSource.memberName
                    });
                    PersonalDataList.Add(new PersonalDataOnCardItem()
                    {
                        HorizontalOffset = 346,
                        VerticalOffset = 124,
                        Data = _cardSource.groupNumber
                    });
                    #endregion

                    #region Masks
                    MaskBackDataList.Add(new PersonalDataOnCardItem()
                    {
                        HorizontalOffset = 430,
                        VerticalOffset = 28,
                        MaskWidth = 156,
                        MaskHeight = 24,
                        Data = "tel:+1-888-876-2756"
                    });
                    #endregion
                    break;
                case "UPMC Vision Care ID Card":
                    _cardImageFile_Front = "card_UPMC_Vision_Care_front.png";
                    _cardImageFile_Back = "card_UPMC_Vision_Care_back.png";

                    #region Personal Data
                    PersonalDataList.Add(new PersonalDataOnCardItem()
                    {
                        HorizontalOffset = 245,
                        VerticalOffset = 132,
                        Data = _cardSource.memberName
                    });
                    PersonalDataList.Add(new PersonalDataOnCardItem()
                    {
                        HorizontalOffset = 366,
                        VerticalOffset = 165,
                        Data = _cardSource.memberID
                    });
                    PersonalDataList.Add(new PersonalDataOnCardItem()
                    {
                        HorizontalOffset = 343,
                        VerticalOffset = 200,
                        Data = _cardSource.groupNumber
                    });
                    PersonalDataList.Add(new PersonalDataOnCardItem()
                    {
                        HorizontalOffset = 245,
                        VerticalOffset = 236,
                        Data = _cardSource.planName
                    });
                    #endregion
                    MaskBackDataList.Add(new PersonalDataOnCardItem()
                    {
                        HorizontalOffset = 172,
                        VerticalOffset = 158,
                        MaskWidth = 160,
                        MaskHeight = 20,
                        Data = "tel:+1-844-252-0687"
                    });
                    MaskBackDataList.Add(new PersonalDataOnCardItem()
                    {
                        HorizontalOffset = 74,
                        VerticalOffset = 272,
                        MaskWidth = 160,
                        MaskHeight = 20,
                        Data = "tel:+1-877-212-7870"
                    });
                    break;
                default:
                    break;
            }
        }
    }
}
