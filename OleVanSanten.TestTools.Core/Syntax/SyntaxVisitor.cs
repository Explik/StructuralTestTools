using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using OleVanSanten.TestTools.Helpers;

namespace OleVanSanten.TestTools.Syntax
{
    public class SyntaxVisitor : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression node)
        {
            ISyntaxTransformer transformer = node.Member.GetCustomTransformer();

            if (transformer != null)
            {
                return base.Visit(transformer.Transform(node));
            }
            return base.VisitMember(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            ISyntaxTransformer transformer = node.Method.GetCustomTransformer();
            
            if (transformer != null)
            {
                return base.Visit(transformer.Transform(node));
            }
            return base.VisitMethodCall(node);
        }
    }
}
