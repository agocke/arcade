// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using Microsoft.DotNet.XUnitExtensions;
#if USES_XUNIT_3
using Xunit.Internal;
#endif

namespace Xunit 
{
    /// <summary>
    /// This test should be run only on .NET (.NET Core).
    /// </summary>
#if USES_XUNIT_3
    public class DotNetOnlyTheoryAttribute : TheoryAttributeBase
#else
    public class DotNetOnlyTheoryAttribute : TheoryAttribute
#endif
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetOnlyTheoryAttribute"/> class.
        /// </summary>
        /// <param name="additionalMessage">The additional message that is appended to skip reason, when test is skipped.</param>
        public DotNetOnlyTheoryAttribute(string? additionalMessage = null)
        {
            if (!DiscovererHelpers.IsRunningOnNetCoreApp)
            {
                this.Skip = "This test only runs on .NET.".AppendAdditionalMessage(additionalMessage);
            }
        }
    }
}
