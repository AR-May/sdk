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
    }
}
