using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ServerTools.Extensions
{
    public static class ListBoxExtensions
    {
        public static void MoveSelectedItemUp(this ListBox listBox)
        {
            MoveSelectedItem(listBox, -1);
        }

        public static void MoveSelectedItemDown(this ListBox listBox)
        {
            MoveSelectedItem(listBox, 1);
        }

        private static void MoveSelectedItem(ListBox listBox, int direction)
        {

            if (listBox.SelectedItem == null || listBox.SelectedIndex < 0)
                return;

            var newIndex = listBox.SelectedIndex + direction;

            if (newIndex < 0 || newIndex >= listBox.Items.Count)
                return;

            var selected = listBox.SelectedItem;

            var checkedListBox = listBox as CheckedListBox;
            var checkState = CheckState.Unchecked;
            
            if (checkedListBox != null)
                checkState = checkedListBox.GetItemCheckState(checkedListBox.SelectedIndex);

            listBox.Items.Remove(selected);
            listBox.Items.Insert(newIndex, selected);
            listBox.SetSelected(newIndex, true);

            checkedListBox?.SetItemCheckState(newIndex, checkState);
        }

        public static List<string> ToList(this ListBox listBox)
        {
            return listBox.Items.Count == 0 
                ? new List<string>() 
                : (from object item in listBox.Items select item.ToString()).ToList();
        }

        public static void BindDataSource(this ListBox listBox, IEnumerable<object> dataSource)
        {
            if (!dataSource.Any())
            {
                listBox.DataSource = null;
                return;
            }
            listBox.Items.Clear();
            foreach (var item in  dataSource.ToList())
            {
                listBox.Items.Add(item);
            }
        }

        public static void Insert(this ListBox listBox, object item)
        {
            listBox.Items.Insert(listBox.Items.Count, item);
        }

        public static string NextItem(this ListBox listBox, string current)
        {
            var currentIndex = listBox.Items.IndexOf(current);
            var nextIndex = currentIndex + 1;

            if (nextIndex > listBox.Items.Count)
            {
                nextIndex = 0;
            }

            return listBox.Items[nextIndex].ToString();
        }
    }
}