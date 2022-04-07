using System.Windows.Controls;

namespace AlphaPersonel.Services;
internal class BoundPasswordBox
{
    private static bool _updating;


    public static readonly DependencyProperty BoundPasswordProperty =
        DependencyProperty.RegisterAttached("BoundPassword",
            typeof(string),
            typeof(BoundPasswordBox),
            new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));


    public static string GetBoundPassword(DependencyObject d)
    {
        return (string)d.GetValue(BoundPasswordProperty);
    }


    public static void SetBoundPassword(DependencyObject d, string value)
    {
        d.SetValue(BoundPasswordProperty, value);
    }

    private static void OnBoundPasswordChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        PasswordBox? password = d as PasswordBox;
        if (password != null)
        {
            // Disconnect the handler while we're updating.
            password.PasswordChanged -= PasswordChanged;
        }

        if (e.NewValue != null)
        {
            if (!_updating)
            {
                if (password != null)
                {
                    password.Password = e.NewValue.ToString() ?? string.Empty;
                }
            }
        }
        else
        {
            if (password != null)
            {
                password.Password = string.Empty;
            }
        }
        // Now, reconnect the handler.
        if (password != null)
        {
            password.PasswordChanged += PasswordChanged;
        }
    }

    static void PasswordChanged(object sender, RoutedEventArgs e)
    {
        _updating = true;
        if (sender is PasswordBox password)
        {
            SetBoundPassword(password, password.Password);
        }

        _updating = false;
    }
}
