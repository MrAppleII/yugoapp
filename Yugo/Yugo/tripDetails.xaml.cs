using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Yugo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class tripDetails : ContentPage
	{
		public tripDetails ()
		{
			InitializeComponent ();
		}
        private void TripDetails_buttonclicked(object sender, EventArgs e)
        {

            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new LoggedInNavigation()));
            //Navigation.PushModalAsync(new LoggedInNavigation());
            //await Navigation.PopAsync();
        }

    }
}