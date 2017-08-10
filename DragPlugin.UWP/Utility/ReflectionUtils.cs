using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

namespace DragPlugin.UWP.Utility
{
    public static class ReflectionUtils
    {

        public static FrameworkElement GetFrameworkElementFromView(View view)
        {
            var renderer = view.GetOrCreateRenderer();
            var properties = renderer.GetType().GetRuntimeProperties().Where(p => p.Name.Equals("Control"));

            var propertyInfos = properties as PropertyInfo[] ?? properties.ToArray();
            if (propertyInfos.Any())
                return (FrameworkElement) propertyInfos.ElementAt(0).GetValue(renderer);

            return default(FrameworkElement);
        }

    }
}