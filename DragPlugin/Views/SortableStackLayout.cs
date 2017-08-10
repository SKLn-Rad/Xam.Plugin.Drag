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
            IsDragging = true;
            FocusedView = view;

            _startingX = view.TranslationX;
            _startingY = view.TranslationY;
        }

        internal void NotifyDragEnd(View view)
        {
            var currentPosition = Children.IndexOf(view);
            int newPosition;

            // Calculate and remove
            newPosition = Orientation == StackOrientation.Horizontal ? GetNewPositionHorizontally(view) : GetNewPositionVertically(view);
            Children.RemoveAt(currentPosition);

            // Restore translation
            view.TranslationX = 0;
            view.TranslationY = 0;

            // Insert and finalize
            Children.Insert(newPosition, view);
            IsDragging = false;
            FocusedView = null;
        }

        internal void NotifyDrag(double newXTranslation, double newYTranslation)
        {
            if (!IsDragging) return;

            FocusedView.TranslationX = newXTranslation;
            FocusedView.TranslationY = newYTranslation;
        }

        private int GetNewPositionHorizontally(VisualElement view)
        {
            var index = 0;
            var position = view.X + view.TranslationX;
            foreach (var child in Children)
            {
                var viewPos = child.X + child.TranslationX;
                if (position <= viewPos)
                    return index;

                index++;
            }

            return index;
        }

        private int GetNewPositionVertically(VisualElement view)
        {
            var index = 0;
            var position = view.Y + view.TranslationY;
            foreach (var child in Children)
            {
                var viewPos = child.Y + child.TranslationY;
                if (position <= viewPos)
                    return index;

                index++;
            }

            return index;
        }
    }
}
