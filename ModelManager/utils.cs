using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelManager
{
    internal class Utils
    {
    }









    // Sortierklasse für ListView (nicht generisch)
    class ListViewItemComparer : IComparer
    {
        private int col;

        public ListViewItemComparer(int column)
        {
            col = column;
        }

        public int Compare(object? x, object? y)
        {
            // Überprüfe, ob die Objekte ListViewItems sind
            var itemX = x as ListViewItem;
            var itemY = y as ListViewItem;

            if (itemX == null || itemY == null)
                return 0; // Fallback, falls die Objekte keine ListViewItems sind

            // Vergleiche die SubItem-Texte nach der gewünschten Spalte
            return string.Compare(
                itemX.SubItems[col].Text,
                itemY.SubItems[col].Text,
                StringComparison.OrdinalIgnoreCase
            );
        }
    }










}
