using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Explik.StructuralTestTools
{
    public static class SyntaxNodeExtensions
    {
        public static IEnumerable<SyntaxNode> AllDescendantNodes(this SyntaxNode syntaxNode)
        {
            return syntaxNode.DescendantNodes(n => true);
        }

        public static IEnumerable<TSyntaxNode> AllDescendantNodes<TSyntaxNode>(this SyntaxNode syntaxNode) where TSyntaxNode : SyntaxNode
        {
            return syntaxNode.AllDescendantNodes().OfType<TSyntaxNode>();
        }

        public static IEnumerable<SyntaxNode> AllDescendantNodesAndSelf(this SyntaxNode syntaxNode)
        {
            return syntaxNode.DescendantNodesAndSelf(n => true);
        }

        public static IEnumerable<SyntaxNode> AllDescendantNodesAndSelf<TSyntaxNode>(this SyntaxNode syntaxNode) where TSyntaxNode : SyntaxNode
        {
            return syntaxNode.DescendantNodesAndSelf(n => true).OfType<TSyntaxNode>();
        }

        public static IEnumerable<SyntaxNode> AllMatchingDescendantNodes(this SyntaxNode node, string source)
        {
            return node.AllDescendantNodes().Where(descendantNode => {
                var descendantSource = SourceText.From(descendantNode.NormalizeWhitespace().ToFullString(), Encoding.UTF8).ToString();
                return descendantSource == source;
            });
        }

        public static IEnumerable<TSyntaxNode> AllMatchingDescendantNodes<TSyntaxNode>(this SyntaxNode syntaxNode, string source)
        {
            var nodeSource = SyntaxFactory.ParseSyntaxTree(source).GetRoot();

            if (nodeSource is TSyntaxNode)
            {
                return AllMatchingDescendantNodes(syntaxNode, source).OfType<TSyntaxNode>();
            }
            else throw new ArgumentException("Source does not match TSyntaxNode");
        }

        public static string ToSource(this SyntaxNode syntaxNode)
        {
            return SourceText.From(syntaxNode.NormalizeWhitespace().ToFullString(), Encoding.UTF8).ToString();
        }
    }
}
