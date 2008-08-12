using System;
using System.Collections;

namespace NUnit.Framework.Syntax
{
    namespace Helpers
    {
        public class PropertyTests
        {
            [Test]
            public void SimplePropertyTests()
            {
                string[] array = { "abc", "bca", "xyz", "qrs" };
                string[] array2 = { "a", "ab", "abc" };
                ArrayList list = new ArrayList(array);

                Assert.That(list, Has.Property("Count"));
                Assert.That(list, Has.No.Property("Length"));

                Assert.That("Hello", Has.Property("Length", 5));
                Assert.That("Hello", Has.Length(5));
                Assert.That("Hello", Has.Property("Length").EqualTo(5));
                Assert.That("Hello", Has.Property("Length").GreaterThan(3));

                Assert.That(array, Has.Property("Length", 4));
                Assert.That(array, Has.Length(4));
                Assert.That(array, Has.Property("Length").LessThan(10));

                Assert.That(array, Has.All.Property("Length", 3));
                Assert.That(array, Has.All.Length(3));
                Assert.That(array, Is.All.Length(3));
                Assert.That(array, Has.All.Property("Length").EqualTo(3));
                Assert.That(array, Is.All.Property("Length").EqualTo(3));
                Assert.That(array, Has.None.Property("Length").Not.EqualTo(3));

                Assert.That(array2, Has.Some.Property("Length", 2));
                Assert.That(array2, Has.Some.Length(2));
                Assert.That(array2, Has.Some.Property("Length").GreaterThan(2));
                Assert.That(array2, Has.No.Property("Length").GreaterThan(3));

                Assert.That(array2, Is.Not.Property("Length", 4));
                Assert.That(array2, Is.Not.Length(4));
                Assert.That(array2, Has.No.Property("Length").GreaterThan(3));

                Assert.That(List.Map(array2).Property("Length"), Is.EqualTo(new int[] { 1, 2, 3 }));
                Assert.That(List.Map(array2).Property("Length"), Is.EquivalentTo(new int[] { 3, 2, 1 }));
                Assert.That(List.Map(array2).Property("Length"), Is.SubsetOf(new int[] { 1, 2, 3, 4, 5 }));
                Assert.That(List.Map(array2).Property("Length"), Is.Unique);
                Assert.That(List.Map(array2).Property("Length"), Is.Ordered());

                Assert.That(list, Has.Count(4));
            }

            [Test, ExpectedException(typeof(AssertionException))]
            public void PropertyTests_DoesNotExistFails()
            {
                object[] array = new object[] { 1, "two", 3, null };
                Assert.That(array, Has.None.Property("Length"));
            }
        }
    }

    namespace Inherited
    {
        public class PropertyTests : AssertionHelper
        {
            [Test]
            public void SimplePropertyTests()
            {
                string[] array = { "abc", "bca", "xyz", "qrs" };
                string[] array2 = { "a", "ab", "abc" };
                ArrayList list = new ArrayList(array);

                Expect(list, Property("Count"));
                Expect(list, Not.Property("Nada"));

                Expect("Hello", Property("Length", 5));
                Expect("Hello", Length(5));
                Expect("Hello", Property("Length").EqualTo(5));
                Expect("Hello", Property("Length").GreaterThan(0));

                Expect(array, Property("Length", 4));
                Expect(array, Length(4));
                Expect(array, Property("Length").LessThan(10));

                Expect(array, All.Property("Length", 3));
                Expect(array, All.Length(3));
                Expect(array, All.Property("Length").EqualTo(3));

                Expect(array2, Some.Property("Length", 2));
                Expect(array2, Some.Length(2));
                Expect(array2, Some.Property("Length").GreaterThan(2));

                Expect(array2, None.Property("Length", 4));
                Expect(array2, None.Length(4));
                Expect(array2, None.Property("Length").GreaterThan(3));

                Expect(Map(array2).Property("Length"), EqualTo(new int[] { 1, 2, 3 }));
                Expect(Map(array2).Property("Length"), EquivalentTo(new int[] { 3, 2, 1 }));
                Expect(Map(array2).Property("Length"), SubsetOf(new int[] { 1, 2, 3, 4, 5 }));
                Expect(Map(array2).Property("Length"), Unique);
                Expect(Map(array2).Property("Length"), Ordered());

                Expect(list, Count(4));
            }

            [Test, ExpectedException(typeof(AssertionException))]
            public void PropertyTests_DoesNotExistFails()
            {
                object[] array = new object[] { 1, "two", 3, null };
                Expect(array, None.Property("Length"));
            }
        }
    }
}
