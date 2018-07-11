using System;
using System.Collections.Generic;
using System.Text;
using Nunit.Gui.Model;
using NUnit.UiKit.Elements;

namespace NUnit.Gui.Views
{
    public interface ICategoryView : IView
    {
        ICommand AddCategoryCommand { get; }
        ICommand RemoveCategoryCommand { get; }
        IChecked ExcludeCommand { get; }

        IListElement AvailableCategories { get; }
        IListElement SelectedCategories { get; }
    }
}
