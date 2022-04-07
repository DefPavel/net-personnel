using AlphaPersonel.Theme.Extensions;
namespace AlphaPersonel;
public partial class MainWindow 
{
    public MainWindow()
    {
        InitializeComponent();
    }
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        if (TryFindResource("AccentColorBrush") is System.Windows.Media.SolidColorBrush accentBrush)
        {
            accentBrush.Color.CreateAccentColors();
        }
    }
}

