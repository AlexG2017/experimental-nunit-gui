using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nunit.Gui.Model;
using NUnit.Gui.Presenters;
using NUnit.UiKit.Elements;

namespace NUnit.Gui.Views
{
    public partial class CategoryView : UserControl, ICategoryView
    {
        private readonly IChecked _excludeCommand;
        private readonly ICommand _addCategoryCommand;
        private readonly ICommand _removeCategoryCommand;
        private readonly IListElement _availableCategories;
        private readonly IListElement _selectedCategories;

        public CategoryView()
        {
            InitializeComponent();

            _excludeCommand = new CheckBoxElement(excludeCategories);

            _addCategoryCommand = new ButtonElement(addCategory);
            _addCategoryCommand.Execute += () =>
            {
                if (AddCategory(AvailableCategories.SelectedItem))
                {
                    AvailableCategories.SelectedItem = null;
                }
            };

            _removeCategoryCommand = new ButtonElement(removeCategory);
            _removeCategoryCommand.Execute += () =>
            {
                if (RemoveCategory(SelectedCategories.SelectedItem))
                {
                    SelectedCategories.SelectedItem = null;
                }
            };
            
            _availableCategories = new ListElement(availableCatagories);
            _availableCategories.Sorted = true;
            _availableCategories.MouseItemDoubleClick += (sender, args) => AddCategory(args.Value);
            _selectedCategories = new ListElement(selectedCatagories);
            _selectedCategories.MouseItemDoubleClick += (sender, args) => RemoveCategory(args.Value);
        }

        public ICommand AddCategoryCommand
        {
            get { return _addCategoryCommand; }
        }

        public ICommand RemoveCategoryCommand
        {
            get { return _removeCategoryCommand; }
        }
        public IChecked ExcludeCommand
        {
            get { return _excludeCommand; }
        }

        public IListElement AvailableCategories
        {
            get { return _availableCategories; }
        }

        public IListElement SelectedCategories
        {
            get { return _selectedCategories; }
        }

        private bool AddCategory(string category)
        {
            if (category != null
                && !Array.Exists(SelectedCategories.Items, x => x == category))
            {
                SelectedCategories.Add(category);
                AvailableCategories.Remove(category);
                return true;
            }

            return false;
        }

        private bool RemoveCategory(string category)
        {
            if (category != null
                && !Array.Exists(AvailableCategories.Items, x => x == category))
            {
                AvailableCategories.Add(category);
                SelectedCategories.Remove(category);
                return true;
            }

            return false;
        }
    }
}
