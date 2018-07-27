using FFImageLoading.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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
        private PlaceDetails detailsDownloaded = new PlaceDetails();
       
        //WARNING THIS API KEY IS EMBEDDED DIRECTLY IN CODE DANGEROUS
        private string apiKey = "AIzaSyBxqvljaUOQb3-j0G_57fkXYQrooQA5Q-c";
        //TODO PLEASE DELETE AND REPLACE WITH A MORE SECURE WAY TO STORE THE KEY
        ObservableCollection<View> _myItemsSource;
       
        public ObservableCollection<View> MyItemsSource
        {
            set
            {
                _myItemsSource = value;
                OnPropertyChanged("MyItemsSource");
            }
            get
            {
                return _myItemsSource;
            }
        }
        public ActivityDetailsPage(Position userLocation, CurrerntTripActivity actvityToShowDetails)
		{
            InitializeComponent();
            MainLayout.IsVisible = false;
            carousel.IsVisible = false;
            activityToShow = actvityToShowDetails;
            Title = "More Details";
            
            
            String testLink = actvityToShowDetails.ActivityPicture;
            
            
            detailsDownloaded.result = new Result();
            SetCurrentActivityAddress(activityToShow);
            getPlaceDatails(getPlaceId(activityToShow.ActivityName));
          //  carousel.ItemsSource = MyItemsSource;
            



        }
        /* Parameters: None
         * Output: None/ of void type
         *  This is a method designed get additional details through google's api. Then it adds it to the view.
         *  Currently it designed to get website info, hours info, and photos
         * 
         * 
         * 
         */
        private void addAdditionalDetails()
        {
            MyItemsSource = new ObservableCollection<View>();
            if (!(string.IsNullOrWhiteSpace(detailsDownloaded.result.website)))
            {
                activityToShow.ActivityWebsite = detailsDownloaded.result.website;
                Console.Out.WriteLine("HERE IS WHAT WAS COPIED:"+detailsDownloaded.result.website);
                websiteBlock.IsVisible = true;
            }
            if (detailsDownloaded.result.photos != null)
            {
              //  Console.Out.WriteLine(detailsDownloaded.result.photos.Count);
                for(int i=0;i< detailsDownloaded.result.photos.Count; ++i)
                {
                    string currentLink = getGoogleImageUrl(detailsDownloaded.result.photos.ElementAt(i).photo_reference);
                  //  Console.Out.WriteLine(currentLink);
                    if(i<4)
                    MyItemsSource.Add(new CachedImage() { Source = currentLink, DownsampleToViewSize = false,FadeAnimationEnabled=true,  Aspect = Aspect.AspectFill });
                }
                

            }
            if (!(string.IsNullOrWhiteSpace(detailsDownloaded.result.opening_hours.open_now)))
            {
                Console.Out.WriteLine(detailsDownloaded.result.opening_hours.open_now);
            }
            DateTime dt = DateTime.Now;

            var date = (int)dt.DayOfWeek;
            switch (date)
            {
                case 0:
                    // Sunday ,0, is value 6
                    date = 6;
                    break;
                case 1:
                    // Mon ,1, is value 0
                    date = 0;
                    break;
                case 2:
                    // Tues ,2, is value 1
                    date = 1;
                    break;
                case 3:
                    // Wed ,3, is value 2

                    date = 2;
                    break;
                case 4:
                    // Thurs ,4, is value 3
                    date = 3;
                    break;
                case 5:
                    // Friday ,5, is value 4
                    date = 4;
                    break;
                case 6:
                    // Sat ,6, is value 5
                    date = 5;
                    break;
                default:

                    break;
            }
            object robotoFont;
            App.Current.Resources.TryGetValue("Roboto-Regular",out robotoFont);
            Console.WriteLine(robotoFont);
            if (detailsDownloaded.result.opening_hours.weekday_text.Count != 0)
            {
                for (int i = 0; i < 7; ++i)
                {
                    if (!(string.IsNullOrWhiteSpace(detailsDownloaded.result.opening_hours.weekday_text.ElementAt(i))))
                    {

                        hoursOpenBlock.Children.Add(new Label { Text = detailsDownloaded.result.opening_hours.weekday_text.ElementAt(i), FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), Margin = 10, TextColor = Color.White  });
                        }
                }
                hoursOpenBlock.Children.Add(new BoxView { HorizontalOptions = LayoutOptions.Fill , HeightRequest = 1, BackgroundColor = Color.Black });
                hoursOpenBlock.IsVisible = true;
            }
           
         


            carousel.ItemsSource = MyItemsSource;

            CarousalProgressLoader.IsVisible = false;
            MainLayout.IsVisible = true;
            carousel.IsVisible = true;

            BindingContext = activityToShow;
           


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
        private string getGoogleImageUrl(string photoRefID)
        {
            int maxHeight = 1200;
            string photoReference = photoRefID;
            string photoParameters = string.Format("maxheight={0}&photoreference={1}", maxHeight, photoReference); 
           var resultingLink = string.Format("https://maps.googleapis.com/maps/api/place/photo?{0}&key={1}", photoParameters, apiKey);
            return resultingLink;
        }
        private string getPlaceId(string placeName)
        {

            // reference of place
            String formattedName = placeName.Replace(" ", "%20"); //Repalce all spaces  
            formattedName.ToLower();

           
         
            string UrlToUse = string.Format("https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input={0}&inputtype=textquery&fields=place_id&key={1}", formattedName, apiKey);
            System.Diagnostics.Debug.WriteLine("METHOD FINISHED " + UrlToUse);

            var request = HttpWebRequest.Create(UrlToUse);
            request.ContentType = "application/json";
            request.Method = "GET";
            string content = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Console.Out.WriteLine("Response contained empty body...");
                    }
                    else
                    {
                        Console.Out.WriteLine("Response Body: \r\n {0}", content);
                    }


                }
            }
            JObject googleSearch = JObject.Parse(content);

            // get JSON result objects into a list
            IList<JToken> results = googleSearch["candidates"].Children().ToList();
           
            // serialize JSON results into .NET objects
            IList<PlaceIdSarchResults> searchResults = new List<PlaceIdSarchResults>();
            foreach (JToken result in results)
            {
                // JToken.ToObject is a helper method that uses JsonSerializer internally
                PlaceIdSarchResults searchResult = result.ToObject<PlaceIdSarchResults>();
                searchResults.Add(searchResult);
            }
            Console.Out.WriteLine("RETURNED RESULTS: "+searchResults.ElementAt(0).place_id);

            return searchResults.ElementAt(0).place_id;


        }
        public async void getPlaceDatails(string placeId)
        {
           
            string UrlToUse = string.Format("https://maps.googleapis.com/maps/api/place/details/json?placeid={0}&fields=name,website,opening_hours,photos,international_phone_number,formatted_phone_number&key={1}", placeId, apiKey);
            
            
            if (NetworkCheck.IsInternet())
            {

                var client = new System.Net.Http.HttpClient();
                var response = await client.GetAsync(UrlToUse);
                string placeDetailsJson = await response.Content.ReadAsStringAsync();
                PlaceDetails ActivityObjList = new PlaceDetails();
                if (placeDetailsJson != "")
                {
                    //Converting JSON Array Objects into generic list  
                    ActivityObjList = JsonConvert.DeserializeObject<PlaceDetails>(placeDetailsJson);

                    Console.Out.WriteLine(placeDetailsJson);

                       detailsDownloaded = ActivityObjList;
                       addAdditionalDetails();
                    // SetCurrentActivityAddress();
                }
                else
                {
                    await DisplayAlert("ERROR", "Unable to load more details.", "Ok");
                }
               
            }
            else
            {
                await DisplayAlert("ERROR", "No network is available.", "Ok");
            }
            

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
public class PlaceIdSarchResults
{
    public string place_id { get; set; }
   
}
public class PlaceDetails
{
  public Result result { get; set; }
}
public class Result
{
    public string name { get; set; }
    public string website { get; set; }
    public string formatted_phone_number { get; set; }
    public string international_phone_number { get; set; }
   public placeHours opening_hours { get; set; }
    public List<Photo> photos { get; set; }
}
public class Photo
{
    public string photo_reference { get; set; }
}
public class placeHours
{
    public string open_now { get; set; }
    public List<string> weekday_text { get; set; }

}
