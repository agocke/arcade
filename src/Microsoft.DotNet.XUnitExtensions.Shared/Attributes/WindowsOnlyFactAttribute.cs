// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable
using System;

using System.Runtime.InteropServices;
using Microsoft.DotNet.XUnitExtensions;

namespace Xunit
{
    /// <summary>
    /// This test should be run only on Windows.
    /// </summary>
#if USES_XUNIT_3
    public class WindowsOnlyFactAttribute : Attribute
#else
    public class WindowsOnlyFactAttribute : FactAttribute
#endif
    {
#if USES_XUNIT_3
        // In xunit v3 AOT, FactAttribute is sealed so custom attributes derive from Attribute.
        // Tests using this attribute must also have [Fact] for the AOT source generator.
        public string? Skip { get; set; }
#endif
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsOnlyFactAttribute"/> class.
        /// </summary>
        /// <param name="additionalMessage">The additional message that is appended to skip reason, when test is skipped.</param>
        public WindowsOnlyFactAttribute(string? additionalMessage = null)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.Skip = "This test requires Windows to run.".AppendAdditionalMessage(additionalMessage);
            }
        }
    }
}
