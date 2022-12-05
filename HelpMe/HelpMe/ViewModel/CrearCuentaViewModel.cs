using HelpMe.Models;
using HelpMe.Views.Register;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HelpMe.ViewModel
{
    public class CrearCuentaViewModel : BaseViewModel
    {
        #region VARIABLES
        string _Texto;
        private readonly IGoogleManager _googleManager;
        GoogleUser googleUserPull = new GoogleUser();
        #endregion

        #region CONSTRUCTOR
        public CrearCuentaViewModel(INavigation navigation)
        {
            Navigation = navigation;
            _googleManager = DependencyService.Get<IGoogleManager>();
        }
        #endregion

        #region OBJETOS
        public string Texto
        {
            get { return _Texto; }
            set { SetValue(ref _Texto, value); }
        }
        #endregion

        #region PROCESOS

        public void LoguearseConGmail()
        {
            _googleManager.Login(OnLoginComplete);
        }

        public async void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                googleUserPull = googleUser;
                string[] cadena = googleUserPull.Name.Split(' ');
                googleUserPull.Name = cadena[0];
                googleUserPull.LastName = cadena[1];
                await Navigation.PushAsync(new CompletarRegistro(googleUserPull));
            }
            else
            {
                await DisplayAlert("Mensaje", message, "OK");
            }
        }


        #endregion

        #region COMANDOS
        public ICommand GmailCommand => new Command(LoguearseConGmail);

        //public ICommand ProcesoSimpcommand => new Command(ProcesoSimple);
        #endregion
    }
}
