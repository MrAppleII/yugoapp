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
    public partial class LoggedInNavigation : TabbedPage
    {
        public LoggedInNavigation ()
        {
            InitializeComponent();
           
        }
       
        private async void GetCities()
        {
            //await DisplayAlert("Internet Availible", "Going forward with Method", "OK");
            //Check network status   
            if (NetworkCheck.IsInternet())
            {

                var client = new System.Net.Http.HttpClient();
                var response = await client.GetAsync("https://api.myjson.com/bins/rodzi");
                string destinationsJson = await response.Content.ReadAsStringAsync();
                DestinationList ObjCityList = new DestinationList();
                if (destinationsJson != "")
                {
                    //Converting JSON Array Objects into generic list  
                    ObjCityList = JsonConvert.DeserializeObject<DestinationList>(destinationsJson);
                }
                //Binding listview with server response    
                listviewTrip.ItemsSource = ObjCityList.Cities;
            }
            else
            {
                await DisplayAlert("JSONParsing", "No network is available.", "Ok");
            }
            //Hide loader after server response    
            ProgressLoader.IsVisible = false;
        }
        public void listviewCity_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var itemSelectedData = e.SelectedItem as Destination;
            String apiLink = itemSelectedData.ListLink;
            if (apiLink != "")
            {
                Navigation.PushAsync(new GuideSelectScreen(itemSelectedData));

            }
            else
            {
                DisplayAlert("Alert", "City is coming soon. Stay tuned!", "OK");
            }
           
           
            //do nothing so far
        }
        protected override void OnAppearing()
        {
            GetCities();

        }
    }
    public class Destination
    {
        public string CityName { get; set; }
        public string CityNameUpperCase { get { return CityName.ToUpper(); } }
        public string CountryName{ get; set; }
        public string CityPic { get; set; }
        public string ListLink { get; set; }
        public string CityID { get; set; }
    }



    public class DestinationList
    {
        public List<Destination> Cities { get; set; }
    }

}