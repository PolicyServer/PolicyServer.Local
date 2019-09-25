// Copyright (c) Brock Allen, Dominick Baier, Michele Leroux Bustamante. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using FluentAssertions;
using PolicyServer.Local;
using Xunit;

namespace PolicyServerLocal.Tests
{
    public class RoleTests
    {
        Role _subject;

        public RoleTests()
        {
            _subject = new Role();
        }

        [Fact]
        public void Evaluate_should_require_user()
        {
            Action a = ()=>_subject.Evaluate(null);
            a.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Evaluate_should_fail_for_invalid_subject()
        {
            var user = TestUser.Create("1");
            var result = _subject.Evaluate(user);

            result.Should().BeFalse();
        }

        [Fact]
        public void Evaluate_should_succeed_for_valid_subject()
        {
            _subject.Subjects.Add("1");

            var user = TestUser.Create("1");
            var result = _subject.Evaluate(user);

            result.Should().BeTrue();
        }

        [Fact]
        public void Evaluate_should_fail_for_invalid_role()
        {
            var user = TestUser.Create("1");
            var result = _subject.Evaluate(user);

            result.Should().BeFalse();
        }

        [Fact]
        public void Evaluate_should_succeed_for_valid_role()
        {
            _subject.IdentityRoles.Add("foo");

            var user = TestUser.Create("1", roles:new[]{ "foo" });
            var result = _subject.Evaluate(user);

            result.Should().BeTrue();
        }
    }
}