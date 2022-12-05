using HelpMe.Models;
using HelpMe.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HelpMe.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompletarRegistro : ContentPage
    {
        public CompletarRegistro(GoogleUser modelo)
        {
            InitializeComponent();
            BindingContext = new CompletarRegistroViewModel(Navigation, modelo);
        }
    }
}