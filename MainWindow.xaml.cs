using AlphaPersonel.Theme.Extensions;
using System.Net.Http;

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

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        using ServiceClient? client = new("http://localhost:8080");
        string encryptedPass = CustomAes256.Encrypt("root", "8UHjPgXZzXDgkhqV2QCnooyJyxUzfJrO");
        Users user = new()
        {
            UserName = "1497",
            Password = encryptedPass,
            IdModules = ModulesProject.Personel
        };
        try
        {
            var userResponse = await client.PostAsync<Users>("api/auth", user);
            MessageBox.Show(userResponse.Token);
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show(ex.Message);
            
        }
        


        
    }
}

