﻿// ***********************************************************************
// Copyright (c) 2015 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System.Drawing;

namespace NUnit.Gui.Settings
{
    using Engine;

    public class TestTreeSettings : SettingsGroup
    {
        public TestTreeSettings(ISettings settingsService) : base(settingsService, "Gui.TestTree") { }

        public FixtureListSettings FixtureList
        {
            get { return new FixtureListSettings(SettingsService); }
        }

        public TestListSettings TestList
        {
            get { return new TestListSettings(SettingsService); }
        }

        private const string displayFormatKey = "DisplayFormat";
        public string DisplayFormat
        {
            get { return GetSetting(displayFormatKey, "NUNIT_TREE"); }
            set { SaveSetting(displayFormatKey, value); }
        }

        private const string initialTreeDisplayKey = "InitialTreeDisplay";
        public TreeDisplayStyle InitialTreeDisplay
        {
            get { return GetSetting(initialTreeDisplayKey, TreeDisplayStyle.Auto); }
            set { SaveSetting(initialTreeDisplayKey, value); }
        }

        private string clearResultsOnReloadKey = "ClearResultsOnReload";
        public bool ClearResultsOnReload
        {
            get { return GetSetting(clearResultsOnReloadKey, true); }
            set { SaveSetting(clearResultsOnReloadKey, value); }
        }

        private const string saveVisualStateKey = "SaveVisualState";
        public bool SaveVisualState
        {
            get { return GetSetting(saveVisualStateKey, true); }
            set { SaveSetting(saveVisualStateKey, value); }
        }

        private const string showCheckboxesKey = "ShowCheckboxes";
        public bool ShowCheckboxes
        {
            get { return GetSetting(showCheckboxesKey, true); }
            set { SaveSetting(showCheckboxesKey, value); }
        }

        //private const string alternateImageSetKey = "AlternateImageSet";
        //public string AlternateImageSet
        //{
        //    get { return GetSetting(alternateImageSetKey, "Default"); }
        //    set { SaveSetting(alternateImageSetKey, value); }
        //}
    }

    public class FixtureListSettings : SettingsGroup
    {
        public FixtureListSettings(ISettings settings) : base(settings, "Gui.TestTree.FixtureList") { }

        private string groupByKey = "GroupBy";
        public string GroupBy
        {
            get { return GetSetting(groupByKey, "OUTCOME"); }
            set { SaveSetting(groupByKey, value); }
        }
    }

    public class TestListSettings : SettingsGroup
    {
        public TestListSettings(ISettings settings) : base(settings, "Gui.TestTree.TestList") { }

        private string groupByKey = "GroupBy";
        public string GroupBy
        {
            get { return GetSetting(groupByKey, "OUTCOME"); }
            set { SaveSetting(groupByKey, value); }
        }
    }

    public class TestCategorySettings : SettingsGroup
    {
        public TestCategorySettings(ISettings settings) : base(settings, "Gui.TestTree.Category") { }

        private static readonly string CategoriesKey = "Categories";
        private static readonly string ExcludeKey = "Exclude";
        public string[] Categories
        {
            get { return GetSetting(CategoriesKey, string.Empty).Split(','); }
            set { SaveSetting(CategoriesKey, string.Join(",", value)); }
        }

        public bool Exclude
        {
            get { return GetSetting(ExcludeKey, false); }
            set { SaveSetting(ExcludeKey, value); }
        }
    }
}
