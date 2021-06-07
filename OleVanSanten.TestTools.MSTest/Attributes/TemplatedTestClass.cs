using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Structure;

namespace OleVanSanten.TestTools.MSTest
{
    [AttributeEquivalent("Microsoft.VisualStudio.TestTools.UnitTesting.TestClass")]
    public class TemplatedTestClass : Attribute
    {
    }
}
