using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace OleVanSanten.TestTools.Syntax
{
    public interface ISyntaxTransformer
    {
        Expression Transform(Expression expression);
    }
}
