using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPMC_App.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UPMC_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IdCardsPage : ContentPage
	{
        //CardPresenter ViewModel;
		public IdCardsPage ()
		{
			InitializeComponent();
            LoadCards();

            //LOCAL_FILL();
		}

        private void LOCAL_FILL()
        {
            //BindingContext = new CardPresenter(new ServerDataModels.FamilyIdCardInfo() { cardType = "Assist America" });
            //BindingContext = new CardPresenter(new ServerDataModels.FamilyIdCardInfo()
            //{
            //    cardType = "Dental <i>Advantage</i> ID Card",
            //    memberName = "THORNTON MARC",
            //    groupNumber = "00-FF1524G-2"
            //});
            BindingContext = new CardPresenter(new ServerDataModels.FamilyIdCardInfo()
            {
                cardType = "UPMC Vision Care ID Card",
                memberName = "THORNTON MARC",
                groupNumber = "00-FF1524G-2",
                memberID="02125212457", planName="test Plan Name"
            });
        }

        private async void LoadCards()
        {
            if (ViewModelContainer.HomeVM.IdCards.Count == 0)
            {
                this.gridWaiting.IsVisible = true;
                await ViewModelContainer.HomeVM.LoadFamilyCards();
                this.gridWaiting.IsVisible = false;
            }

            if (ViewModelContainer.HomeVM.IdCards.Count > 0)
            {
                BindingContext = ViewModelContainer.HomeVM.GetCardPresenter();
            }
        }

        private void returnImage_Clicked(object sender, EventArgs e)
        {
            var presenter = BindingContext as CardPresenter;
            if(presenter!=null)
            {
                presenter.IsShowFront = !presenter.IsShowFront;
            }
        }

        private void sendCard_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{img.Width}x{img.Height}");
        }

        private void btnShowCards_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CardsListPage());
        }


        protected internal void SetSelectedCard(int cardIndex)
        {
            BindingContext = ViewModelContainer.HomeVM.GetCardPresenter(cardIndex);
        }

        private void cardImg_SizeChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{img.Width}x{img.Height}");
            if (!_isSourceChanged)
                UpdateCardData();
        }

        bool _isSourceChanged = false;
        private void cardImg_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Source")
            {
                _isSourceChanged = true;
                if ((BindingContext as CardPresenter).IsImage64String)
                {
                    img.HeightRequest = 531;
                    img.WidthRequest = 768;
                }
                else
                {
                    if (gridImageContainer.Width < gridImageContainer.Height)
                    {
                        img.WidthRequest = gridImageContainer.Width;
                        double scale = gridImageContainer.Width / CardPresenter.CardImageWidth;
                        img.HeightRequest = CardPresenter.CardImageHeight * scale;
                    }
                    else
                    {
                        img.HeightRequest = gridImageContainer.Height;
                        double scale = gridImageContainer.Height / CardPresenter.CardImageHeight;
                        img.WidthRequest = CardPresenter.CardImageWidth * scale;
                    }

                    //img.HeightRequest = -1;
                    //img.WidthRequest = -1;
                }
                _isSourceChanged = false;
                UpdateCardData();
            }
        }

        private void gridImageContainer_SizeChanged(object sender, EventArgs e)
        {
            //if (gridImageContainer.Width > gridImageContainer.Height)
            //{
            //    double scale = gridImageContainer.Width/ CardPresenter.CardImageWidth;
            //    img.WidthRequest = gridImageContainer.Width;
            //    img.HeightRequest = scale * CardPresenter.CardImageHeight;
            //}
            //else
            //{
            //    double scale = gridImageContainer.Height/ CardPresenter.CardImageHeight;
            //    img.HeightRequest = gridImageContainer.Height;
            //    img.WidthRequest = scale * CardPresenter.CardImageWidth;
            //}
        }

        private void UpdateCardData()
        {
            var cardInfo = BindingContext as CardPresenter;
            if(cardInfo!=null)
            {
                gridCardData.Children.Clear();
                gridCardData.HeightRequest = img.Height;
                gridCardData.WidthRequest = img.Width;

                double scale = img.Width / CardPresenter.CardImageWidth;

                if (cardInfo.IsShowFront)
                {
                    //personal data
                    foreach(var item in cardInfo.PersonalDataList)
                    {
                        gridCardData.Children.Add(new Label()
                        {
                            Text = item.Data,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                            FontAttributes = item.TextStyle == "bold" ? FontAttributes.Bold : FontAttributes.None,
                            Margin = new Thickness(item.HorizontalOffset * scale, item.VerticalOffset * scale, 0, 0)
                        });
                    }

                    //masks
                    foreach (var item in cardInfo.MaskFrontDataList)
                    {
                        Grid maskGrid = new Grid()
                        {
                            Margin = new Thickness(item.HorizontalOffset * scale, item.VerticalOffset * scale, 0, 0),
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            HeightRequest = item.MaskHeight * scale,
                            WidthRequest = item.MaskWidth * scale
                        };
                        TapGestureRecognizer maskGridTap = new TapGestureRecognizer();
                        maskGridTap.Tapped += (s, e) => TapOnMask(item.Data);
                        maskGrid.GestureRecognizers.Add(maskGridTap);

                        gridCardData.Children.Add(maskGrid);
                    }
                }
                else
                {
                    foreach(var item in cardInfo.MaskBackDataList)
                    {
                        Grid maskGrid = new Grid()
                        {
                            Margin = new Thickness(item.HorizontalOffset * scale, item.VerticalOffset * scale, 0, 0),
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start, /*BackgroundColor=Color.AliceBlue,*/
                            HeightRequest = item.MaskHeight * scale,
                            WidthRequest = item.MaskWidth * scale
                        };
                        TapGestureRecognizer maskGridTap = new TapGestureRecognizer();
                        maskGridTap.Tapped += (s, e) => TapOnMask(item.Data);
                        maskGrid.GestureRecognizers.Add(maskGridTap);

                        gridCardData.Children.Add(maskGrid);
                    }
                }
            }
        }

        private void TapOnMask(string data)
        {
            if(data.StartsWith("tel:"))
            {
                string phone = data.Replace("tel:", "");
                try
                {
                    PhoneDialer.Open(phone);
                }
                catch { DisplayAlert("UPMC", "Phone Dialer is not supported on this device!", "Cancel"); }
            }
            else if (data.StartsWith("showmsg:"))
            {
                string[] msg = data.Replace("showmsg:", "").Split('|');
                if (msg.Count() > 1)
                {
                    DisplayAlert(msg[0], msg[1], "OK");
                }
            }
        }
    }
}