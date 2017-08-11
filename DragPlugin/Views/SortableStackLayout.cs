using System.Diagnostics;
using Xamarin.Forms;

namespace DragPlugin.Views
{
    public class SortableStackLayout : StackLayout
    {

        public bool IsDragging { get; set; }
        public View FocusedView { get; set; }

        private double _startingX;
        private double _startingY;

        internal void NotifyDragStart(View view)
        {
            if (view == null) return;

            IsDragging = true;
            FocusedView = view;

            RaiseChild(FocusedView);

            _startingX = view.TranslationX;
            _startingY = view.TranslationY;
        }

        internal void NotifyDragEnd()
        {
            if (FocusedView == null) return;

            var currentPosition = Children.IndexOf(FocusedView);
            int newPosition = Orientation == StackOrientation.Horizontal ? GetNewPositionHorizontally(FocusedView) : GetNewPositionVertically(FocusedView);

            if (newPosition != currentPosition)
            {
                Children.RemoveAt(currentPosition);
                Children.Insert(newPosition, FocusedView);
            }
            
            FocusedView.TranslationX = 0;
            FocusedView.TranslationY = 0;

            IsDragging = false;
            FocusedView = null;
        }

        internal void NotifyDrag(double newXTranslation, double newYTranslation)
        {
            if (!IsDragging || FocusedView == null) return;

            FocusedView.TranslationX = newXTranslation;
            FocusedView.TranslationY = newYTranslation;
        }

        private int GetNewPositionHorizontally(View view)
        {
            var index = 0;
            var position = view.X + view.TranslationX;
            foreach (var child in Children)
            {
                var viewPos = child.X + child.TranslationX;
                if (position < viewPos)
                    return index;

                index++;
            }

            return index;
        }

        private int GetNewPositionVertically(View view)
        {
            var index = 0;
            var position = view.Y + view.TranslationY;
            foreach (var child in Children)
            {
                var viewPos = child.Y + child.TranslationY;
                if (position < viewPos)
                    return index;

                index++;
            }

            return index;
        }
    }
}
