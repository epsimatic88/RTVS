﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.ComponentModel;
using Microsoft.R.Components.Application.Configuration;

namespace Microsoft.VisualStudio.R.Package.ProjectSystem.PropertyPages.Settings {
    /// <summary>
    /// Represents a single entry in the property grid in the Project | Properties | Settings page
    /// </summary>
    [Browsable(true)]
    [DesignTimeVisible(true)]
    internal sealed class SettingPropertyDescriptor : PropertyDescriptor {
        public IConfigurationSetting Setting { get; }

        public SettingPropertyDescriptor(IConfigurationSetting setting) :
                base(setting.Name, null) {
            Setting = setting;
        }

        public override Type ComponentType => this.GetType();
        public override bool IsReadOnly => false;
        public override Type PropertyType => typeof(string);
        public override bool CanResetValue(object component) => false;
        public override object GetValue(object component) => Setting.Value;
        public override void ResetValue(object component) { }
        public override void SetValue(object component, object value) {
            Setting.Value = value as string;
        }
        public override bool ShouldSerializeValue(object component) => false;

        protected override void FillAttributes(IList attributeList) {
            foreach(var a in Setting.Attributes) {
                attributeList.Add(a.GetDotNetAttribute());
             }
            base.FillAttributes(attributeList);
        }
    }
}
