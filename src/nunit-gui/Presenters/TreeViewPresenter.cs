// ***********************************************************************
// Copyright (c) 2016 Charlie Poole
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Nunit.Gui;

namespace NUnit.Gui.Presenters
{
    using Engine;
    using Model;
    using Views;

    /// <summary>
    /// TreeViewPresenter is the presenter for the TestTreeView
    /// </summary>
    public class TreeViewPresenter
    {
        private ITestTreeView _view;
        private ITestModel _model;

        private DisplayStrategy _strategy;

        private ITestItem _selectedTestItem;

        private Dictionary<string, TreeNode> _nodeIndex = new Dictionary<string, TreeNode>();

        #region Constructor

        public TreeViewPresenter(ITestTreeView treeView, ITestModel model)
        {
            _view = treeView;
            _model = model;

            var settingsService = _model.GetService<ISettings>();
            Settings = new Settings.TestTreeSettings(settingsService);
            SelectedCategorySettings = new Settings.TestCategorySettings(settingsService);

            InitializeRunCommands();
            WireUpEvents();
        }

        #endregion

        #region Private Members

        private void WireUpEvents()
        {
            // Model actions
            _model.TestLoaded += (ea) =>
            {
                _strategy.OnTestLoaded(ea.Test);
                InitializeRunCommands();
                InitializeCategories(ea.Categories);
                _view.ShowCheckBoxesCommand.Checked = Settings.ShowCheckboxes;
            };

            _model.TestReloaded += (ea) =>
            {
                _strategy.OnTestLoaded(ea.Test);
                InitializeRunCommands();
                ApplyCategoryFilter(_view.Category.SelectedCategories.Items, _view.Category.ExcludeCommand.Checked);
            };

            _model.TestUnloaded += (ea) =>
            {
                _strategy.OnTestUnloaded();
                InitializeRunCommands();
            };

            _model.RunStarting += (ea) => InitializeRunCommands();
            _model.RunFinished += (ea) => InitializeRunCommands();

            _model.TestFinished += (ea) => _strategy.OnTestFinished(ea.Result);
            _model.SuiteFinished += (ea) => _strategy.OnTestFinished(ea.Result);

            // View actions - Initial Load
            _view.Load += (s, e) => SetDefaultDisplayStrategy();

            // View context commands
            _view.Tree.ContextMenu.Popup += () => _view.RunCheckedCommand.Visible = _view.Tree.CheckBoxes && _view.Tree.CheckedNodes.Count > 0;
            _view.CollapseAllCommand.Execute += () => _view.CollapseAll();
            _view.ExpandAllCommand.Execute += () => _view.ExpandAll();
            _view.CollapseToFixturesCommand.Execute += () => _strategy.CollapseToFixtures();
            _view.ShowCheckBoxesCommand.CheckedChanged += () =>
            {
                _view.RunSelectedCommand.Enabled = false;
                _view.Tree.CheckBoxes =
                    _view.CheckAllTestsCommand.Visible =
                        _view.UncheckAllTestsCommand.Visible =
                            _view.CheckFailedTestsCommand.Visible =
                                Settings.ShowCheckboxes = _view.ShowCheckBoxesCommand.Checked;
            };
            _view.CheckAllTestsCommand.Execute += () =>
            {
                var treeView = _view.Tree?.Control;
                if (treeView != null)
                {
                    ToggleNodeCheck(treeView.Nodes, true, true);
                }
            };
            _view.UncheckAllTestsCommand.Execute += () =>
            {
                var treeView = _view.Tree?.Control;
                if (treeView != null)
                {
                    ToggleNodeCheck(treeView.Nodes, false, true);
                }
            };
            _view.CheckFailedTestsCommand.Execute += () =>
            {
                var treeView = _view.Tree?.Control;
                if (treeView != null)
                {
                    ToggleNodeCheck(_strategy.GetFailedNodes(), true, true);
                }
            };
            _view.RunContextCommand.Execute += () => _model.RunTests(_selectedTestItem);
            _view.RunCheckedCommand.Execute += RunCheckedTests;

            // Node selected in tree
            _view.Tree.SelectedNodeChanged += (tn) =>
            {
                _selectedTestItem = tn.Tag as ITestItem;
                _view.RunContextCommand.Enabled = (_selectedTestItem as TestNode)?.CanRun() ?? true;
                _model.NotifySelectedItemChanged(_selectedTestItem);
            };

            // Run button and dropdowns
            _view.RunButton.Execute += () => _model.RunAllTests();
            _view.RunAllCommand.Execute += () => _model.RunAllTests();
            _view.RunSelectedCommand.Execute += () => _model.RunTests(_selectedTestItem);
            _view.RunFailedCommand.Execute += () => RunFailedTest();
            _view.StopRunCommand.Execute += () => _model.CancelTestRun();

            // Change of display format
            _view.DisplayFormat.SelectionChanged += () =>
            {
                SetDisplayStrategy(_view.DisplayFormat.SelectedItem);
                _strategy.Reload();
                ApplyCategoryFilter(_view.Category.SelectedCategories.Items, _view.Category.ExcludeCommand.Checked);
            };

            _view.Category.SelectedCategories.ItemsChanged += (sender, ae) => ApplyCategoryFilter(ae.Value, _view.Category.ExcludeCommand.Checked);
            _view.Category.ExcludeCommand.CheckedChanged += () => ApplyCategoryFilter(_view.Category.SelectedCategories.Items, _view.Category.ExcludeCommand.Checked);
        }

        private void InitializeCategories(string[] categories)
        {
            _view.Category.AvailableCategories.Clear();
            _view.Category.AvailableCategories.AddRange(categories);

            var excludeSelectedCategories = SelectedCategorySettings.Exclude;
            if (SelectedCategorySettings.Categories?.Length > 0)
            {
                var selectedCategories = ArrayExtensions.Intersect(
                    SelectedCategorySettings.Categories,
                    _view.Category.AvailableCategories.Items);
                _view.Category.SelectedCategories.Clear();
                _view.Category.SelectedCategories.AddRange(selectedCategories);
                if (selectedCategories.Length > 0)
                {
                    _view.Category.AvailableCategories.Clear();
                    _view.Category.AvailableCategories.AddRange(ArrayExtensions.Except(categories, selectedCategories));
                }
            }
            _view.Category.ExcludeCommand.Checked = excludeSelectedCategories;
        }

        private void ApplyCategoryFilter(string[] categories, bool excludeCommand)
        {
            SelectedCategorySettings.Categories = categories;
            SelectedCategorySettings.Exclude = excludeCommand;

            _strategy.Filter(x =>
            {
                TestNode node = x as TestNode;
                return node == null
                       || categories.Length == 0
                       || ArrayExtensions.IsIntersect(categories, node.Categories) != excludeCommand;
            }, excludeCommand);
        }

        private void RunCheckedTests()
        {
            RunTestGroup(_view.Tree.CheckedNodes);
        }

        private void RunFailedTest()
        {
            RunTestGroup(_strategy.GetFailedNodes());
        }

        private void RunTestGroup(IList<TreeNode> nodes)
        {
            var tests = new TestGroup("RunTests");
            foreach (TreeNode treeNode in nodes)
            {
                CollectTestGroup(tests, treeNode);
            }

            _model.RunTests(tests);
        }

        private void ToggleNodeCheck(IEnumerable nodes, bool checkedValue, bool recursive = true)
        {
            foreach (TreeNode node in nodes)
            {
                _strategy.ToggleNodeCheck(node, checkedValue, recursive);
            }
        }

        private void CollectTestGroup(TestGroup tests, TreeNode node)
        {
            var testNode = node.Tag as TestNode;
            if (testNode != null && testNode.CanRun())
            {
                if (testNode.Type == TestNode.TestCase)
                {
                    tests.Add(testNode);
                }
                else
                {
                    foreach (TreeNode treeNode in node.Nodes)
                    {
                        CollectTestGroup(tests, treeNode);
                    }
                }
            }
            else
            {
                var group = node.Tag as TestGroup;
                if (group != null)
                    tests.AddRange(group);
            }
        }

        private void InitializeRunCommands()
        {
            bool isRunning = _model.IsTestRunning;
            bool canRun = _model.HasTests && !isRunning;

            // TODO: Figure out how to disable the button click but not the dropdown.
            //_view.RunButton.Enabled = canRun;
            _view.RunAllCommand.Enabled =
                _view.RunSelectedCommand.Enabled =
                    _view.RunContextCommand.Enabled =
                        _view.RunCheckedCommand.Enabled =
                            _view.RunFailedCommand.Enabled = canRun;
            _view.StopRunCommand.Enabled = isRunning;

            if (isRunning)
            {
                ClearTestsResult();
            }
        }

        private void ClearTestsResult()
        {
            _model?.ClearTestResults();
            _strategy?.ClearTestResults();
        }

        private void SetDefaultDisplayStrategy()
        {
            CreateDisplayStrategy(Settings.DisplayFormat);
        }

        private void SetDisplayStrategy(string format)
        {
            CreateDisplayStrategy(format);
            Settings.DisplayFormat = format;
        }

        private void CreateDisplayStrategy(string format)
        {
            switch (format.ToUpperInvariant())
            {
                default:
                case "NUNIT_TREE":
                    _strategy = new NUnitTreeDisplayStrategy(_view, _model);
                    break;
                case "FIXTURE_LIST":
                    _strategy = new FixtureListDisplayStrategy(_view, _model);
                    break;
                case "TEST_LIST":
                    _strategy = new TestListDisplayStrategy(_view, _model);
                    break;
            }

            _view.FormatButton.ToolTipText = _strategy.Description;
            _view.DisplayFormat.SelectedItem = format;
        }

        private Settings.TestTreeSettings Settings { get; }
        private Settings.TestCategorySettings SelectedCategorySettings { get; }

        #endregion
    }
}
