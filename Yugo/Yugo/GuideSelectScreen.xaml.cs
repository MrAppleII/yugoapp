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
	public partial class GuideSelectScreen : ContentPage
	{
        private Destination currentCity;
        
		public GuideSelectScreen (Destination selectedCity)
		{
            currentCity = selectedCity;
            InitializeComponent ();
           
		}
        private async void GetJSON()
        {
            //await DisplayAlert("Internet Availible", "Going forward with Method", "OK");
            //Check network status   
            if (NetworkCheck.IsInternet())
            {
               
                var client = new System.Net.Http.HttpClient();
                String guideLink = currentCity.ListLink;
                var response = await client.GetAsync(guideLink);
                string contactsJson = await response.Content.ReadAsStringAsync();
                ContactList ObjContactList = new ContactList();
                if (contactsJson != "")
                {
                    //Converting JSON Array Objects into generic list  
                    ObjContactList = JsonConvert.DeserializeObject<ContactList>(contactsJson);
                }
                //Binding listview with server response    
                listviewConacts.ItemsSource = ObjContactList.contacts;
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
          
            var itemSelectedData = e.SelectedItem as Contact;
            Navigation.PushAsync(new GuideDetails(itemSelectedData));
            //do nothing so far
        }
        protected override void OnAppearing()
        {
            GetJSON();

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
                // write your code if there is no Internet available      
                return false;
            }
        }
    }
    public class Phone
    {
        public string mobile { get; set; }
        public string home { get; set; }
        public string office { get; set; }
    }

    public class Contact
    {
        public string id { get; set; }
        public string PicUrl { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string rating { get; set; }
        public string languages { get; set; }
        public string about { get; set; }
        public string gender { get; set; }
        public Phone phone { get; set; }
    }

    public class ContactList
    {
        public List<Contact> contacts { get; set; }
    }
}