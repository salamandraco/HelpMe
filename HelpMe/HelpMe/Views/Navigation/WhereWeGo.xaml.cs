using HelpMe.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HelpMe.Views.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WhereWeGo : ContentPage
    {
        public WhereWeGo()
        {
            InitializeComponent();
            BindingContext = new WhereWeGoViewModel(Navigation, MapaGo);
        }
    }
}