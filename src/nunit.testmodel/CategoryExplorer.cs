using System;
using System.Collections.Generic;
using NUnit.Gui.Model;

namespace Nunit.Gui
{
    internal static class CategoryExplorer
    {
        internal static string[] Expolre(TestNode tests)
        {
            List<string> categories = new List<string>();
            Expolre(tests, categories);
            return categories.ToArray();
        }

        private static void Expolre(TestNode testNode, List<string> list)
        {
            foreach (string category in testNode.Categories)
            {
                if (!string.IsNullOrEmpty(category) && !list.Contains(category))
                {
                    list.Add(category);
                }
            }

            foreach (TestNode child in testNode.Children)
            {
                Expolre(child, list);
            }
        }
    }
}
