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
	public partial class GettingStarted : ContentPage
	{
		public GettingStarted ()
		{
			InitializeComponent ();
		}
        private void GS_buttonclicked(object sender, EventArgs e)
        {
            //(sender as Button).Text = "I was just clicked!";
            Navigation.PushAsync(new tripDetails());
        }

    }
}