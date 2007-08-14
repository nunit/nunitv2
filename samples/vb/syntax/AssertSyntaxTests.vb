' ****************************************************************
' Copyright 2007, Charlie Poole
' This is free software licensed under the NUnit license. You may
' obtain a copy of the license at http:'nunit.org/?p=license&r=2.4
' ****************************************************************

Option Explicit On 

Imports System
Imports NUnit.Framework
Imports NUnit.Framework.Constraints
Imports NUnit.Framework.SyntaxHelpers
Imports Tis = NUnit.Framework.SyntaxHelpers.Is
Imports Text = NUnit.Framework.SyntaxHelpers.Text

Namespace NUnit.Samples

    ' This test fixture attempts to exercise all the syntactic
    ' variations of Assert without getting into failures, errors 
    ' or corner cases. Thus, some of the tests may be duplicated 
    ' in other fixtures.
    ' 
    ' Each test performs the same operations using the classic
    ' syntax (if available) and the new syntax in both the
    ' helper-based and inherited forms.
    ' 
    ' This Fixture will eventually be duplicated in other
    ' supported languages. 

    <TestFixture()> _
    Public Class AssertSyntaxTests
        Inherits AssertionHelper

        <Test()> _
        Public Sub IsNull()
            ' Classic syntax
            Assert.IsNull(Nothing)

            ' Helper syntax
            Assert.That(Nothing, Tis.Null)

            ' Inherited syntax
            Expect(Nothing, Null)
        End Sub


        <Test()> _
        Public Sub IsNotNull()
            ' Classic syntax
            Assert.IsNotNull(42)

            ' Helper syntax
            Assert.That(42, Tis.Not.Null)

            ' Inherited syntax
            Expect(42, Tis.Not.Null)
        End Sub

        <Test()> _
        Public Sub IsTrue()
            ' Classic syntax
            Assert.IsTrue(2 + 2 = 4)

            ' Helper syntax
            Assert.That(2 + 2 = 4, Tis.True)
            Assert.That(2 + 2 = 4)

            ' Inherited syntax
            Expect(2 + 2 = 4, Tis.True)
            Expect(2 + 2 = 4)
        End Sub

        <Test()> _
        Public Sub IsFalse()
            ' Classic syntax
            Assert.IsFalse(2 + 2 = 5)

            ' Helper syntax
            Assert.That(2 + 2 = 5, Tis.False)

            ' Inherited syntax
            Expect(2 + 2 = 5, Tis.False)
        End Sub

        <Test()> _
        Public Sub IsNaN()
            Dim d As Double = Double.NaN
            Dim f As Single = Single.NaN

            ' Classic syntax
            Assert.IsNaN(d)
            Assert.IsNaN(f)

            ' Helper syntax
            Assert.That(d, Tis.NaN)
            Assert.That(f, Tis.NaN)

            ' Inherited syntax
            Expect(d, NaN)
            Expect(f, NaN)
        End Sub

        <Test()> _
        Public Sub EmptyStringTests()
            ' Classic syntax
            Assert.IsEmpty("")
            Assert.IsNotEmpty("Hello!")

            ' Helper syntax
            Assert.That("", Tis.Empty)
            Assert.That("Hello!", Tis.Not.Empty)

            ' Inherited syntax
            Expect("", Empty)
            Expect("Hello!", Tis.Not.Empty)
        End Sub

        <Test()> _
        Public Sub EmptyCollectionTests()

            Dim boolArray As Boolean() = New Boolean() {}
            Dim nonEmpty As Integer() = New Integer() {1, 2, 3}

            ' Classic syntax
            Assert.IsEmpty(boolArray)
            Assert.IsNotEmpty(nonEmpty)

            ' Helper syntax
            Assert.That(boolArray, Tis.Empty)
            Assert.That(nonEmpty, Tis.Not.Empty)

            ' Inherited syntax
            Expect(boolArray, Tis.Empty)
            Expect(nonEmpty, Tis.Not.Empty)
        End Sub

        <Test()> _
        Public Sub ExactTypeTests()
            ' Classic syntax workarounds
            Assert.AreEqual(GetType(String), "Hello".GetType())
            Assert.AreEqual("System.String", "Hello".GetType().FullName)
            Assert.AreNotEqual(GetType(Integer), "Hello".GetType())
            Assert.AreNotEqual("System.Int32", "Hello".GetType().FullName)

            ' Helper syntax
            Assert.That("Hello", Tis.TypeOf(GetType(String)))
            Assert.That("Hello", Tis.Not.TypeOf(GetType(Integer)))

            ' Inherited syntax
            Expect("Hello", Tis.TypeOf(GetType(String)))
            Expect("Hello", Tis.Not.TypeOf(GetType(Integer)))
        End Sub

        <Test()> _
        Public Sub InstanceOfTypeTests()
            ' Classic syntax
            Assert.IsInstanceOfType(GetType(String), "Hello")
            Assert.IsNotInstanceOfType(GetType(String), 5)

            ' Helper syntax
            Assert.That("Hello", Tis.InstanceOfType(GetType(String)))
            Assert.That(5, Tis.Not.InstanceOfType(GetType(String)))

            ' Inherited syntax
            Expect("Hello", InstanceOfType(GetType(String)))
            Expect(5, Tis.Not.InstanceOfType(GetType(String)))
        End Sub

        <Test()> _
        Public Sub AssignableFromTypeTests()
            ' Classic syntax
            Assert.IsAssignableFrom(GetType(String), "Hello")
            Assert.IsNotAssignableFrom(GetType(String), 5)

            ' Helper syntax
            Assert.That("Hello", Tis.AssignableFrom(GetType(String)))
            Assert.That(5, Tis.Not.AssignableFrom(GetType(String)))

            ' Inherited syntax
            Expect("Hello", AssignableFrom(GetType(String)))
            Expect(5, Tis.Not.AssignableFrom(GetType(String)))
        End Sub

        <Test()> _
        Public Sub SubstringTests()
            Dim phrase As String = "Hello World!"
            Dim array As String() = New String() {"abc", "bad", "dba"}

            ' Classic Syntax
            StringAssert.Contains("World", phrase)

            ' Helper syntax
            Assert.That(phrase, Text.Contains("World"))
            ' Only available using new syntax
            Assert.That(phrase, Text.DoesNotContain("goodbye"))
            Assert.That(phrase, Text.Contains("WORLD").IgnoreCase)
            Assert.That(phrase, Text.DoesNotContain("BYE").IgnoreCase)
            Assert.That(array, Text.All.Contains("b"))

            ' Inherited syntax
            Expect(phrase, Contains("World"))
            ' Only available using new syntax
            Expect(phrase, Text.DoesNotContain("goodbye"))
            Expect(phrase, Contains("WORLD").IgnoreCase)
            Expect(phrase, Text.DoesNotContain("BYE").IgnoreCase)
            Expect(array, All.Contains("b"))
        End Sub

        <Test()> _
        Public Sub StartsWithTests()
            Dim phrase As String = "Hello World!"
            Dim greetings As String() = New String() {"Hello!", "Hi!", "Hola!"}

            ' Classic syntax
            StringAssert.StartsWith("Hello", phrase)

            ' Helper syntax
            Assert.That(phrase, Text.StartsWith("Hello"))
            ' Only available using new syntax
            Assert.That(phrase, Text.DoesNotStartWith("Hi!"))
            Assert.That(phrase, Text.StartsWith("HeLLo").IgnoreCase)
            Assert.That(phrase, Text.DoesNotStartWith("HI").IgnoreCase)
            Assert.That(greetings, Text.All.StartsWith("h").IgnoreCase)

            ' Inherited syntax
            Expect(phrase, StartsWith("Hello"))
            ' Only available using new syntax
            Expect(phrase, Text.DoesNotStartWith("Hi!"))
            Expect(phrase, StartsWith("HeLLo").IgnoreCase)
            Expect(phrase, Text.DoesNotStartWith("HI").IgnoreCase)
            Expect(greetings, All.StartsWith("h").IgnoreCase)
        End Sub

        <Test()> _
        Public Sub EndsWithTests()
            Dim phrase As String = "Hello World!"
            Dim greetings As String() = New String() {"Hello!", "Hi!", "Hola!"}

            ' Classic Syntax
            StringAssert.EndsWith("!", phrase)

            ' Helper syntax
            Assert.That(phrase, Text.EndsWith("!"))
            ' Only available using new syntax
            Assert.That(phrase, Text.DoesNotEndWith("?"))
            Assert.That(phrase, Text.EndsWith("WORLD!").IgnoreCase)
            Assert.That(greetings, Text.All.EndsWith("!"))

            ' Inherited syntax
            Expect(phrase, EndsWith("!"))
            ' Only available using new syntax
            Expect(phrase, Text.DoesNotEndWith("?"))
            Expect(phrase, EndsWith("WORLD!").IgnoreCase)
            Expect(greetings, All.EndsWith("!"))
        End Sub

        <Test()> _
        Public Sub EqualIgnoringCaseTests()

            Dim phrase As String = "Hello World!"
            Dim array1 As String() = New String() {"Hello", "World"}
            Dim array2 As String() = New String() {"HELLO", "WORLD"}
            Dim array3 As String() = New String() {"HELLO", "Hello", "hello"}

            ' Classic syntax
            StringAssert.AreEqualIgnoringCase("hello world!", phrase)

            ' Helper syntax
            Assert.That(phrase, Tis.EqualTo("hello world!").IgnoreCase)
            'Only available using new syntax
            Assert.That(phrase, Tis.Not.EqualTo("goodbye world!").IgnoreCase)
            Assert.That(array1, Tis.EqualTo(array2).IgnoreCase)
            Assert.That(array3, Tis.All.EqualTo("hello").IgnoreCase)

            ' Inherited syntax
            Expect(phrase, EqualTo("hello world!").IgnoreCase)
            'Only available using new syntax
            Expect(phrase, Tis.Not.EqualTo("goodbye world!").IgnoreCase)
            Expect(array1, EqualTo(array2).IgnoreCase)
            Expect(array3, All.EqualTo("hello").IgnoreCase)
        End Sub

        <Test()> _
        Public Sub RegularExpressionTests()
            Dim phrase As String = "Now is the time for all good men to come to the aid of their country."
            Dim quotes As String() = New String() {"Never say never", "It's never too late", "Nevermore!"}

            ' Classic syntax
            StringAssert.IsMatch("all good men", phrase)
            StringAssert.IsMatch("Now.*come", phrase)

            ' Helper syntax
            Assert.That(phrase, Text.Matches("all good men"))
            Assert.That(phrase, Text.Matches("Now.*come"))
            ' Only available using new syntax
            Assert.That(phrase, Text.DoesNotMatch("all.*men.*good"))
            Assert.That(phrase, Text.Matches("ALL").IgnoreCase)
            Assert.That(quotes, Text.All.Matches("never").IgnoreCase)

            ' Inherited syntax
            Expect(phrase, Matches("all good men"))
            Expect(phrase, Matches("Now.*come"))
            ' Only available using new syntax
            Expect(phrase, Text.DoesNotMatch("all.*men.*good"))
            Expect(phrase, Matches("ALL").IgnoreCase)
            Expect(quotes, All.Matches("never").IgnoreCase)
        End Sub

        <Test()> _
        Public Sub EqualityTests()

            Dim i3 As Integer() = {1, 2, 3}
            Dim d3 As Double() = {1.0, 2.0, 3.0}
            Dim iunequal As Integer() = {1, 3, 2}

            ' Classic Syntax
            Assert.AreEqual(4, 2 + 2)
            Assert.AreEqual(i3, d3)
            Assert.AreNotEqual(5, 2 + 2)
            Assert.AreNotEqual(i3, iunequal)

            ' Helper syntax
            Assert.That(2 + 2, Tis.EqualTo(4))
            Assert.That(2 + 2 = 4)
            Assert.That(i3, Tis.EqualTo(d3))
            Assert.That(2 + 2, Tis.Not.EqualTo(5))
            Assert.That(i3, Tis.Not.EqualTo(iunequal))

            ' Inherited syntax
            Expect(2 + 2, EqualTo(4))
            Expect(2 + 2 = 4)
            Expect(i3, EqualTo(d3))
            Expect(2 + 2, Tis.Not.EqualTo(5))
            Expect(i3, Tis.Not.EqualTo(iunequal))
        End Sub

        <Test()> _
        Public Sub EqualityTestsWithTolerance()
            ' CLassic syntax
            Assert.AreEqual(5.0R, 4.99R, 0.05R)
            Assert.AreEqual(5.0F, 4.99F, 0.05F)

            ' Helper syntax
            Assert.That(4.99R, Tis.EqualTo(5.0R).Within(0.05R))
            Assert.That(4.99F, Tis.EqualTo(5.0F).Within(0.05F))

            ' Inherited syntax
            Expect(4.99R, EqualTo(5.0R).Within(0.05R))
            Expect(4.99F, EqualTo(5.0F).Within(0.05F))
        End Sub

        <Test()> _
        Public Sub ComparisonTests()
            ' Classic Syntax
            Assert.Greater(7, 3)
            Assert.GreaterOrEqual(7, 3)
            Assert.GreaterOrEqual(7, 7)

            ' Helper syntax
            Assert.That(7, Tis.GreaterThan(3))
            Assert.That(7, Tis.GreaterThanOrEqualTo(3))
            Assert.That(7, Tis.AtLeast(3))
            Assert.That(7, Tis.GreaterThanOrEqualTo(7))
            Assert.That(7, Tis.AtLeast(7))

            ' Inherited syntax
            Expect(7, GreaterThan(3))
            Expect(7, GreaterThanOrEqualTo(3))
            Expect(7, AtLeast(3))
            Expect(7, GreaterThanOrEqualTo(7))
            Expect(7, AtLeast(7))

            ' Classic syntax
            Assert.Less(3, 7)
            Assert.LessOrEqual(3, 7)
            Assert.LessOrEqual(3, 3)

            ' Helper syntax
            Assert.That(3, Tis.LessThan(7))
            Assert.That(3, Tis.LessThanOrEqualTo(7))
            Assert.That(3, Tis.AtMost(7))
            Assert.That(3, Tis.LessThanOrEqualTo(3))
            Assert.That(3, Tis.AtMost(3))

            ' Inherited syntax
            Expect(3, LessThan(7))
            Expect(3, LessThanOrEqualTo(7))
            Expect(3, AtMost(7))
            Expect(3, LessThanOrEqualTo(3))
            Expect(3, AtMost(3))
        End Sub

        <Test()> _
        Public Sub AllItemsTests()

            Dim ints As Object() = {1, 2, 3, 4}
            Dim strings As Object() = {"abc", "bad", "cab", "bad", "dad"}

            ' Classic syntax
            CollectionAssert.AllItemsAreNotNull(ints)
            CollectionAssert.AllItemsAreInstancesOfType(ints, GetType(Integer))
            CollectionAssert.AllItemsAreInstancesOfType(strings, GetType(String))
            CollectionAssert.AllItemsAreUnique(ints)

            ' Helper syntax
            Assert.That(ints, Tis.All.Not.Null)
            Assert.That(ints, Tis.All.InstanceOfType(GetType(Integer)))
            Assert.That(strings, Tis.All.InstanceOfType(GetType(String)))
            Assert.That(ints, Tis.Unique)
            ' Only available using new syntax
            Assert.That(strings, Tis.Not.Unique)
            Assert.That(ints, Tis.All.GreaterThan(0))
            Assert.That(strings, Text.All.Contains("a"))
            Assert.That(strings, Has.Some.StartsWith("ba"))

            ' Inherited syntax
            Expect(ints, All.Not.Null)
            Expect(ints, All.InstanceOfType(GetType(Integer)))
            Expect(strings, All.InstanceOfType(GetType(String)))
            Expect(ints, Unique)
            ' Only available using new syntax
            Expect(strings, Tis.Not.Unique)
            Expect(ints, All.GreaterThan(0))
            Expect(strings, All.Contains("a"))
            Expect(strings, Some.StartsWith("ba"))
        End Sub

        <Test()> _
       Public Sub SomeItemsTests()

            Dim mixed As Object() = {1, 2, "3", Nothing, "four", 100}
            Dim strings As Object() = {"abc", "bad", "cab", "bad", "dad"}

            ' Not available using the classic syntax

            ' Helper syntax
            Assert.That(mixed, Has.Some.Null)
            Assert.That(mixed, Has.Some.InstanceOfType(GetType(Integer)))
            Assert.That(mixed, Has.Some.InstanceOfType(GetType(String)))
            Assert.That(mixed, Has.Some.GreaterThan(99))
            Assert.That(strings, Has.Some.StartsWith("ba"))
            Assert.That(strings, Has.Some.Not.StartsWith("ba"))

            ' Inherited syntax
            Expect(mixed, Some.Null)
            Expect(mixed, Some.InstanceOfType(GetType(Integer)))
            Expect(mixed, Some.InstanceOfType(GetType(String)))
            Expect(mixed, Some.GreaterThan(99))
            Expect(strings, Some.StartsWith("ba"))
            Expect(strings, Some.Not.StartsWith("ba"))
        End Sub

        <Test()> _
        Public Sub NoItemsTests()

            Dim ints As Object() = {1, 2, 3, 4, 5}
            Dim strings As Object() = {"abc", "bad", "cab", "bad", "dad"}

            ' Not available using the classic syntax

            ' Helper syntax
            Assert.That(ints, Has.None.Null)
            Assert.That(ints, Has.None.InstanceOfType(GetType(String)))
            Assert.That(ints, Has.None.GreaterThan(99))
            Assert.That(strings, Has.None.StartsWith("qu"))

            ' Inherited syntax
            Expect(ints, None.Null)
            Expect(ints, None.InstanceOfType(GetType(String)))
            Expect(ints, None.GreaterThan(99))
            Expect(strings, None.StartsWith("qu"))
        End Sub

        <Test()> _
        Public Sub CollectionContainsTests()

            Dim iarray As Integer() = {1, 2, 3}
            Dim sarray As String() = {"a", "b", "c"}

            ' Classic syntax
            Assert.Contains(3, iarray)
            Assert.Contains("b", sarray)
            CollectionAssert.Contains(iarray, 3)
            CollectionAssert.Contains(sarray, "b")
            CollectionAssert.DoesNotContain(sarray, "x")

            ' Helper syntax
            Assert.That(iarray, Has.Member(3))
            Assert.That(sarray, Has.Member("b"))
            Assert.That(sarray, Has.No.Member("x"))

            ' Inherited syntax
            Expect(iarray, Contains(3))
            Expect(sarray, Contains("b"))
            Expect(sarray, Has.No.Member("x"))
        End Sub

        <Test()> _
        Public Sub CollectionEquivalenceTests()

            Dim ints1to5 As Integer() = {1, 2, 3, 4, 5}

            ' Classic syntax
            CollectionAssert.AreEquivalent(New Integer() {2, 1, 4, 3, 5}, ints1to5)
            CollectionAssert.AreNotEquivalent(New Integer() {2, 2, 4, 3, 5}, ints1to5)
            CollectionAssert.AreNotEquivalent(New Integer() {2, 4, 3, 5}, ints1to5)
            CollectionAssert.AreEquivalent(New Integer() {2, 2, 1, 1, 4, 3, 5}, ints1to5)

            ' Helper syntax
            Assert.That(New Integer() {2, 1, 4, 3, 5}, Tis.EquivalentTo(ints1to5))
            Assert.That(New Integer() {2, 2, 4, 3, 5}, Tis.Not.EquivalentTo(ints1to5))
            Assert.That(New Integer() {2, 4, 3, 5}, Tis.Not.EquivalentTo(ints1to5))
            Assert.That(New Integer() {2, 2, 1, 1, 4, 3, 5}, Tis.EquivalentTo(ints1to5))

            ' Inherited syntax
            Expect(New Integer() {2, 1, 4, 3, 5}, EquivalentTo(ints1to5))
            Expect(New Integer() {2, 2, 4, 3, 5}, Tis.Not.EquivalentTo(ints1to5))
            Expect(New Integer() {2, 4, 3, 5}, Tis.Not.EquivalentTo(ints1to5))
            Expect(New Integer() {2, 2, 1, 1, 4, 3, 5}, EquivalentTo(ints1to5))
        End Sub

        <Test()> _
        Public Sub SubsetTests()

            Dim ints1to5 As Integer() = {1, 2, 3, 4, 5}

            ' Classic syntax
            CollectionAssert.IsSubsetOf(New Integer() {1, 3, 5}, ints1to5)
            CollectionAssert.IsSubsetOf(New Integer() {1, 2, 3, 4, 5}, ints1to5)
            CollectionAssert.IsNotSubsetOf(New Integer() {2, 4, 6}, ints1to5)

            ' Helper syntax
            Assert.That(New Integer() {1, 3, 5}, Tis.SubsetOf(ints1to5))
            Assert.That(New Integer() {1, 2, 3, 4, 5}, Tis.SubsetOf(ints1to5))
            Assert.That(New Integer() {2, 4, 6}, Tis.Not.SubsetOf(ints1to5))

            ' Inherited syntax
            Expect(New Integer() {1, 3, 5}, SubsetOf(ints1to5))
            Expect(New Integer() {1, 2, 3, 4, 5}, SubsetOf(ints1to5))
            Expect(New Integer() {2, 4, 6}, Tis.Not.SubsetOf(ints1to5))
        End Sub

        <Test()> _
        Public Sub PropertyTests()

            Dim array As String() = {"abc", "bca", "xyz"}

            ' Helper syntax
            Assert.That("Hello", Has.Property("Length", 5))
            Assert.That("Hello", Has.Length(5))
            Assert.That(array, Has.All.Property("Length", 3))
            Assert.That(array, Has.All.Length(3))

            ' Inherited syntax
            Expect("Hello", Has.Property("Length", 5))
            Expect("Hello", Length(5))
            Expect(array, All.Property("Length", 3))
            Expect(array, All.Length(3))
        End Sub

        <Test()> _
        Public Sub NotTests()
            ' Not available using the classic syntax

            ' Helper syntax
            Assert.That(42, Tis.Not.Null)
            Assert.That(42, Tis.Not.True)
            Assert.That(42, Tis.Not.False)
            Assert.That(2.5, Tis.Not.NaN)
            Assert.That(2 + 2, Tis.Not.EqualTo(3))
            Assert.That(2 + 2, Tis.Not.Not.EqualTo(4))
            Assert.That(2 + 2, Tis.Not.Not.Not.EqualTo(5))

            ' Inherited syntax
            Expect(42, Tis.Not.Null)
            Expect(42, Tis.Not.True)
            Expect(42, Tis.Not.False)
            Expect(2.5, Tis.Not.NaN)
            Expect(2 + 2, Tis.Not.EqualTo(3))
            Expect(2 + 2, Tis.Not.Not.EqualTo(4))
            Expect(2 + 2, Tis.Not.Not.Not.EqualTo(5))
        End Sub

    End Class

End Namespace

