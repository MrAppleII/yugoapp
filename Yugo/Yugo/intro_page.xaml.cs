using Newtonsoft.Json;
using Plugin.Connectivity;
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
	public partial class intro_page : ContentPage
	{
		public intro_page ()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            
            InitializeComponent();
        }
        private void SignUp_buttonclicked(object sender, EventArgs e)
        {
            //(sender as Button).Text = "I was just clicked!";
            Navigation.PushAsync(new GettingStarted(),true);
        }
    

    }

}
    public class NetworkCheck
    {
        public static bool IsInternet()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }
            else
            {
                // TODO We dont have permission to use the internet so therefore further action is needed   
                return false;
            }
        }
    }
   
