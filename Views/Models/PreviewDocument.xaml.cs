using System.Windows.Media.Imaging;

namespace AlphaPersonel.Views.Models;

public partial class PreviewDocument : Window
{
    public PreviewDocument(string url)
    {
        InitializeComponent();
        UrlImage.Source = new BitmapImage(new Uri(url));
    }
    /*private void SV_ScrollChanged(object sender, ScrollChangedEventArgs e)  
    {  
        HRect.Width = SV.ViewportWidth / Zoom.Value;  
        HRect.Height = SV.ViewportHeight / Zoom.Value;  
        HRect.SetValue(Canvas.LeftProperty, SV.ContentHorizontalOffset / Zoom.Value);  
        HRect.SetValue(Canvas.TopProperty, SV.ContentVerticalOffset / Zoom.Value);  
    }  
 
    private void Image_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed) return;
        var p = e.GetPosition(Canv);  
        SV.ScrollToHorizontalOffset((p.X * Zoom.Value) - HRect.Width / 2);  
        SV.ScrollToVerticalOffset((p.Y * Zoom.Value) - HRect.Height / 2);
    }  
    */
}