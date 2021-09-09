using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools;

namespace OleVanSanten.TestTools.MSTest
{
    public class TemplatedTestMethodAttribute : Attribute
    {
        public TemplatedTestMethodAttribute(string message)
        {
        }
    }
}
