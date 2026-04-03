// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.Runtime.InteropServices;
using Microsoft.DotNet.XUnitExtensions;
#if USES_XUNIT_3
using Xunit.Internal;
#endif

namespace Xunit
{
    /// <summary>
    /// This test should be run only on Unix (Linux, OSX platforms).
    /// </summary>
#if USES_XUNIT_3
    public class UnixOnlyTheoryAttribute : TheoryAttributeBase
#else
    public class UnixOnlyTheoryAttribute : TheoryAttribute
#endif
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnixOnlyTheoryAttribute"/> class.
        /// </summary>
        /// <param name="additionalMessage">The additional message that is appended to skip reason, when test is skipped.</param>
        public UnixOnlyTheoryAttribute(string? additionalMessage = null)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.Skip = "This test requires Unix to run.".AppendAdditionalMessage(additionalMessage);
            }
        }
    }
}
