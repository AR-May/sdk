﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using FluentAssertions;
using Microsoft.NET.TestFramework;
using Microsoft.NET.TestFramework.Commands;
using Xunit;
using Xunit.Abstractions;
using Microsoft.NET.TestFramework.Assertions;
using Microsoft.NET.TestFramework.ProjectConstruction;
using System.Collections.Generic;

namespace Microsoft.NET.Build.Tests
{
    public class GivenThatWeWantToProduceReferenceAssembly : SdkTest
    {
        public GivenThatWeWantToProduceReferenceAssembly(ITestOutputHelper log) : base(log)
        {}

        [RequiresMSBuildVersionTheory("16.8.0")]
        [InlineData("netcoreapp3.1", ".csproj", false)]
        [InlineData("net6.0", ".fsproj", false)]
        [InlineData("net5.0", ".csproj", true)]
        [InlineData("net7.0", ".fsproj", true)]
#pragma warning disable xUnit1025 // InlineData duplicates
        [InlineData(ToolsetInfo.CurrentTargetFramework, ".csproj", true)]
        [InlineData(ToolsetInfo.CurrentTargetFramework, ".fsproj", true)]
#pragma warning restore xUnit1025 // InlineData duplicates
        public void It_produces_ref_assembly_for_appropriate_frameworks(string targetFramework, string extension, bool expectedExists)
        {
            TestProject testProject = new TestProject()
            {
                Name = "ProduceRefAssembly",
                IsExe = true,
                TargetFrameworks = targetFramework,
            };

            var testAsset = _testAssetsManager.CreateTestProject(testProject, identifier: targetFramework, targetExtension:extension);

            var buildCommand = new BuildCommand(testAsset);
            buildCommand.Execute()
                .Should()
                .Pass();
            var filePath = Path.Combine(testAsset.Path, testProject.Name, "obj", "Debug", targetFramework, "ref", $"{testProject.Name}.dll");
            File.Exists(filePath).Should().Be(expectedExists);
        }
    }
}
