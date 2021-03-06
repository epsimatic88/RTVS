﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Languages.Editor.EditorFactory;
using Microsoft.Languages.Editor.Services;
using Microsoft.R.Components.ContentTypes;
using Microsoft.R.Core.Parser;
using Microsoft.R.Editor.Document;
using Microsoft.R.Editor.Tree;
using Microsoft.VisualStudio.Editor.Mocks;
using Microsoft.VisualStudio.Text;

namespace Microsoft.R.Editor.Test.Mocks {
    [ExcludeFromCodeCoverage]
    public sealed class EditorDocumentMock : IREditorDocument {
        public EditorDocumentMock(string content, string filePath = null) {
            var tb = new TextBufferMock(content, RContentTypeDefinition.ContentType);
            EditorTree = new EditorTreeMock(tb, RParser.Parse(content));
            ServiceManager.AddService<IREditorDocument>(this, tb, null);
            if (!string.IsNullOrEmpty(filePath)) {
                tb.Properties.AddProperty(typeof(ITextDocument), new TextDocumentMock(tb, filePath));
            }
        }

        public EditorDocumentMock(IEditorTree tree) {
            EditorTree = tree;
            ServiceManager.AddService<IREditorDocument>(this, tree.TextBuffer, null);
        }

        public IEditorTree EditorTree { get; private set; }

        public void Close() { }

        public bool IsTransient => false;

        public bool IsClosed { get; private set; }

        public bool IsMassiveChangeInProgress => false;

        public ITextBuffer TextBuffer => EditorTree.TextBuffer;

        public string FilePath { get; set; }

#pragma warning disable 67
        private readonly object _syncObj = new object();

        public event EventHandler<EventArgs> Activated;
        public event EventHandler<EventArgs> Deactivated;

        public EventHandler<EventArgs> DocumentClosing { get; private set; }
        event EventHandler<EventArgs> IEditorDocument.DocumentClosing {
            add {
                lock (_syncObj) {
                    DocumentClosing = (EventHandler<EventArgs>)Delegate.Combine(DocumentClosing, value);
                }
            }
            remove {
                lock (_syncObj) {
                    DocumentClosing = (EventHandler<EventArgs>)Delegate.Remove(DocumentClosing, value);
                }
            }
        }

        public event EventHandler<EventArgs> MassiveChangeBegun;
        public event EventHandler<EventArgs> MassiveChangeEnded;

        public void BeginMassiveChange() { }

        public void Dispose() { }

        public bool EndMassiveChange() => true;
    }
}
