using Newtonsoft.Json;
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
	public partial class TripSelectionPage : ContentPage

	{
        private int ListLength = 0;
        private int counter = 0;
		public TripSelectionPage ()
		{
			InitializeComponent ();
		}
        private async void GetCurrentTrips()
        {
            //await DisplayAlert("Internet Availible", "Going forward with Method", "OK");
            //Check network status   
            if (NetworkCheck.IsInternet())
            {

                var client = new System.Net.Http.HttpClient();
                var response = await client.GetAsync("https://api.myjson.com/bins/hnuti");
                string contactsJson = await response.Content.ReadAsStringAsync();
                TripPlanList ObjTripList = new TripPlanList();
                if (contactsJson != "")
                {
                    //Converting JSON Array Objects into generic list  
                    ObjTripList = JsonConvert.DeserializeObject<TripPlanList>(contactsJson);
                }
                //Binding listview with server response    
                listviewTrip.ItemsSource = ObjTripList.Trips;
                ListLength = ObjTripList.Trips.Count();
            }
            else
            {
                await DisplayAlert("Oh no!", "No network is available.", "Ok");
            }
            //Hide loader after server response    
           ProgressLoader.IsVisible = false;
        }
        public void listviewContacts_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var itemSelectedData = e.SelectedItem as TripPlan;
           // DisplayAlert("Trip Selected!", itemSelectedData.TripDestination, "Ok");
             Navigation.PushAsync(new TripPlanDisplay(itemSelectedData));
            //do nothing so far
        }
        protected override void OnAppearing()
        {
            GetCurrentTrips();

        }

        private void CachedImage_Success(object sender, FFImageLoading.Forms.CachedImageEvents.SuccessEventArgs e)
        {
            if (counter != ListLength)
            {
                ++counter;
            }
            if (counter == ListLength)
            {
                // System.Diagnostics.Debug.WriteLine("MAX REACHED " + cityDownloadedList.Cities.Count() + " " + counter);
                displayAlItems();
            }

        }
        private async void displayAlItems()
        {

            await listviewTrip.FadeTo(1, 250);
        }
    }
    public class TripPlan
    {
        public string TripName { get; set; }
        public string TripDestination { get; set; }
        public string CityPic { get; set; }
        public string TripStatus { get; set; }
        public string TripID { get; set; }
        public string TripApiLink { get; set; }
    }

    

    public class TripPlanList
    {
        public List<TripPlan> Trips { get; set; }
    }
}
