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

        void CreateChildren()
        {
            Random rand = new Random(DateTime.Now.Second);

            for (int i = 0; i != 50; i++)
            {
                int r = rand.Next(0, 255);
                int g = rand.Next(0, 255);
                int b = rand.Next(0, 255);

                DragZone.Children.Add(new BoxView()
                {
                    HeightRequest = 50,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromRgb(r, g, b).WithLuminosity(.8).WithSaturation(.8)
                });
            }
        }
    }
}
