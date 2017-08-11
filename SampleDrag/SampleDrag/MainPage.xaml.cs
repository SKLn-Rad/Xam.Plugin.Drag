using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SampleDrag
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            CreateChildren();
        }

        private void CreateChildren()
        {
            Random rand = new Random();
            for (int i = 0; i != 50; i++)
            {
                DragZone.Children.Add(new BoxView()
                {
                    WidthRequest = 75,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromRgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)).WithLuminosity(.8).WithSaturation(.95)
                });
            }
        }
    }
}
