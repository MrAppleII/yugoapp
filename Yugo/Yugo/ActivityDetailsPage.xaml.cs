using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Yugo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActivityDetailsPage : ContentPage
	{
        private CurrerntTripActivity activityToShow;
		public ActivityDetailsPage(Position userLocation, CurrerntTripActivity actvityToShowDetails)
		{
            activityToShow = actvityToShowDetails;
            Title = "More Details";
           
            InitializeComponent ();
            BindingContext = activityToShow;
            SetCurrentActivityAddress(activityToShow);
            tester();
         
		}
        private async void tester()
        {
            if (activityToShow != null)
            {
                 System.Diagnostics.Debug.WriteLine("TEST SUCCESSFUL");
            }
            else
            {
                await DisplayAlert("Debug", "Shits stupid", "OK");
            }

        }
        private void SetCurrentActivityAddress(CurrerntTripActivity givenActivity)
        {






            // await DisplayAlert("Address Retrieved", AddressLongitude + " " + AddressLongitude, "Ok");
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(givenActivity.ActivityAddressLatitude, givenActivity.ActivityAddressLongitude),
                Label = givenActivity.ActivityName,
                Address = givenActivity.ActivityAddress,
            };

            TripMap.Pins.Add(pin);
            TripMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                pin.Position, Distance.FromMiles(1.0)));

        }
        public void directionsButtonClicked()
        {
            LaunchMapApp(activityToShow);
        }
        public void LaunchMapApp(CurrerntTripActivity ActivityToGoTo)
        {
            // Windows Phone doesn't like ampersands in the names and the normal URI escaping doesn't help
            var name = ActivityToGoTo.ActivityName.Replace("&", "and"); // var name = Uri.EscapeUriString(place.Name);
            var loc = string.Format("{0},{1}", ActivityToGoTo.ActivityAddressLatitude, ActivityToGoTo.ActivityAddressLongitude);
            var addr = Uri.EscapeUriString(ActivityToGoTo.ActivityAddress);
            var request = "";
            if (Device.RuntimePlatform == Device.iOS)
            {
                //This is the code to run for iPhones
                // iOS doesn't like %s or spaces in their URLs, so manually replace spaces with +s
                request = string.Format("http://maps.apple.com/maps?q={0}&sll={1}", name.Replace(' ', '+'), loc);

            }
            else if (Device.RuntimePlatform == Device.Android)
            {

                // pass the address to Android if we have it
                request = string.Format("geo:0,0?q={0}({1})", string.IsNullOrWhiteSpace(addr) ? loc : addr, name);

            }
            else if (Device.RuntimePlatform == Device.WPF)
            {
                request = string.Format("bingmaps:?cp={0}&q={1}", loc, name);
            }


            if (request != "")
                Device.OpenUri(new Uri(request));
        }
    }
}