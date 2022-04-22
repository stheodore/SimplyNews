using System;
using System.Collections.Generic;
using UIKit;

namespace SimplyNews.iOS
{
    public class CategoryPickerModel : UIPickerViewModel
    {
        public List<string> Items { get; set; }
        protected int selectedIndex = 0;
        public event EventHandler<EventArgs> ValueChanged;

        public CategoryPickerModel()
        {
            Items = new List<string>();
        }

        public string SelectedItem
        {
            get { return (Items.Count > 0 ? Items[selectedIndex] : null); }
            set { Items[selectedIndex] = value; }
        }

        public string SelectedValue
        {
            get { return (Items.Count > 0 ? Items[selectedIndex] : null); }
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return Items.Count;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return Items[(int)row];
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            selectedIndex = (int)row;
            ValueChanged?.Invoke(this, new EventArgs());
        }
    }
}