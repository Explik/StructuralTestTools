using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools;

namespace OleVanSanten.TestTools.MSTest
{
    [EquivalentAttribute("Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod")]
    [EquivalentException("Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException")]
    public class TemplatedTestMethodAttribute : Attribute
    {
        public TemplatedTestMethodAttribute(string message)
        {
        }
    }
}
