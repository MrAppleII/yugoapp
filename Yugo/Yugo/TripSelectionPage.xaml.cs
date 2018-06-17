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
		public TripSelectionPage ()
		{
			InitializeComponent ();
		}
        private async void GetJSON()
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
            }
            else
            {
                await DisplayAlert("JSONParsing", "No network is available.", "Ok");
            }
            //Hide loader after server response    
           ProgressLoader.IsVisible = false;
        }
        public void listviewContacts_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var itemSelectedData = e.SelectedItem as TripPlan;
           // Navigation.PushAsync(new GuideDetails(itemSelectedData));
            //do nothing so far
        }
        protected override void OnAppearing()
        {
            GetJSON();

        }

    }
    public class TripPlan
    {
        public string TripName { get; set; }
        public string TripDestination { get; set; }
        public string CityPic { get; set; }
        public string TripStatus { get; set; }
        public string TripID { get; set; }
    }

    

    public class TripPlanList
    {
        public List<TripPlan> Trips { get; set; }
    }
}
