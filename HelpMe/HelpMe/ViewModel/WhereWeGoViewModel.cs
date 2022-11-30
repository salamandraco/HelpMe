using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms;
using HelpMe.Models;
using HelpMe.Services;

namespace HelpMe.ViewModel
{
    public class WhereWeGoViewModel : BaseViewModel
    {

        #region VARIABLES
        List<GooglePlaceAutoCompletePrediction> _ListAddress;
        private readonly IGoogleMapsApiService _googleMapsApi = new GoogleMapsApiService();
        Xamarin.Forms.GoogleMaps.Map _mapa;
        Pin punto = new Pin();

        string _txtOrigen;
        string _txtDestino;

        double latOrigen = 0;
        double lonOrigen = 0;
        double latDestino = 0;
        double lonDestino = 0;

        bool _selectOrigen;
        bool _selectDestino;
        bool _enabledTxtOrigen;
        bool _enabledTxtDestino;
        bool _visibleListAddress;

        #endregion

        #region CONSTRUCTOR

        public WhereWeGoViewModel(INavigation navigation, Xamarin.Forms.GoogleMaps.Map mapa)
        {
            Navigation = navigation;
            _mapa = mapa;
            EnabledTxtOrigen = false;
            EnabledTxtDestino = false;
            SelectTxtOrigen = false;
            SelectTxtDestino = false;
            VisibleListAddress = true;

            CurrentPin();
        }
        #endregion

        #region OBJETOS

        public bool EnabledTxtOrigen
        {
            get { return _enabledTxtOrigen; }
            set { SetValue(ref _enabledTxtOrigen, value); }
        }

        public bool EnabledTxtDestino
        {
            get { return _enabledTxtDestino; }
            set { SetValue(ref _enabledTxtDestino, value); }
        }

        public bool SelectTxtOrigen
        {
            get { return _selectOrigen; }
            set { SetValue(ref _selectOrigen, value); }
        }

        public bool SelectTxtDestino
        {
            get { return _selectDestino; }
            set { SetValue(ref _selectDestino, value); }
        }

        public string TxtOrigen
        {
            get { return _txtOrigen; }
            set { SetValue(ref _txtOrigen, value); }
        }

        public string TxtDestino
        {
            get { return _txtDestino; }
            set { SetValue(ref _txtDestino, value); }
        }

        public List<GooglePlaceAutoCompletePrediction> ListAddress
        {
            get { return _ListAddress; }
            set { SetValue(ref _ListAddress, value); }
        }

        public bool VisibleListAddress
        {
            get { return _visibleListAddress; }
            set { SetValue(ref _visibleListAddress, value); }
        }
        #endregion

        #region PROCESOS

        public async Task CurrentLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(30),
                    });

                    if (location != null)
                    {
                        latOrigen = location.Latitude;
                        lonOrigen = location.Longitude;

                        var position = new Position(latOrigen, lonOrigen);
                        punto.Position = new Position(latOrigen, lonOrigen);

                        _mapa.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMeters(500)));

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task CurrentPin()
        {
            punto = new Pin()
            {
                Label = "Tu ubicación",
                Type = PinType.Place,
                Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("pin.png") : BitmapDescriptorFactory.FromView(new Image()
                {
                    Source = "pin.png",
                    WidthRequest = 64,
                    HeightRequest = 64,
                }),
                Position = new Position(0, 0)
            };

            _mapa.Pins.Add(punto);
            await CurrentLocation();
        }

        private async Task SearchAddress(string txtSearch)
        {
            var places = await _googleMapsApi.ApiPlaces(txtSearch);
            var placesResult = places.AutoCompletePlaces;

            if (placesResult != null && placesResult.Count > 0)
            {
                ListAddress = new List<GooglePlaceAutoCompletePrediction>(placesResult);
            }
        }

        private async Task SelectAddress(GooglePlaceAutoCompletePrediction model)
        {
            var place = await _googleMapsApi.ApiPlacesDetails(model.PlaceId);

            if (place != null)
            {
                if (_selectOrigen == true)
                {
                    latOrigen = place.Latitude;
                    lonOrigen = place.Longitude;
                    TxtOrigen = place.Name;
                }
                if (_selectDestino == true)
                {
                    latDestino = place.Latitude;
                    lonDestino = place.Longitude;
                    TxtDestino = place.Name;
                }
                VisibleListAddress = false;
            }
        }

        private void SelectOrigen()
        {
            SelectTxtOrigen = true;
            SelectTxtDestino = false;
            EnabledTxtOrigen = true;
            EnabledTxtDestino = false;
            VisibleListAddress = true;
        }

        private void SelectDestino()
        {
            SelectTxtOrigen = false;
            SelectTxtDestino = true;
            EnabledTxtOrigen = false;
            EnabledTxtDestino = true;
            VisibleListAddress = true;
        }


        #endregion

        #region COMANDOS

        public ICommand SearchCommand => new Command<string>(async (s) => await SearchAddress(s));

        public ICommand SelectOrigenCommand => new Command(SelectOrigen);

        public ICommand SelectDestinoCommand => new Command(SelectDestino);

        public ICommand SelectAddressCommand => new Command<GooglePlaceAutoCompletePrediction>(async (s) => await SelectAddress(s));

        #endregion

    }
}
