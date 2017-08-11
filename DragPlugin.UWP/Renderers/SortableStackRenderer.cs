using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using DragPlugin.UWP.Renderers;
using DragPlugin.UWP.Utility;
using DragPlugin.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Input;

[assembly: ExportRenderer(typeof(SortableStackLayout), typeof(SortableStackRenderer))]
namespace DragPlugin.UWP.Renderers
{
    public class SortableStackRenderer : ViewRenderer<StackLayout, FrameworkElement>
    {
        private readonly Dictionary<FrameworkElement, View> _supportedChildren = new Dictionary<FrameworkElement, View>();
        private uint _currentPointer = uint.MaxValue;

        private double _initialX = -1;
        private double _initialY = -1;

        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            if (e.NewElement != null)
                SetupElement(e.NewElement);

            base.OnElementChanged(e);
        }

        private void SetupElement(StackLayout layout)
        {
            ContainerElement.PointerExited += OnPointerExited;
            layout.ChildAdded += OnChildAdded;
            layout.ChildRemoved += OnChildRemoved;

            // Setup children
            foreach (var child in layout.Children)
                SetupChild(child);
        }

        private void TeardownElement(StackLayout layout)
        {
            ContainerElement.PointerExited -= OnPointerExited;
            layout.ChildAdded -= OnChildAdded;
            layout.ChildRemoved -= OnChildRemoved;

            // Remove children
            foreach (var child in layout.Children)
                RemoveChild(child);
        }

        private void OnChildAdded(object sender, ElementEventArgs e) => SetupChild(e.Element as View);
        private void OnChildRemoved(object sender, ElementEventArgs e) => RemoveChild(e.Element as View);

        private void SetupChild(View view)
        {
            if (view == null) return;

            var control = ReflectionUtils.GetFrameworkElementFromView(view);
            if (!DragUtils.IsChildSupported(view, control)) return;

            _supportedChildren.Add(control, view);

            control.IsHoldingEnabled = true;
            control.PointerPressed += OnPointerPressed;
            control.PointerReleased += OnPointerReleased;
            control.PointerMoved += OnPointerMoved;
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            OnPointerReleased(sender, e);
        }

        private void RemoveChild(View view)
        {
            if (_supportedChildren.ContainsValue(view))
                _supportedChildren.Remove(ReflectionUtils.GetFrameworkElementFromView(view));
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerId != _currentPointer) return;

            var points = e.GetCurrentPoint(Control);
            ((SortableStackLayout)Element).NotifyDrag(points.Position.X - _initialX, points.Position.Y - _initialY);
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerId == _currentPointer)
                _currentPointer = uint.MaxValue;
            
            ((SortableStackLayout)Element).NotifyDragEnd();
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (PointerRegistered)
                return;
            
            var view = _supportedChildren[sender as FrameworkElement];
            var points = e.GetCurrentPoint(Control);

            _currentPointer = e.Pointer.PointerId;
            _initialX = points.Position.X - view.TranslationX;
            _initialY = points.Position.Y - view.TranslationY;

            ((SortableStackLayout)Element).NotifyDragStart(view);
        }

        private bool PointerRegistered => _currentPointer != uint.MaxValue;
    }
}