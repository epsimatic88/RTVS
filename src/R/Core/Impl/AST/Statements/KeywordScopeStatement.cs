﻿using System.Diagnostics;
using Microsoft.R.Core.AST.Definitions;
using Microsoft.R.Core.AST.Scopes.Definitions;
using Microsoft.R.Core.AST.Statements.Definitions;
using Microsoft.R.Core.Parser;

namespace Microsoft.R.Core.AST.Statements
{
    /// <summary>
    /// Statement with keyword and scope { } such as repeat { } and else { }
    /// </summary>
    [DebuggerDisplay("[{Text}]")]
    public sealed class KeywordScopeStatement : KeywordStatement, IKeywordScopeStatement
    {
        public IScope Scope { get; private set; }

        private bool _allowsSimpleScope;

        public KeywordScopeStatement(bool allowsSimpleScope)
        {
            _allowsSimpleScope = allowsSimpleScope;
        }

        public override bool Parse(ParseContext context, IAstNode parent)
        {
            if (ParseKeyword(context, parent))
            {
                IScope scope = RParser.ParseScope(context, this, _allowsSimpleScope, terminatingKeyword: null);
                if (scope != null)
                {
                    this.Scope = scope;
                }

                this.Parent = parent;
                return true;
            }

            return false;
        }
    }
}