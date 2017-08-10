using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using DragPlugin.UWP.Effects;
using DragPlugin.UWP.Utility;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Input;

[assembly: ResolutionGroupName("DragPlugin")]
[assembly: ExportEffect(typeof(DragProviderEffect), "DragProvider")]
namespace DragPlugin.UWP.Effects
{
    public class DragProviderEffect
    {
        private Dictionary<FrameworkElement, View> _supportedChildren;
        private Layout<View> _currentLayout;

        private View _currentChild;
        private int _currentPointer = -1;
        private double _xOrigin;
        private double _yOrigin;

        /*
        private void Setup()
        {
            _currentLayout = (Layout<View>) Element;

            foreach (var child in _currentLayout.Children)
                RegisterChild(child);
        }

        private void RegisterChild(View child)
        {
            var control = ReflectionUtils.GetFrameworkElementFromView(child);
            if (DragUtils.IsChildSupported(child, control))
                SetupChild(child, control);
        }

        private void SetupChild(View child, FrameworkElement control)
        {
            _supportedChildren.Add(control, child);

            control.IsHoldingEnabled = true;
            control.PointerPressed += OnPointerPressed;
            control.PointerReleased += OnPointerReleased;
            control.PointerMoved += OnPointerMoved;
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!IsCurrentlyInDragEvent())
                return;

            var point = e.GetCurrentPoint(Container);
            _currentChild.TranslationX = (point.Position.X - _xOrigin);
            _currentChild.TranslationY = (point.Position.Y - _yOrigin);
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (((int) e.Pointer.PointerId).Equals(_currentPointer))
            {
                // Use offset to swap elements around

                // Release!
                _currentPointer = -1;
            }
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (IsCurrentlyInDragEvent())
                return;

            // Get the current pointer
            var point = e.GetCurrentPoint(Container);
            _currentPointer = (int) e.Pointer.PointerId;
            _currentChild = _supportedChildren[sender as FrameworkElement];

            // Account for current translation
            _xOrigin = (point.Position.X - _currentChild.TranslationX);
            _yOrigin = (point.Position.Y - _currentChild.TranslationY);
        }

        private bool IsCurrentlyInDragEvent() => _currentPointer != -1;
        */
    }
}