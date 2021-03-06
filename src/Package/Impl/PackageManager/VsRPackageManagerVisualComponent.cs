﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.Common.Core;
using Microsoft.R.Components.InteractiveWorkflow;
using Microsoft.R.Components.PackageManager;
using Microsoft.R.Components.Search;
using Microsoft.R.Components.View;
using Microsoft.R.Host.Client;
using Microsoft.R.Support.Settings;
using Microsoft.VisualStudio.R.Package.Shell;
using Microsoft.VisualStudio.R.Package.Windows;

namespace Microsoft.VisualStudio.R.Package.PackageManager {
    [Export(typeof(IRPackageManagerVisualComponentContainerFactory))]
    internal class VsRPackageManagerVisualComponentContainerFactory : ToolWindowPaneFactory<PackageManagerWindowPane>, IRPackageManagerVisualComponentContainerFactory { 
        private readonly ISearchControlProvider _searchControlProvider;

        [ImportingConstructor]
        public VsRPackageManagerVisualComponentContainerFactory(ISearchControlProvider searchControlProvider) {
            _searchControlProvider = searchControlProvider;
        }

        public IVisualComponentContainer<IRPackageManagerVisualComponent> GetOrCreate(IRPackageManager packageManager, IRSession session, int instanceId = 0) {
            return GetOrCreate(instanceId, i => new PackageManagerWindowPane(packageManager, session, _searchControlProvider, RToolsSettings.Current, VsAppShell.Current));
        }
    }
}