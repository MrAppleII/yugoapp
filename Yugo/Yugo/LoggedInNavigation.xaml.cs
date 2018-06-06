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
        private void Jp_buttonclicked(object sender, EventArgs e)
        {
            //TODO goto the next string
            //(sender as Button).Text = "I was just clicked!";

            Navigation.PushAsync(new GuideSelectScreen());


        }
    }

}