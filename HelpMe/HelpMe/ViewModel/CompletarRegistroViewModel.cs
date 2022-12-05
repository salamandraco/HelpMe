using HelpMe.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HelpMe.ViewModel
{
    public class CompletarRegistroViewModel : BaseViewModel
    {
        #region VARIABLES
        string _Texto;
        public GoogleUser _googleUser { get; set; }
        #endregion

        #region CONSTRUCTOR
        public CompletarRegistroViewModel(INavigation navigation, GoogleUser googleUser)
        {
            Navigation = navigation;
            _googleUser = googleUser;
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
        public async Task ProcesoAsyncrono()
        {

        }

        public void ProcesoSimple()
        {

        }
        #endregion

        #region COMANDOS
        public ICommand ProcesoAsyncommand => new Command(async () => await ProcesoAsyncrono());

        public ICommand ProcesoSimpcommand => new Command(ProcesoSimple);
        #endregion
    }
}
