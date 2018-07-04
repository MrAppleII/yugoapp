using FFImageLoading.Forms.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Yugo
{
	public partial class App : Application
	{
		public App ()
		{
           
            InitializeComponent();

                  MainPage = new NavigationPage(new Yugo.intro_page());


            //   MainPage = new NavigationPage(new Yugo.GuideSelectScreen());
            // MainPage = new NavigationPage(new Yugo.LoggedInNavigation());

     //       MainPage = new NavigationPage(new Yugo.TripPlayMasterPage()); 
    }
        

		protected override void OnStart ()
		{

			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
