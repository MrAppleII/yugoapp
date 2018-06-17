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
	public partial class GuideDetails : ContentPage
	{
		public GuideDetails(Contact SelectedContact)
        {
			InitializeComponent ();
            GridDetails.BindingContext = SelectedContact;
        }
	}
}