using System;
using FluentAssertions;
using Xunit;

namespace PolicyServerLocal.Tests
{
    public class PolicyTests
    {
        Policy _subject;

        public PolicyTests()
        {
            _subject = new Policy();
        }

        [Fact]
        public void Evaluate_should_require_user()
        {
            Action a = () => _subject.Evaluate(null);
            a.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Evaluate_should_return_matched_roles()
        {
            _subject.Roles.AddRange(new [] {
                new Role{ Name = "c", Subjects = { "1" } },
                new Role{ Name = "a", Subjects = { "1" } },
                new Role{ Name = "b", Subjects = { "2" } },
            });

            var user = TestUser.Create("1");

            var result = _subject.Evaluate(user);

            result.Roles.ShouldAllBeEquivalentTo(new[] { "a", "c" });
        }

        [Fact]
        public void Evaluate_should_not_return_unmatched_roles()
        {
            _subject.Roles.AddRange(new[] {
                new Role{ Name = "c", Subjects = { "2" } },
                new Role{ Name = "a", Subjects = { "3" } },
                new Role{ Name = "b", Subjects = { "2" } },
            });

            var user = TestUser.Create("1");

            var result = _subject.Evaluate(user);

            result.Roles.Should().BeEmpty();
        }

        [Fact]
        public void Evaluate_should_return_remove_duplicate_roles()
        {
            _subject.Roles.AddRange(new[] {
                new Role{ Name = "a", Subjects = { "1" } },
                new Role{ Name = "a", Subjects = { "1" } },
            });

            var user = TestUser.Create("1");

            var result = _subject.Evaluate(user);

            result.Roles.ShouldAllBeEquivalentTo(new[] { "a" });
        }

        [Fact]
        public void Evaluate_should_return_matched_permissions()
        {
            _subject.Roles.AddRange(new[] {
                new Role{ Name = "role", Subjects = { "1" } },
                new Role{ Name = "xoxo", Subjects = { "2" } },
            });
            _subject.Permissions.AddRange(new [] {
                new Permission{ Name = "a", Roles = { "role" } },
                new Permission{ Name = "c", Roles = { "role" } },
                new Permission{ Name = "b", Roles = { "xoxo" } },
            });

            var user = TestUser.Create("1");

            var result = _subject.Evaluate(user);

            result.Permissions.ShouldAllBeEquivalentTo(new[] { "a", "c" });
        }

        [Fact]
        public void Evaluate_should_not_return_unmatched_permissions()
        {
            _subject.Roles.AddRange(new[] {
                new Role{ Name = "role", Subjects = { "1" } },
            });
            _subject.Permissions.AddRange(new[] {
                new Permission{ Name = "a", Roles = { "xoxo" } },
                new Permission{ Name = "c", Roles = { "xoxo" } },
                new Permission{ Name = "b", Roles = { "xoxo" } },
            });

            var user = TestUser.Create("1");

            var result = _subject.Evaluate(user);

            result.Permissions.Should().BeEmpty();
        }

        [Fact]
        public void Evaluate_should_remove_duplicate_permissions()
        {
            _subject.Roles.AddRange(new[] {
                new Role{ Name = "role", Subjects = { "1" } },
            });
            _subject.Permissions.AddRange(new[] {
                new Permission{ Name = "a", Roles = { "role" } },
                new Permission{ Name = "a", Roles = { "role" } },
            });

            var user = TestUser.Create("1");

            var result = _subject.Evaluate(user);

            result.Permissions.ShouldAllBeEquivalentTo(new[] { "a" });
        }

        [Fact]
        public void Evaluate_should_allow_identity_roles_to_match_permissions()
        {
            _subject.Permissions.AddRange(new[] {
                new Permission{ Name = "perm", Roles = { "role" } },
            });

            var user = TestUser.Create("1", roles:new[] { "role" });

            var result = _subject.Evaluate(user);

            result.Permissions.ShouldAllBeEquivalentTo(new[] { "perm" });
        }
    }
}
