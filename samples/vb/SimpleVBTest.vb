'********************************************************************************************************************
'
' Copyright (c) 2002, James Newkirk, Michael C. Two, Alexei Vorontsov, Philip Craig
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
' THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
'
'*******************************************************************************************************************
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