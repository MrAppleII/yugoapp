
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Yugo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripPlanDisplay : ContentPage , INotifyPropertyChanged
    {
        // VARIABLES FOR OUR MAIN CLASS
        TripPlan ourTrip = null;
        private ActivityList givenActivities = null;
        private CurrerntTripActivity _SelectedActivity = null;
        private int _TotalDays = 2;
        private Position userPosition;
        private int _CurrentDay = 1;
        private string _titleText = "";
        private List<ActivityList> ListOfDayPlans = new List<ActivityList>(); //Currently making a list of lists
        public event PropertyChangedEventHandler MainPropertyChanged;
        protected virtual  void OnPropertyChanged(string propertyName)
        {
            MainPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string CurrentDayText 
        {
            get
            {
                return _titleText;
            }
            set
            {
                this.Title = value;
                _titleText = value;
                OnPropertyChanged(nameof(CurrentDayText));

            }
        }
        //END VARIABLES 
        public TripPlanDisplay(TripPlan tripGiven)
        {
            InitializeComponent();
            /* When toolbar item code is write cs page */
            ourTrip = tripGiven;
            var NextDayButton = new ToolbarItem
            {
                Text = "Next",
                Command = new Command(() =>
                {

                    NextDay_ClickedAsync();
                })

            };
           
            var PrevDayButton = new ToolbarItem  
             {  
                 Text = "Prev",  
                 Order = ToolbarItemOrder.Primary,
                  Command = new Command(() =>
                  {

                      PrevDay_ClickedAsync();
                  })
            };
            this.ToolbarItems.Add(PrevDayButton);

            this.ToolbarItems.Add(NextDayButton);
            




            // this.ToolbarItems.Add(Item2); 
            BindingContext = this;
            TripNameUI.Text = ourTrip.TripName;
            CurrentDayText = "Day "+_CurrentDay+" of "+_TotalDays;
            GetCities();
            



        }

        private async void PrevDay_ClickedAsync()
        {
            if (_CurrentDay > 1)
            {
                _CurrentDay -= 1;
                CurrentDayText = "Day " + _CurrentDay + " of " + _TotalDays;

                ProgressLoader.IsVisible = true;
                await SetCoordinatesForActivityAddress(ListOfDayPlans.ElementAt(_CurrentDay - 1));
                listviewActivities.ItemsSource = ListOfDayPlans.ElementAt(_CurrentDay - 1).Activities;
                ProgressLoader.IsVisible = false;

            }
           
            CurrentDayText = "Day " + _CurrentDay + " of " + _TotalDays;
        }

        private async void NextDay_ClickedAsync()
        {
            if (_CurrentDay < _TotalDays)
            {
                _CurrentDay += 1;
                CurrentDayText = "Day " + _CurrentDay + " of " + _TotalDays;

                ProgressLoader.IsVisible = true;
                await SetCoordinatesForActivityAddress(ListOfDayPlans.ElementAt(_CurrentDay - 1));
                listviewActivities.ItemsSource = ListOfDayPlans.ElementAt(_CurrentDay - 1).Activities;
                ProgressLoader.IsVisible = false;

            }
            else
            {
               await DisplayAlert("Debug", "MAX DAYS REACHED", "OKAY");
            }
            CurrentDayText = "Day " + _CurrentDay + " of " + _TotalDays;
        }
        private void OrganizeListByDay(ActivityList sentActivityList)
        {
            
            for (int i = 0; i < _TotalDays; i++)
            {

                ActivityList currentlyWorkedOnList = new ActivityList(); //Gotta initialize your damn variables
                currentlyWorkedOnList.Activities = new List<CurrerntTripActivity>(); //This is a list of Activities 
                int elementsAdded = 0;
                for (int j = 0; j < sentActivityList.Activities.Count(); j++) //Start at day 1, go through each item every time sadly
                {
                    
                    if (sentActivityList.Activities.ElementAt(j).DayOfPlan() == (i+1))
                    {

                        System.Diagnostics.Debug.WriteLine("Day FOUND "+sentActivityList.Activities.ElementAt(j).DayOfPlan());
                        System.Diagnostics.Debug.WriteLine("ACTIVITY ADDED " + sentActivityList.Activities.ElementAt(j).ActivityName);

                        currentlyWorkedOnList.Activities.Add(sentActivityList.Activities.ElementAt(j));
                        elementsAdded += 1;
                    }
                }
               
                ListOfDayPlans.Add(currentlyWorkedOnList);

            }
          
            for(int i = 0; i < ListOfDayPlans.Count(); i++)
            {
                System.Diagnostics.Debug.WriteLine("For the day: " +(i+1) + ", there are " + ListOfDayPlans.ElementAt(i).Activities.Count() + " elements");
                for (int j = 0; j < ListOfDayPlans.ElementAt(i).Activities.Count(); j++) //Start at day 1, go through each item every time sadly
                {

                   

                        System.Diagnostics.Debug.WriteLine("Day "+(i+1)+" Activity "+ ListOfDayPlans.ElementAt(i).Activities.ElementAt(j).ActivityName);
                        
                    
                }

                //DisplayAlert("Debug", "For the day: "+i+", there are " + ListOfDayPlans.ElementAt(i).Activities.Count() + " elements", "Cool");


            }
        }
        private async Task SetMapToUserLocation()
        {
           
            var position = await Plugin.Geolocator.CrossGeolocator.Current.GetPositionAsync();
            Position MyPosition = new Position(position.Latitude, position.Longitude);
            userPosition = MyPosition;
            TripMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                MyPosition, Distance.FromMiles(1.0)));
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
        void Pin_Clicked(object sender, EventArgs e)
        {
            var pin = (Pin)sender;

            DisplayAlert("Pin Clicked", $"{pin.Label} Clicked.", "Close");
        }
        public void directionsButtonClicked()
        {
            LaunchMapApp(_SelectedActivity);
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
            else if(Device.RuntimePlatform == Device.Android)
            {

                // pass the address to Android if we have it
                request = string.Format("geo:0,0?q={0}({1})", string.IsNullOrWhiteSpace(addr) ? loc : addr, name);

            }
            else if(Device.RuntimePlatform == Device.WPF)
            {
                request = string.Format("bingmaps:?cp={0}&q={1}", loc, name);
            }

           
            if(request!="")
            Device.OpenUri(new Uri(request));
        }
        private async Task SetCoordinatesForActivityAddress()
        {
            Double AddressLongitude = 0.0;
            Double AddressLatitude = 0.0;
            Geocoder gc = new Geocoder();
            if (givenActivities != null)
            {
                for (int i = 0; i < givenActivities.Activities.Count(); i++)
                {
                    if (givenActivities.Activities.ElementAt(i).ActivityAddressLatitude == -1 && givenActivities.Activities.ElementAt(i).ActivityAddressLongitude== -1)
                    {
                       
                        var theEnteredAdress = givenActivities.Activities.ElementAt(i).ActivityAddress;
                        IEnumerable<Position> result = await gc.GetPositionsForAddressAsync(theEnteredAdress);

                        foreach (Position pos in result)
                        {
                            // System.Diagnostics.Debug.WriteLine("Lat: {0}, Lng: {1}", pos.Latitude, pos.Longitude);
                            AddressLatitude = pos.Latitude;
                            AddressLongitude = pos.Longitude;
                        }
                        givenActivities.Activities.ElementAt(i).ActivityAddressLatitude = AddressLatitude;
                        givenActivities.Activities.ElementAt(i).ActivityAddressLongitude = AddressLongitude;

                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Activity aslready has coordinates");
                        
                    }
                }
                System.Diagnostics.Debug.WriteLine("List has been populated with coordinates");

            }

            


           

        }
        private async Task SetCoordinatesForActivityAddress(ActivityList listToSetCoordinates)
        {
            Double AddressLongitude = 0.0;
            Double AddressLatitude = 0.0;
            Geocoder gc = new Geocoder();
            if (listToSetCoordinates != null)
            {
                for (int i = 0; i < listToSetCoordinates.Activities.Count(); i++)
                {
                    if (listToSetCoordinates.Activities.ElementAt(i).ActivityAddressLatitude == -1 && listToSetCoordinates.Activities.ElementAt(i).ActivityAddressLongitude == -1)
                    {

                        var theEnteredAdress = listToSetCoordinates.Activities.ElementAt(i).ActivityAddress;
                        IEnumerable<Position> result = await gc.GetPositionsForAddressAsync(theEnteredAdress);

                        foreach (Position pos in result)
                        {
                            // System.Diagnostics.Debug.WriteLine("Lat: {0}, Lng: {1}", pos.Latitude, pos.Longitude);
                            AddressLatitude = pos.Latitude;
                            AddressLongitude = pos.Longitude;
                        }
                        listToSetCoordinates.Activities.ElementAt(i).ActivityAddressLatitude = AddressLatitude;
                        listToSetCoordinates.Activities.ElementAt(i).ActivityAddressLongitude = AddressLongitude;

                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Activity aslready has coordinates");

                    }
                }
                System.Diagnostics.Debug.WriteLine("List has been populated with coordinates");

            }
        }

            private async void GetCities()
        {
            //await DisplayAlert("Internet Availible", "Going forward with Method", "OK");
            //Check network status   
            if (NetworkCheck.IsInternet())
            {

                var client = new System.Net.Http.HttpClient();
                var response = await client.GetAsync("https://api.myjson.com/bins/b097a");
                string activitiesJson = await response.Content.ReadAsStringAsync();
                ActivityList ActivityObjList = new ActivityList();
                if (activitiesJson!= "")
                {
                    //Converting JSON Array Objects into generic list  
                    ActivityObjList = JsonConvert.DeserializeObject<ActivityList>(activitiesJson);
                    givenActivities = ActivityObjList;
                   // SetCurrentActivityAddress();
                }
                else
                {
                    await DisplayAlert("ERROR", "Unable to load trip details.", "Ok");
                }
                //Binding listview with server response    
                OrganizeListByDay(givenActivities);
                 await SetCoordinatesForActivityAddress(ListOfDayPlans.ElementAt(_CurrentDay - 1));
                listviewActivities.ItemsSource = ListOfDayPlans.ElementAt(_CurrentDay-1).Activities;
            }
            else
            {
                await DisplayAlert("ERROR", "No network is available.", "Ok");
            }
            //Hide loader after server response    
            await SetMapToUserLocation();
           
          
            ProgressLoader.IsVisible = false;
             
        }
        public void listviewActivity_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ResetSelectionsInList();
           
            //This method was created to reset the size of the other objects in the list
            _SelectedActivity = e.SelectedItem as CurrerntTripActivity;
            
           
            //Really stupid code that gets the index of the selected item, I am sure there has to be a better way
            int Id = Convert.ToInt32( givenActivities.Activities.IndexOf((e.SelectedItem as CurrerntTripActivity)).ToString()); //find the index of the item by comparison with original list.
            //DisplayAlert("DEBUG", "Item Index" + Id + ".", "Understood");
           
            
              
             (e.SelectedItem as CurrerntTripActivity).CurrentlySelectedEvent = true;
        

             //  _SelectedActivity.ActivityViewSize = 400;


            SetCurrentActivityAddress((e.SelectedItem as CurrerntTripActivity));
          
            
            
            
        }
        
        public void ResetSelectionsInList()
        {
            if (givenActivities != null)
            {
                for (int i = 0; i < givenActivities.Activities.Count(); i++)
                {
                    givenActivities.Activities.ElementAt(i).CurrentlySelectedEvent = false;
                }
                System.Diagnostics.Debug.WriteLine("List has been reset");

            }
            else
            {
                
            }
           
        }

        private void MoreDetails_Button_Pressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ActivityDetailsPage(userPosition, _SelectedActivity));
        }
    }


    public class CurrerntTripActivity : INotifyPropertyChanged
    {
        private int _size = 100;
        private int _SelectedViewSize = 300;
        private Color _BackgroundColorNotSelected = Color.Black;
        private Color _BackgroundColorSelected = Color.Accent;
        private Color _CurrentColor = Color.Black;
        private string _ActivityName = null;
        private double _ActivityLongitude = -1.0;
        private double _ActivityLatitude = -1.0; //Middle of nowhere 
        private bool _IsCurrentlySelectedEvent = false;
        public int DayOfPlan()
        {
            return Convert.ToInt32(ActivityDay.ToString());
        }
        public Color SelectedColor
        {
            get
            {
                return _CurrentColor;
            }
            set
            {
                _CurrentColor = value;
                OnPropertyChanged(nameof(SelectedColor));

            }
        }
        public bool CurrentlySelectedEvent
        {
            get
            {
                return _IsCurrentlySelectedEvent;
            }
            set
            {
                _IsCurrentlySelectedEvent = value;
                if (value == false)
                {
                    SelectedColor = _BackgroundColorNotSelected;
                    ActivityViewSize = 100;
                }
                else
                {
                    SelectedColor = _BackgroundColorSelected;
                    ActivityViewSize = _SelectedViewSize;
                }
                OnPropertyChanged(nameof(CurrentlySelectedEvent));

            }
        }
        //private string name = null;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int ActivityViewSize
        
           {
            get {
                return _size;
            }
            set
            {
               _size = value;
                OnPropertyChanged(nameof(ActivityViewSize));
            }

        }
        
        public string ActivityName {
            get
            {
                return _ActivityName;
            }
            set
            {
                _ActivityName = value;
                OnPropertyChanged(nameof(ActivityName));
            }
        }
        public double ActivityAddressLongitude  {
           get {
                return _ActivityLongitude;
            }
            set
            {
                _ActivityLongitude = value;
               // OnPropertyChanged(nameof(ActivityName));
            }
        }
        public double  ActivityAddressLatitude { 
            get{
                return _ActivityLatitude;
            }
            set
            {
                _ActivityLatitude = value;
                // OnPropertyChanged(nameof(ActivityName));
            }
        }

        public string ActivityPicture { get; set; }
        public string ActivityAddress { get; set; }
        public string ActivityMoreDetails { get; set; } = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam";
        public string AcvtivityAbout{ get; set; }
        public string ActivityTime { get; set; } 
        public string ActivityRecommendation { get; set; } = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam";
        public string ActivityType { get; set; }
        public string ActivityDay { get; set; }
        public string ActivityMonth { get; set; }
        public string ActivityTripName { get; set; }
        
    }
    public class ActivityList
    {
        public List<CurrerntTripActivity> Activities { get; set; }
        public CurrerntTripActivity getActivity(int y)
        {
            return Activities[y];
        }
        public int ListLength()
        {
            return Activities.Count();
        }
    }

}
