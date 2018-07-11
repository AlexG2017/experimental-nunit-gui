using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Nunit.Gui.Model;

namespace NUnit.UiKit.Elements
{
    public interface IListElement : IViewElement
    {
        string SelectedItem { get; set; }

        string[] Items { get; }

        bool Sorted { get; set; }

        void Add(string item);

        void AddRange(string[] items);

        void Remove(string item);

        void Clear();

        event GenericEventHandler<string[]> ItemsChanged;

        event GenericEventHandler<string> MouseItemDoubleClick;
    }
}
