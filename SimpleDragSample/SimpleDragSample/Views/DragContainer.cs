using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimpleDragSample.Views
{
    public class DragContainer : ContentView
    {

        public DragContainer()
        {
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(ContentPage.Content)))
                SetupContentPage();
        }

        private void SetupContentPage()
        {
            var layout = Content as Layout<View> ?? throw new Exception("Content in DragContainer doesn't contain children");
        }
    }
}
