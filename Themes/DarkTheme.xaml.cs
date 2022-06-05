namespace AlphaPersonel.Themes
{
    public partial class DarkTheme
    {
        private void CloseWindow_Event(object sender, RoutedEventArgs e)
        {
            if (e.Source == null) return;
            CloseWind(Window.GetWindow((FrameworkElement)e.Source) ?? throw new InvalidOperationException());
        }
        private void AutoMinimize_Event(object sender, RoutedEventArgs e)
        {
            if (e.Source == null) return;
            MaximizeRestore(Window.GetWindow((FrameworkElement)e.Source) ?? throw new InvalidOperationException());
        }
        private void Minimize_Event(object sender, RoutedEventArgs e)
        {
            if (e.Source == null) return;
             MinimizeWind(Window.GetWindow((FrameworkElement)e.Source) ?? throw new InvalidOperationException()); 
        }

        public static void CloseWind(Window window) => window.Close();
        public static void MaximizeRestore(Window window)
        {
            window.WindowState = window.WindowState switch
            {
                WindowState.Maximized => WindowState.Normal,
                WindowState.Normal => WindowState.Maximized,
                _ => window.WindowState
            };
        }
        public static void MinimizeWind(Window window) => window.WindowState = WindowState.Minimized;
    }
}
