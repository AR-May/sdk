// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.NET.TestFramework;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.DotNet.PackageValidation.Tests
{
    public class CompatibleFrameworkValidatorTests : SdkTest
    {
        private TestLogger _log;

        public CompatibleFrameworkValidatorTests(ITestOutputHelper log) : base(log)
        {
            _log = new TestLogger();
        }

        [Fact(Skip="tmp")]
        public void MissingRidLessAssetForFramework()
        {
            string[] filePaths = new[]
            {
                @"ref/netcoreapp3.1/TestPackage.dll",
                @"runtimes/win/lib/netcoreapp3.1/TestPackage.dll"
            };

            Package package = new("TestPackage", "1.0.0", filePaths, null, null);
            new CompatibleTfmValidator(string.Empty, null, false, false, _log).Validate(package);
            Assert.Single(_log.errors);
            Assert.Equal(DiagnosticIds.CompatibleRuntimeRidLessAsset + " " + string.Format(Resources.NoCompatibleRuntimeAsset, ".NETCoreApp,Version=v3.1"), _log.errors[0]);
        }

        [Fact(Skip="tmp")]
        public void MissingAssetForFramework()
        {
            string[] filePaths = new[]
            {
                @"ref/netstandard2.0/TestPackage.dll",
                @"lib/netcoreapp3.1/TestPackage.dll"
            };

            Package package = new("TestPackage", "1.0.0", filePaths, null, null);
            new CompatibleTfmValidator(string.Empty, null, false, false, _log).Validate(package); ;
            Assert.NotEmpty(_log.errors);
            Assert.Contains(DiagnosticIds.CompatibleRuntimeRidLessAsset + " " + string.Format(Resources.NoCompatibleRuntimeAsset, ".NETStandard,Version=v2.0"), _log.errors);
        }

        [Fact(Skip="tmp")]
        public void MissingRidSpecificAssetForFramework()
        {
            string[] filePaths = new[]
            {
                @"ref/netcoreapp2.0/TestPackage.dll",
                @"ref/netcoreapp3.1/TestPackage.dll",
                @"lib/netcoreapp3.1/TestPackage.dll",
                @"runtimes/win/lib/netcoreapp3.1/TestPackage.dll"
            };

            Package package = new("TestPackage", "1.0.0", filePaths, null, null);
            new CompatibleTfmValidator(string.Empty, null, false, false, _log).Validate(package);
            Assert.NotEmpty(_log.errors);
            Assert.Contains(DiagnosticIds.CompatibleRuntimeRidLessAsset + " " + string.Format(Resources.NoCompatibleRuntimeAsset, ".NETCoreApp,Version=v2.0"), _log.errors);
            Assert.Contains(DiagnosticIds.CompatibleRuntimeRidSpecificAsset + " " + string.Format(Resources.NoCompatibleRidSpecificRuntimeAsset, ".NETCoreApp,Version=v2.0", "win"), _log.errors);
        }

        [Fact(Skip="tmp")]
        public void OnlyRuntimeAssembly()
        {
            string[] filePaths = new[]
            {
                @"runtimes/win/lib/netstandard2.0/TestPackage.dll"
            };

            Package package = new("TestPackage", "1.0.0", filePaths, null, null);
            new CompatibleTfmValidator(string.Empty, null, false, false, _log).Validate(package);

            Assert.NotEmpty(_log.errors);
            Assert.Contains(DiagnosticIds.ApplicableCompileTimeAsset + " " + string.Format(Resources.NoCompatibleCompileTimeAsset, ".NETStandard,Version=v2.0"), _log.errors);
        }

        [Fact(Skip="tmp")]
        public void LibAndRuntimeAssembly()
        {
            string[] filePaths = new[]
            {
                @"lib/netcoreapp3.1/TestPackage.dll",
                @"runtimes/win/lib/netcoreapp3.1/TestPackage.dll",
            };

            Package package = new("TestPackage", "1.0.0", filePaths, null, null);
            new CompatibleTfmValidator(string.Empty, null, false, false, _log).Validate(package);
            Assert.Empty(_log.errors);
        }

        [Fact(Skip="tmp")]
        public void NoCompileTimeAssetForSpecificFramework()
        {
            string[] filePaths = new[]
            {
                @"ref/netcoreapp3.0/TestPackage.dll",
                @"lib/netstandard2.0/TestPackage.dll",
                @"lib/netcoreapp3.1/TestPackage.dll"
            };

            Package package = new("TestPackage", "1.0.0", filePaths, null, null);
            new CompatibleTfmValidator(string.Empty, null, false, false, _log).Validate(package);
            Assert.NotEmpty(_log.errors);
            Assert.Contains(DiagnosticIds.ApplicableCompileTimeAsset + " " +string.Format(Resources.NoCompatibleCompileTimeAsset, ".NETStandard,Version=v2.0"), _log.errors);
        }

        [Fact(Skip="tmp")]
        public void NoRuntimeAssetForSpecificFramework()
        {
            string[] filePaths = new[]
            {
                @"ref/netcoreapp3.0/TestPackage.dll",
                @"runtimes/win/lib/netcoreapp3.0/TestPackage.dll"
            };

            Package package = new("TestPackage", "1.0.0", filePaths, null, null);
            new CompatibleTfmValidator(string.Empty, null, false, false, _log).Validate(package);
            Assert.NotEmpty(_log.errors);
            Assert.Contains(DiagnosticIds.CompatibleRuntimeRidLessAsset +  " " + string.Format(Resources.NoCompatibleRuntimeAsset, ".NETCoreApp,Version=v3.0"), _log.errors);
        }

        [Fact(Skip="tmp")]
        public void NoRuntimeSpecificAssetForSpecificFramework()
        {
            string[] filePaths = new[]
            {
                @"lib/netstandard2.0/TestPackage.dll",
                @"lib/netcoreapp3.0/TestPackage.dll",
                @"runtimes/win/lib/netcoreapp3.0/TestPackage.dll",
                @"runtimes/unix/lib/netcoreapp3.0/TestPackage.dll"
            };

            Package package = new("TestPackage", "1.0.0", filePaths, null, null);
            new CompatibleTfmValidator(string.Empty, null, false, false, _log).Validate(package);
            Assert.Empty(_log.errors);
        }

        [Fact(Skip="tmp")]
        public void CompatibleLibAsset()
        {
            string[] filePaths = new[]
            {
                @"ref/netcoreapp2.0/TestPackage.dll",
                @"lib/netstandard2.0/TestPackage.dll"
            };

            Package package = new("TestPackage", "1.0.0", filePaths, null, null);
            new CompatibleTfmValidator(string.Empty, null, false, false, _log).Validate(package);
            Assert.NotEmpty(_log.errors);
            Assert.Contains(DiagnosticIds.ApplicableCompileTimeAsset + " " + string.Format(Resources.NoCompatibleCompileTimeAsset, ".NETStandard,Version=v2.0"), _log.errors);
        }

        [Fact(Skip="tmp")]
        public void CompatibleRidSpecificAsset()
        {
            string[] filePaths = new[]
            {
                @"lib/netcoreapp2.0/TestPackage.dll",
                @"lib/netcoreapp3.0/TestPackage.dll",
                @"runtimes/win/lib/netcoreapp3.0/TestPackage.dll"
            };

            Package package = new("TestPackage", "1.0.0", filePaths, null, null);
            new CompatibleTfmValidator(string.Empty, null, false, false, _log).Validate(package);
            Assert.Empty(_log.errors);
        }

        [Fact(Skip="tmp")]
        public void CompatibleFrameworksWithDifferentAssets()
        {
            string[] filePaths = new[]
            {
                @"ref/netstandard2.0/TestPackage.dll",
                @"ref/netcoreapp3.1/TestPackage.dll",
                @"lib/netstandard2.0/TestPackage.dll",
                @"lib/net5.0/TestPackage.dll"
            };

            Package package = new("TestPackage", "1.0.0", filePaths, null, null);
            new CompatibleTfmValidator(string.Empty, null, false, false, _log).Validate(package);
            Assert.Empty(_log.errors);
        }

    }
}
