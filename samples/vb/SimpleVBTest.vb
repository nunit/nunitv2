'
' Copyright (C) 2002. James W. Newkirk, Michael C. Two, Alexei A. Vorontsov. All Rights Reserved.
'
Option Explicit On 
Imports System
Imports NUnit.Framework

Namespace NUnit.Samples

    <TestFixture()> Public Class SimpleVBTest

        Private fValue1 As Integer
        Private fValue2 As Integer

        Public Sub New()
            MyBase.New()
        End Sub

        <SetUp()> Public Sub Init()
            fValue1 = 2
            fValue2 = 3
        End Sub

        <Test()> Public Sub Add()
            Dim result As Double

            result = fValue1 + fValue2
            Assertion.AssertEquals(6, result)
        End Sub

        <Test()> Public Sub DivideByZero()
            Dim zero As Integer
            Dim result As Double

            zero = 0
            ' In VB7 Beta1, the below does not throw an exception. Result = 1.#INF after the below.
            ' All documentation seems to say it should throw an exception, so I am confused.
            result = 8 / zero
        End Sub

        <Test()> Public Sub TestEquals()
            Assertion.AssertEquals(12, 12)
            Assertion.AssertEquals(CLng(12), CLng(12))

            Assertion.AssertEquals("Size", 12, 13)
            Assertion.AssertEquals("Capacity", 12, 11.99, 0)
        End Sub
    End Class
End Namespace