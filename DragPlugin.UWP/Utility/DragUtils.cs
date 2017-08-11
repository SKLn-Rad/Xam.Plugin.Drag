using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms;

namespace DragPlugin.UWP.Utility
{
    public static class DragUtils
    {

        private static readonly Type[] SupportedViewTypes =
        {
            typeof(BoxView)
        };

        private static readonly Type[] SupportedFrameworkElementTypes =
        {
            typeof(Windows.UI.Xaml.Shapes.Rectangle)
        };

        public static bool IsChildSupported(View view, FrameworkElement control)
        {
            return true;
            /*
            if (control == null) return false;
            return SupportedViewTypes.Contains(view.GetType()) &&
                   SupportedFrameworkElementTypes.Contains(control.GetType());
                   */
        }

    }
}