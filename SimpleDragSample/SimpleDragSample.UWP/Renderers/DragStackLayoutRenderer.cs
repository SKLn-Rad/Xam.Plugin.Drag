using System;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using SimpleDragSample.UWP.Renderers;
using SimpleDragSample.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(DragStackLayout), typeof(DragStackLayoutRenderer))]
namespace SimpleDragSample.UWP.Renderers
{
    public class DragStackLayoutRenderer : ViewRenderer<StackLayout, FrameworkElement>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            if (e.NewElement != null)
                SetupElement(e.NewElement);

            base.OnElementChanged(e);
        }

        private void SetupElement(StackLayout element)
        {
            ApplyChildrenTransitions();
            SetupDrop();
        }

        private void SetupDrop()
        {
            AllowDrop = true;
            //DragOver += OnDragOver;

            foreach (var child in Children)
            {
                child.CanDrag = true;
                child.AllowDrop = true;
                child.DragOver += OnChildDragOver;
                child.DragStarting += OnChildDragStarting;
                child.DropCompleted += OnChildDropCompleted;
            }
        }

        private void OnChildDragOver(object sender, DragEventArgs e)
        {
            // Notify index of the child
            var view = sender as FrameworkElement;
            var index = Children.IndexOf(sender as FrameworkElement);

            if (view == null) return;

            var position = e.GetPosition(this);
            var height = view.ActualHeight;
            var visual = view.TransformToVisual(this);
            var point = visual.TransformPoint(new Windows.Foundation.Point(0.5, 0.5));

            if (Element.Orientation == StackOrientation.Horizontal)
            {

            }
            else
            {
                if (position.Y > point.Y)
                    ((DragStackLayout)Element).NotifyHoverPosition(index);
            }
        }

        private void OnChildDropCompleted(UIElement sender, DropCompletedEventArgs args)
        {
            var layout = Element as DragStackLayout;

            if (layout != null && layout.IsCurrentlyDragging)
                layout.NotifyDragStop();
        }

        private void OnChildDragStarting(UIElement sender, DragStartingEventArgs args)
        {
            var layout = Element as DragStackLayout;

            var index = Children.IndexOf(sender);
            var view = Element.Children[index];

            layout?.NotifyDragStart(view);
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;
            var position = e.GetPosition(this);

            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                var visual = child.TransformToVisual(this);
                var fe = child as FrameworkElement;
                Windows.Foundation.Point outPoint;

                if (!visual.TryTransform(new Windows.Foundation.Point(0, 0), out outPoint))
                    return;

                if (fe == default(FrameworkElement))
                    return;

                // Check if position is less than visual position; then notify insert at position
                if (Element.Orientation == StackOrientation.Horizontal)
                {
                    // Compare X
                }
                else
                {
                    // Compare Y
                    if (!(outPoint.Y <= position.Y)) continue;
                    ((DragStackLayout)Element).NotifyHoverPosition(i);
                    return;
                }
            }
        }

        private void ApplyChildrenTransitions()
        {
            ChildrenTransitions = new TransitionCollection
            {
                new EdgeUIThemeTransition(),
                new EntranceThemeTransition()
            };
        }
    }
}