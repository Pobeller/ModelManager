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
        public static void InitializeListViewSorting(ListView listView)
        {
            listView.ColumnClick += new ColumnClickEventHandler(ListView_ColumnClick);
        }

        private static void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView == null) return;

            if (listView.ListViewItemSorter is ListViewItemComparer sorter && sorter.Column == e.Column)
            {
                sorter.Order = sorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                listView.ListViewItemSorter = new ListViewItemComparer(e.Column, SortOrder.Ascending);
            }

            listView.Sort();
        }

        // Sortierklasse für ListView (nicht generisch)
        class ListViewItemComparer : IComparer
        {
            public int Column { get; }
            public SortOrder Order { get; set; }

            public ListViewItemComparer(int column, SortOrder order)
            {
                Column = column;
                Order = order;
            }

            public int Compare(object x, object y)
            {
                var itemX = x as ListViewItem;
                var itemY = y as ListViewItem;

                if (itemX == null || itemY == null)
                    return 0;

                string textX = itemX.SubItems[Column].Text;
                string textY = itemY.SubItems[Column].Text;

                int result;
                if (Column == 1) // Assuming the size column is at index 1
                {
                    if (TryParseFileSize(textX, out long sizeX) && TryParseFileSize(textY, out long sizeY))
                    {
                        result = sizeX.CompareTo(sizeY);
                    }
                    else
                    {
                        result = string.Compare(textX, textY, StringComparison.OrdinalIgnoreCase);
                    }
                }
                else
                {
                    result = string.Compare(textX, textY, StringComparison.OrdinalIgnoreCase);
                }

                return Order == SortOrder.Ascending ? result : -result;
            }

            private static bool TryParseFileSize(string sizeText, out long size)
            {
                size = 0;
                if (string.IsNullOrWhiteSpace(sizeText)) return false;

                sizeText = sizeText.ToUpper().Replace(" ", "").Replace(",", ".");

                if (sizeText.EndsWith("GB") && double.TryParse(sizeText.Replace("GB", ""), out double gb))
                {
                    size = (long)(gb * 1024 * 1024 * 1024);
                    return true;
                }

                if (sizeText.EndsWith("MB") && double.TryParse(sizeText.Replace("MB", ""), out double mb))
                {
                    size = (long)(mb * 1024 * 1024);
                    return true;
                }

                return false;
            }
        }
    }









    // Sortierklasse für ListView (nicht generisch)
    class ListViewItemComparer : IComparer
    {
        public int Column { get; }
        public SortOrder Order { get; set; }

        public ListViewItemComparer(int column, SortOrder order)
        {
            Column = column;
            Order = order;
        }

        public int Compare(object x, object y)
        {
            var itemX = x as ListViewItem;
            var itemY = y as ListViewItem;

            if (itemX == null || itemY == null)
                return 0;

            string textX = itemX.SubItems[Column].Text;
            string textY = itemY.SubItems[Column].Text;

            int result;
            if (Column == 1) // Assuming the size column is at index 1
            {
                if (TryParseFileSize(textX, out long sizeX) && TryParseFileSize(textY, out long sizeY))
                {
                    result = sizeX.CompareTo(sizeY);
                }
                else
                {
                    result = string.Compare(textX, textY, StringComparison.OrdinalIgnoreCase);
                }
            }
            else
            {
                result = string.Compare(textX, textY, StringComparison.OrdinalIgnoreCase);
            }

            return Order == SortOrder.Ascending ? result : -result;
        }

        private static bool TryParseFileSize(string sizeText, out long size)
        {
            size = 0;
            if (string.IsNullOrWhiteSpace(sizeText)) return false;

            sizeText = sizeText.ToUpper().Replace(" ", "").Replace(",", ".");

            if (sizeText.EndsWith("GB") && double.TryParse(sizeText.Replace("GB", ""), out double gb))
            {
                size = (long)(gb * 1024 * 1024 * 1024);
                return true;
            }

            if (sizeText.EndsWith("MB") && double.TryParse(sizeText.Replace("MB", ""), out double mb))
            {
                size = (long)(mb * 1024 * 1024);
                return true;
            }

            return false;
        }
    }










}
