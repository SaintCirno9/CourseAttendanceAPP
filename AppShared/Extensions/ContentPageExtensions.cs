using Xamarin.Forms;

namespace AppShared.Extensions
{
    public static class ContentPageExtensions
    {
        public static void AddBusyDisplay(this ContentPage page)
        {
            var content = page.Content;

            var grid = new Grid(); //不加Row、Column实现覆盖
            grid.Children.Add(content);
            var backGround = new Grid {BackgroundColor = Color.CornflowerBlue, Opacity = 0.2};
            backGround.SetBinding(VisualElement.IsVisibleProperty, "IsBusy");
            var busyGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                }
            };
            busyGrid.SetBinding(VisualElement.IsVisibleProperty, "IsBusy");

            busyGrid.Children.Add(new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.Center,
                IsRunning = true,
                Color = Color.CornflowerBlue
            }, 0, 1); //居中在Grid内（类似padding、margin用法）
            grid.Children.Add(backGround);
            grid.Children.Add(busyGrid);
            page.Content = grid;
        }
    }
}