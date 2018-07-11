using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Nunit.Gui.Model;

namespace NUnit.UiKit.Elements
{
    public class ListElement : ControlElement<ListBox>, IListElement
    {
        public ListElement(ListBox listBox)
            : base(listBox)
        {
            Control.MouseDoubleClick += (sender, args) =>
            {
                if (args.Button == MouseButtons.Left)
                {
                    var idx = Control.IndexFromPoint(args.Location);
                    if (idx >= 0)
                    {
                        MouseItemDoubleClick?.Invoke(this, new GenericEventArgs<string>(Items[idx]));
                    }
                }
            };
        }

        public event GenericEventHandler<string[]> ItemsChanged;
        public event GenericEventHandler<string> MouseItemDoubleClick;

        public string[] Items
        {
            get
            {
                string[] items = new string[Control.Items.Count];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = Control.Items[i]?.ToString();
                }
                return items;
            }
        }

        public bool Sorted
        {
            get { return Control.Sorted; }
            set
            {
                InvokeIfRequired(() =>
                {
                    Control.Sorted = value;
                });
            }

        }

        public string SelectedItem
        {
            get { return Control.SelectedItem as string; }

            set
            {
                InvokeIfRequired(() =>
                {
                    Control.SelectedItem = value;
                });
            }
        }

        public void Add(string item)
        {
            InvokeIfRequired(() =>
            {
                Control.Items.Add(item);
                ItemsChanged?.Invoke(this, new GenericEventArgs<string[]>(Items));
            });
        }

        public void AddRange(string[] items)
        {
            InvokeIfRequired(() =>
            {
                Control.Items.AddRange(items);
                ItemsChanged?.Invoke(this, new GenericEventArgs<string[]>(Items));
            });
        }

        public void Remove(string item)
        {
            InvokeIfRequired(() =>
            {
                Control.Items.Remove(item);
                ItemsChanged?.Invoke(this, new GenericEventArgs<string[]>(Items));
            });
        }

        public void Clear()
        {
            InvokeIfRequired(() =>
            {
                Control.Items.Clear();
                ItemsChanged?.Invoke(this, new GenericEventArgs<string[]>(Items));
            });
        }
    }
}
