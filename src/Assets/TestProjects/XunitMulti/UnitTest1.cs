// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Xunit;

namespace TestNamespace
{
    public class VSTestXunitTests
    {
        [Fact(Skip="tmp")]
        public void VSTestXunitPassTest()
        {
        }

        [Fact(Skip="tmp")]
        public void VSTestXunitFailTest()
        {
            Assert.Equal(1, 2);
        }

#if DESKTOP
        [Fact(Skip="tmp")]
        public void VSTestXunitPassTestDesktop()
        {
        }
#else
        [Fact(Skip="tmp")]
        public void VSTestXunitFailTestNetCoreApp()
        {
            Assert.Equal(1, 2);
        }
#endif
    }
}
