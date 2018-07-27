using FFImageLoading.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Yugo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoggedInNavigation : TabbedPage
    {
       

        private int counter = 0;
        private DestinationList cityDownloadedList = new DestinationList();
        
        public LoggedInNavigation ()
        {
            InitializeComponent();
          

            cityDownloadedList.Cities = new List<Destination>();
            BindingContext = this;
           
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
                cityDownloadedList = ObjCityList;
                cityDownloadedList.Cities = ObjCityList.Cities;
                listviewTrip.ItemsSource = cityDownloadedList.Cities;
                
            }
            else
            {
                await DisplayAlert("Yugo", "No network is available.", "Ok");
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

        private  async void CityBackgroundPhoto_Success(object sender, FFImageLoading.Forms.CachedImageEvents.SuccessEventArgs e)
        {
            //TODO Finish setting the images so they can load in as they finish loading 
            if((sender as FFImageLoading.Forms.CachedImage).Opacity == 0)
            {
              
                System.Diagnostics.Debug.WriteLine((sender as FFImageLoading.Forms.CachedImage).Opacity.ToString());
                await (sender as FFImageLoading.Forms.CachedImage).FadeTo(1, 500);
                (sender as FFImageLoading.Forms.CachedImage).Opacity= 1;
            }
            else
            {
                 
            }

            // DisplayAlert("DEBUG", sender.GetType().ToString(), "OK");
        }

        private async void CityBackgroundPhoto_Finish(object sender, FFImageLoading.Forms.CachedImageEvents.FinishEventArgs e)
        {
            if(counter!= cityDownloadedList.Cities.Count())
            {
                ++counter;
            }
              if (counter == (cityDownloadedList.Cities.Count()))
            {
               // System.Diagnostics.Debug.WriteLine("MAX REACHED " + cityDownloadedList.Cities.Count() + " " + counter);
                displayAlItems();
            }
           
            //  (sender as FFImageLoading.Forms.CachedImage).Opacity = 1;
        }

        private async void displayAlItems()
        {

            await listviewTrip.FadeTo(1, 250);
        }
       


    }
    public class Destination
    {
        public int imageOpacity { get; set; } = 0;
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