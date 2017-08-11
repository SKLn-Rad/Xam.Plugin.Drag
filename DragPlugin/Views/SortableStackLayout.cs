using System;
using System.Collections.Generic;
using System.Linq;
using DragPlugin.Model;
using Xamarin.Forms;

namespace DragPlugin.Views
{
    public class SortableStackLayout : Layout<View>
    {

        private readonly Dictionary<View, SizeRequest> _layoutCache = new Dictionary<View, SizeRequest>();
        private LayoutInformation _layoutInformation = new LayoutInformation();

        public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation),
            typeof(SortableStackLayout), StackOrientation.Vertical, propertyChanged: (bindable, oldValue, newValue) => ((SortableStackLayout)bindable).InvalidateLayout());

        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(SortableStackLayout),
            6d, propertyChanged: (bindable, oldValue, newValue) => ((SortableStackLayout)bindable).InvalidateLayout());

        public StackOrientation Orientation
        {
            get => (StackOrientation) GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public double Spacing
        {
            get => (double)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        public SortableStackLayout()
        {
            VerticalOptions = HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (!HasVisibleChildren())
                return;

            var index = -1;
            foreach (var child in Children)
            {
                index++;
                if (child.IsVisible)
                    LayoutChildIntoBoundingRegion(child, _layoutInformation.Plots[index]);
            }
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (!HasVisibleChildren())
                return new SizeRequest();

            CalculateLayout(_layoutInformation, Padding.Left, Padding.Top, widthConstraint, heightConstraint);
            return new SizeRequest(_layoutInformation.Bounds.Size, _layoutInformation.MinimumSize);
        }

        private void CalculateLayout(LayoutInformation layout, double x, double y, double widthConstraint, double heightConstraint)
        {
            layout.Plots = new Rectangle[Children.Count];
            layout.Requests = new SizeRequest[Children.Count];

            if (Orientation == StackOrientation.Vertical)
                CalculateNaiveLayoutVertically(layout, x, y, widthConstraint);
            else
                CalculateNaiveLayoutHorizontally(layout, x, y, heightConstraint);
        }

        private void CalculateNaiveLayoutHorizontally(LayoutInformation layout, double x, double y, double heightConstraint)
        {
            var yOffset = y;
            var xOffset = x;

            var boundsWidth = 0d;
            var boundsHeight = 0d;
            var minimumWidth = 0d;
            var minimumHeight = 0d;

            var index = -1;
            foreach (var child in Children)
            {
                index++;
                if (!child.IsVisible)
                    continue;

                var request = child.Measure(double.PositiveInfinity, heightConstraint, MeasureFlags.IncludeMargins);
                var actualHeight = child.VerticalOptions.Expands ? heightConstraint : request.Request.Height;

                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (child.VerticalOptions.Alignment == LayoutAlignment.End)
                    yOffset = heightConstraint - request.Request.Height;

                else if (child.VerticalOptions.Alignment == LayoutAlignment.Center)
                    yOffset = heightConstraint / 2 - request.Request.Height / 2;

                var bounds = new Rectangle(xOffset, yOffset, request.Request.Width, actualHeight);

                layout.Plots[index] = bounds;
                layout.Requests[index] = request;
                xOffset = bounds.Right + Spacing;

                boundsWidth = bounds.Right + y;
                boundsHeight = Math.Max(boundsHeight, actualHeight);
                minimumHeight = Math.Max(boundsHeight, boundsHeight);
                minimumWidth += request.Minimum.Width + Spacing;
            }

            // Remove last spacing
            minimumWidth -= Spacing;

            layout.Bounds = new Rectangle(x, y, boundsWidth, boundsHeight);
            layout.MinimumSize = new Size(minimumWidth, minimumHeight);
        }

        private void CalculateNaiveLayoutVertically(LayoutInformation layout, double x, double y, double widthConstraint)
        {
            var yOffset = y;
            var xOffset = x;

            var boundsWidth = 0d;
            var boundsHeight = 0d;
            var minimumWidth = 0d;
            var minimumHeight = 0d;

            var index = -1;
            foreach (var child in Children)
            {
                index++;
                if (!child.IsVisible)
                    continue;

                var request = child.Measure(widthConstraint, double.PositiveInfinity, MeasureFlags.IncludeMargins);
                var actualWidth = child.HorizontalOptions.Expands ? widthConstraint : request.Request.Width;

                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (child.HorizontalOptions.Alignment == LayoutAlignment.End)
                    xOffset = widthConstraint - request.Request.Width;

                else if (child.HorizontalOptions.Alignment == LayoutAlignment.Center)
                    xOffset = widthConstraint / 2 - request.Request.Width / 2;

                var bounds = new Rectangle(xOffset, yOffset, actualWidth, request.Request.Height);

                layout.Plots[index] = bounds;
                layout.Requests[index] = request;
                yOffset = bounds.Bottom + Spacing;

                boundsWidth = Math.Max(boundsWidth, actualWidth);
                boundsHeight = bounds.Bottom - y;
                minimumHeight += request.Minimum.Height + Spacing;
                minimumWidth = Math.Max(minimumWidth, actualWidth);
            }

            // Remove last spacing
            minimumHeight -= Spacing;

            layout.Bounds = new Rectangle(x, y, boundsWidth, boundsHeight);
            layout.MinimumSize = new Size(minimumWidth, minimumHeight);
        }

        protected override void OnChildMeasureInvalidated()
        {
            InvalidateCache();
            base.OnChildMeasureInvalidated();
        }

        private void InvalidateCache()
        {
            _layoutCache.Clear();
            _layoutInformation = new LayoutInformation();
        }

        private bool HasVisibleChildren() => Children.Any(d => d.IsVisible);
    }
}