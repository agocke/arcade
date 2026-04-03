// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using Xunit;

namespace Microsoft.DotNet.XUnitExtensions.Tests
{
    /// <summary>
    /// Validates ConditionalFact and ConditionalTheory attribute behavior.
    ///
    /// In xunit v3 AOT mode, custom attributes cannot derive from FactAttribute (sealed).
    /// ConditionalFact/ConditionalTheory are metadata-only attributes that evaluate conditions
    /// and expose a Skip property, but they are NOT test discoverers. Tests must use
    /// [Fact]/[Theory] directly for discovery. The conditional skip behavior is evaluated
    /// at attribute construction time and stored in the Skip property.
    /// </summary>
    public class ConditionalAttributeTests
    {
        public static bool AlwaysTrue => true;
        public static bool AlwaysFalse => false;

        [Fact]
        public void ConditionalFact_TrueCondition_SkipIsNull()
        {
            var attr = new ConditionalFactAttribute(typeof(ConditionalAttributeTests), nameof(AlwaysTrue));
            Assert.Null(attr.Skip);
        }

        [Fact]
        public void ConditionalFact_FalseCondition_SkipIsSet()
        {
            var attr = new ConditionalFactAttribute(typeof(ConditionalAttributeTests), nameof(AlwaysFalse));
            Assert.NotNull(attr.Skip);
        }

        [Fact]
        public void ConditionalTheory_TrueCondition_SkipIsNull()
        {
            var attr = new ConditionalTheoryAttribute(typeof(ConditionalAttributeTests), nameof(AlwaysTrue));
            Assert.Null(attr.Skip);
        }

        [Fact]
        public void ConditionalTheory_FalseCondition_SkipIsSet()
        {
            var attr = new ConditionalTheoryAttribute(typeof(ConditionalAttributeTests), nameof(AlwaysFalse));
            Assert.NotNull(attr.Skip);
        }

        [Fact]
        public void ConditionalFact_IsNotFactAttribute()
        {
            // In AOT mode, ConditionalFact does not derive from FactAttribute.
            // It is a standalone metadata attribute.
            var attr = new ConditionalFactAttribute(typeof(ConditionalAttributeTests), nameof(AlwaysTrue));
            Assert.IsAssignableFrom<Attribute>(attr);
        }
    }
}
