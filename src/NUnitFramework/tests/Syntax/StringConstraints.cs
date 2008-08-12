using System;

namespace NUnit.Framework.Syntax
{
    namespace Classic
    {
        public class StringConstraintTests
        {
            [Test]
            public void Substring()
            {
                string phrase = "Hello World!";
                StringAssert.Contains("World", phrase);
            }

            [Test]
            public void StartsWith()
            {
                string phrase = "Hello World!";
                StringAssert.StartsWith("Hello", phrase);
            }

            [Test]
            public void EndsWith()
            {
                string phrase = "Hello World!";
                StringAssert.EndsWith("!", phrase);
            }

            [Test]
            public void EqualIgnoringCase()
            {
                string phrase = "Hello World!";
                StringAssert.AreEqualIgnoringCase("hello world!", phrase);
            }

            [Test]
            public void RegularExpression()
            {
                string phrase = "Now is the time for all good men to come to the aid of their country.";
                StringAssert.IsMatch("all good men", phrase);
                StringAssert.IsMatch("Now.*come", phrase);
            }
        }
    }

    namespace Helpers
    {
        public class StringConstraintTests
        {
            [Test]
            public void Substring()
            {
                string phrase = "Hello World!";
                string[] array = new string[] { "abc", "bad", "dba" };

                Assert.That(phrase, Text.Contains("World"));
                // Only available using new syntax
                Assert.That(phrase, Text.DoesNotContain("goodbye"));
                Assert.That(phrase, Text.Contains("WORLD").IgnoreCase);
                Assert.That(phrase, Text.DoesNotContain("BYE").IgnoreCase);
                Assert.That(array, Text.All.Contains("b"));
            }

            [Test]
            public void StartsWith()
            {
                string phrase = "Hello World!";
                string[] greetings = new string[] { "Hello!", "Hi!", "Hola!" };

                Assert.That(phrase, Text.StartsWith("Hello"));
                // Only available using new syntax
                Assert.That(phrase, Text.DoesNotStartWith("Hi!"));
                Assert.That(phrase, Text.StartsWith("HeLLo").IgnoreCase);
                Assert.That(phrase, Text.DoesNotStartWith("HI").IgnoreCase);
                Assert.That(greetings, Text.All.StartsWith("h").IgnoreCase);
            }

            [Test]
            public void EndsWith()
            {
                string phrase = "Hello World!";
                string[] greetings = new string[] { "Hello!", "Hi!", "Hola!" };

                Assert.That(phrase, Text.EndsWith("!"));
                // Only available using new syntax
                Assert.That(phrase, Text.DoesNotEndWith("?"));
                Assert.That(phrase, Text.EndsWith("WORLD!").IgnoreCase);
                Assert.That(greetings, Text.All.EndsWith("!"));
            }

            [Test]
            public void EqualIgnoringCase()
            {
                string phrase = "Hello World!";

                Assert.That(phrase, Is.EqualTo("hello world!").IgnoreCase);
                //Only available using new syntax
                Assert.That(phrase, Is.Not.EqualTo("goodbye world!").IgnoreCase);
                Assert.That(new string[] { "Hello", "World" },
                    Is.EqualTo(new object[] { "HELLO", "WORLD" }).IgnoreCase);
                Assert.That(new string[] { "HELLO", "Hello", "hello" },
                    Is.All.EqualTo("hello").IgnoreCase);
            }

            [Test]
            public void RegularExpression()
            {
                string phrase = "Now is the time for all good men to come to the aid of their country.";
                string[] quotes = new string[] { "Never say never", "It's never too late", "Nevermore!" };

                Assert.That(phrase, Text.Matches("all good men"));
                Assert.That(phrase, Text.Matches("Now.*come"));
                // Only available using new syntax
                Assert.That(phrase, Text.DoesNotMatch("all.*men.*good"));
                Assert.That(phrase, Text.Matches("ALL").IgnoreCase);
                Assert.That(quotes, Text.All.Matches("never").IgnoreCase);
            }
        }
    }

    namespace Inherited
    {
        public class StringConstraintTests : AssertionHelper
        {
            [Test]
            public void Substring()
            {
                string phrase = "Hello World!";
                string[] array = new string[] { "abc", "bad", "dba" };

                Expect(phrase, Contains("World"));
                // Only available using new syntax
                Expect(phrase, Not.Contains("goodbye"));
                Expect(phrase, Contains("WORLD").IgnoreCase);
                Expect(phrase, Not.Contains("BYE").IgnoreCase);
                Expect(array, All.Contains("b"));
            }

            [Test]
            public void StartsWith()
            {
                string phrase = "Hello World!";
                string[] greetings = new string[] { "Hello!", "Hi!", "Hola!" };

                Expect(phrase, StartsWith("Hello"));
                //// Only available using new syntax
                Expect(phrase, Not.StartsWith("Hi!"));
                Expect(phrase, StartsWith("HeLLo").IgnoreCase);
                Expect(phrase, Not.StartsWith("HI").IgnoreCase);
                Expect(greetings, All.StartsWith("h").IgnoreCase);
            }

            [Test]
            public void EndsWith()
            {
                string phrase = "Hello World!";
                string[] greetings = new string[] { "Hello!", "Hi!", "Hola!" };

                Expect(phrase, EndsWith("!"));
                //// Only available using new syntax
                Expect(phrase, Not.EndsWith("?"));
                Expect(phrase, EndsWith("WORLD!").IgnoreCase);
                Expect(greetings, All.EndsWith("!"));
            }

            [Test]
            public void EqualIgnoringCase()
            {
                string phrase = "Hello World!";

                Expect(phrase, EqualTo("hello world!").IgnoreCase);
                ////Only available using new syntax
                Expect(phrase, Not.EqualTo("goodbye world!").IgnoreCase);
                Expect(new string[] { "Hello", "World" },
                    EqualTo(new object[] { "HELLO", "WORLD" }).IgnoreCase);
                Expect(new string[] { "HELLO", "Hello", "hello" },
                    All.EqualTo("hello").IgnoreCase);
            }

            [Test]
            public void RegularExpression()
            {
                string phrase = "Now is the time for all good men to come to the aid of their country.";
                string[] quotes = new string[] { "Never say never", "It's never too late", "Nevermore!" };

                Expect(phrase, Matches("all good men"));
                Expect(phrase, Matches("Now.*come"));
                //// Only available using new syntax
                Expect(phrase, Not.Matches("all.*men.*good"));
                Expect(phrase, Matches("ALL").IgnoreCase);
                Expect(quotes, All.Matches("never").IgnoreCase);
            }
        }
    }
}
