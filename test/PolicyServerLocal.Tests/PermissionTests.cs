// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using FluentAssertions;
using PolicyServer.Local;
using Xunit;

namespace PolicyServerLocal.Tests
{
    public class PermissionTests
    {
        Permission _subject;

        public PermissionTests()
        {
            _subject = new Permission();
        }

        [Fact]
        public void Evaluate_should_require_roles()
        {
            Action a = () => _subject.Evaluate(null);
            a.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Evaluate_should_fail_for_invalid_roles()
        {
            var result = _subject.Evaluate(new[] { "foo" });
            result.Should().BeFalse();
        }

        [Fact]
        public void Evaluate_should_succeed_for_valid_roles()
        {
            _subject.Roles.Add("foo");
            var result = _subject.Evaluate(new[] { "foo" });
            result.Should().BeTrue();
        }
    }
}