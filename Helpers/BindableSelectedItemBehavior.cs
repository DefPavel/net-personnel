using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;

namespace AlphaPersonel.Helpers;
internal class BindableSelectedItemBehavior : Behavior<TreeView>
{
    #region Переменные
    //Выбранный элемент
    public Departments SelectedItem
    {
        get => (Departments)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register("SelectedItem", typeof(Departments), typeof(BindableSelectedItemBehavior), new UIPropertyMetadata(null, OnSelectedItemChanged));

    private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        TreeViewItem? item = e.NewValue as TreeViewItem;
        item?.SetValue(TreeViewItem.IsSelectedProperty, true);
    }
    #endregion

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject != null)
        {
            AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
        }
    }

    private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        // При выборе элемента назвначем
        SelectedItem = (Departments)e.NewValue;
    }
}

