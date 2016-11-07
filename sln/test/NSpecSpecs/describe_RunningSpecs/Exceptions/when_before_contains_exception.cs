﻿using System;
using NSpec;
using NSpec.Domain;
using NSpecSpecs.WhenRunningSpecs;
using NUnit.Framework;
using FluentAssertions;

namespace NSpecSpecs.describe_RunningSpecs.Exceptions
{
    [TestFixture]
    [Category("RunningSpecs")]
    public class when_before_contains_exception : when_running_specs
    {
        class BeforeThrowsSpecClass : nspec
        {
            void method_level_context()
            {
                before = () => { throw new BeforeException(); };

                it["should fail this example because of before"] = () => Assert.That(true, Is.True);

                it["should also fail this example because of before"] = () => Assert.That(true, Is.True);

                it["overrides exception from same level it"] = () => { throw new ItException(); };

                context["exception thrown by both before and nested before"] = () =>
                {
                    before = () => { throw new NestedBeforeException(); };

                    it["overrides exception from nested before"] = () => Assert.That(true, Is.True);
                };

                context["exception thrown by both before and nested act"] = () =>
                {
                    act = () => { throw new ActException(); };

                    it["overrides exception from nested act"] = () => Assert.That(true, Is.True);
                };

                context["exception thrown by both before and nested it"] = () =>
                {
                    it["overrides exception from nested it"] = () => { throw new ItException(); };
                };

                context["exception thrown by both before and nested after"] = () =>
                {
                    it["overrides exception from nested after"] = () => Assert.That(true, Is.True);

                    after = () => { throw new AfterException(); };
                };
            }
        }

        [SetUp]
        public void setup()
        {
            Run(typeof(BeforeThrowsSpecClass));
        }

        [Test]
        public void the_example_level_failure_should_indicate_a_context_failure()
        {
            TheExample("should fail this example because of before")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("should also fail this example because of before")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from same level it")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from nested before")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from nested act")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from nested it")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
            TheExample("overrides exception from nested after")
                .Exception.GetType().Should().Be(typeof(ExampleFailureException));
        }

        [Test]
        public void examples_with_only_before_failure_should_fail_because_of_before()
        {
            TheExample("should fail this example because of before")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
            TheExample("should also fail this example because of before")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
        }

        [Test]
        public void it_should_throw_exception_from_before_not_from_same_level_it()
        {
            TheExample("overrides exception from same level it")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
        }

        [Test]
        public void it_should_throw_exception_from_before_not_from_nested_before()
        {
            TheExample("overrides exception from nested before")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
        }

        [Test]
        public void it_should_throw_exception_from_before_not_from_nested_act()
        {
            TheExample("overrides exception from nested act")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
        }

        [Test]
        public void it_should_throw_exception_from_before_not_from_nested_it()
        {
            TheExample("overrides exception from nested it")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
        }

        [Test]
        public void it_should_throw_exception_from_before_not_from_nested_after()
        {
            TheExample("overrides exception from nested after")
                .Exception.InnerException.GetType().Should().Be(typeof(BeforeException));
        }
    }
}
