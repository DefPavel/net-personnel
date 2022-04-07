using System.Windows.Controls;
using System.Windows.Media;

namespace AlphaPersonel.Theme.Extensions;
public static class TreeViewItemExtension
{
    public static int GetDepth(this TreeViewItem item)
    {
        TreeViewItem? parent = GetParent(item);
        while (GetParent(item) != null)
        {
            return GetDepth(item: parent!)
                + 1;
        }
        return 0;
    }

    public static TreeViewItem? GetParent(this TreeViewItem item)
    {
        DependencyObject? parent = VisualTreeHelper.GetParent(item);
        while (parent is not (TreeViewItem or TreeView))
        {
            if (parent == null)
            {
                return null;
            }

            parent = VisualTreeHelper.GetParent(parent);
        }

        return parent as TreeViewItem;
    }
}

