﻿// ***********************************************************************
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
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NUnit.Engine;

namespace NUnit.Gui.Model
{
    public delegate bool TestNodePredicate(TestNode testNode);

    /// <summary>
    /// TestNode represents a single NUnit test in the test model.
    /// 
    /// It is used in three different ways:
    /// 
    /// 1. When tests are loaded, the XML contains all the
    /// info that defines a test.
    /// 
    /// 2. When a test is starting, only the Id, Name and
    /// FullName are available.
    /// 
    /// 3. When a test completes, information about the 
    /// result of running it is added to the full test
    /// information and the derived class ResultNode
    /// is created from it.
    /// </summary>
    public class TestNode : ITestItem
    {
        private static readonly char[] Splitters = new char[] { ',' };

        public const string TestCase = "TestCase";
        public const string TestFixture = "TestFixture";

        #region Constructors

        public TestNode(XmlNode xmlNode)
        {
            Xml = xmlNode;
            IsSuite = Xml.Name == "test-suite" || Xml.Name == "test-run";
            Id = Xml.GetAttribute("id");
            Name = Xml.GetAttribute("name");
            FullName = Xml.GetAttribute("fullname");
            Type = IsSuite ? GetAttribute("type") : TestCase;
            TestCount = IsSuite ? GetAttribute("testcasecount", 0) : 1;
            RunState = GetRunState();
            Categories = GetCategories();
        }

        public TestNode(string xmlText) : this(XmlHelper.CreateXmlNode(xmlText)) { }

        #endregion

        #region Public Properties

        public XmlNode Xml { get; }
        public bool IsSuite { get; }
        public string Id { get; }
        public string Name { get; }
        public string FullName { get; }
        public string Type { get; }
        public int TestCount { get; }
        public RunState RunState { get; private set; }
        public string[] Categories { get; }

        private List<TestNode> _children;
        public IList<TestNode> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new List<TestNode>();
                    foreach (XmlNode node in Xml.ChildNodes)
                        if (node.Name == "test-case" || node.Name == "test-suite")
                            _children.Add(new TestNode(node));
                }

                return _children;
            }
        }

        #endregion

        #region Public Methods

        public bool CanRun()
        {
            return RunState != RunState.Ignored && RunState != RunState.Skipped;
        }

        public TestFilter GetTestFilter()
        {
            return !CanRun()
                ? new TestFilter("<filter><id>-1</id></filter>")
                : new TestFilter(string.Format("<filter><id>{0}</id></filter>", this.Id));
        }

        public string GetAttribute(string name)
        {
            return Xml.GetAttribute(name);
        }

        public int GetAttribute(string name, int defaultValue)
        {
            return Xml.GetAttribute(name, defaultValue);
        }

        public string GetProperty(string name)
        {
            var propNode = Xml.SelectSingleNode("properties/property[@name='" + name + "']");

            return (propNode != null)
                ? propNode.GetAttribute("value")
                : null;
        }

        // Get a comma-separated list of all properties having the specified name
        public string GetPropertyList(string name)
        {
            var propList = Xml.SelectNodes("properties/property[@name='" + name + "']");
            if (propList == null || propList.Count == 0) return string.Empty;

            StringBuilder result = new StringBuilder();

            foreach (XmlNode propNode in propList)
            {
                var val = propNode.GetAttribute("value");
                if (result.Length > 0)
                    result.Append(',');
                result.Append(FormatPropertyValue(val));
            }

            return result.ToString();
        }

        public string[] GetAllProperties(bool displayHiddenProperties)
        {
            var items = new List<string>();

            foreach (XmlNode propNode in this.Xml.SelectNodes("properties/property"))
            {
                var name = propNode.GetAttribute("name");
                var val = propNode.GetAttribute("value");
                if (name != null && val != null)
                    if (displayHiddenProperties || !name.StartsWith("_"))
                        items.Add(name + " = " + FormatPropertyValue(val));
            }

            return items.ToArray();
        }

        public TestSelection Select(TestNodePredicate predicate)
        {
            return Select(predicate, null);
        }

        public TestSelection Select(TestNodePredicate predicate, Comparison<TestNode> comparer)
        {
            var selection = new TestSelection();

            Accumulate(selection, this, predicate);

            if (comparer != null)
                selection.Sort(comparer);

            return selection;
        }

        private void Accumulate(TestSelection selection, TestNode testNode, TestNodePredicate predicate)
        {
            if (predicate(testNode))
                selection.Add(testNode);
            else if (testNode.IsSuite)
                foreach (TestNode child in testNode.Children)
                    Accumulate(selection, child, predicate);
        }

        public void SetRunState(RunState state)
        {
            RunState = state;
        }

        public void ResetRunState()
        {
            RunState = GetRunState();
        }

        #endregion

        #region Helper Methods

        private static string FormatPropertyValue(string val)
        {
            return val == null
                ? "<null>"
                : val == string.Empty
                    ? "<empty>"
                    : val;
        }

        private RunState GetRunState()
        {
            switch (GetAttribute("runstate"))
            {
                case "Runnable":
                    return RunState.Runnable;
                case "NotRunnable":
                    return RunState.NotRunnable;
                case "Ignored":
                    return RunState.Ignored;
                case "Explicit":
                    return RunState.Explicit;
                case "Skipped":
                    return RunState.Skipped;
                default:
                    return RunState.Unknown;
            }
        }

        private string[] GetCategories()
        {
            string categories = GetPropertyList("Category");
            if (!string.IsNullOrEmpty(categories))
            {
                return categories.Split(Splitters, StringSplitOptions.RemoveEmptyEntries);
            }
            return new string[0];
        }

        #endregion
    }
}
