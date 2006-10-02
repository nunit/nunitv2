'************************************************************************************
'
' Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov
' Copyright © 2000-2002 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2002 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov 
' or Copyright © 2000-2002 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/

Option Explicit On 

Namespace NUnit.Samples

    ' A Simple Money.
    Public Class Money
        Implements IMoney

        Private fAmount As Int32
        Private fCurrency As String

        ' Constructs a money from a given amount and currency.
        Public Sub New(ByVal amount As Int32, ByVal currency As String)
            Me.fAmount = amount
            Me.fCurrency = currency
        End Sub


        ' Adds a money to this money. Forwards the request
        ' to the AddMoney helper.
        Public Overloads Function Add(ByVal m As IMoney) As IMoney Implements IMoney.Add
            Return m.AddMoney(Me)
        End Function

        Public Overloads Function AddMoney(ByVal m As Money) As IMoney Implements IMoney.AddMoney
            If m.Currency.Equals(Currency) Then
                Return New Money(Amount + m.Amount, Currency)
            End If

            Return New MoneyBag(Me, m)
        End Function

        Public Function AddMoneyBag(ByVal s As MoneyBag) As IMoney Implements IMoney.AddMoneyBag
            Return s.AddMoney(Me)
        End Function

        Public ReadOnly Property Amount()
            Get
                Return fAmount
            End Get
        End Property

        Public ReadOnly Property Currency()
            Get
                Return fCurrency
            End Get
        End Property

        Public Overloads Overrides Function Equals(ByVal anObject As Object) As Boolean
            If IsZero And TypeOf anObject Is IMoney Then
                Dim aMoney As IMoney = anObject
                Return aMoney.IsZero
            End If

            If TypeOf anObject Is Money Then
                Dim aMoney As Money = anObject
                If (IsZero) Then
                    Return aMoney.IsZero
                End If

                Return Currency.Equals(aMoney.Currency) And Amount.Equals(aMoney.Amount)
            End If

            Return False
        End Function

        Public Overrides Function GetHashCode() As Int32
            Return fCurrency.GetHashCode() + fAmount
        End Function

        Public ReadOnly Property IsZero() As Boolean Implements IMoney.IsZero
            Get
                Return Amount.Equals(0)
            End Get
        End Property

        Public Function Multiply(ByVal factor As Integer) As IMoney Implements IMoney.Multiply

            Return New Money(Amount * factor, Currency)

        End Function

        Public Function Negate() As IMoney Implements IMoney.Negate

            Return New Money(-Amount, Currency)

        End Function

        Public Function Subtract(ByVal m As IMoney) As IMoney Implements IMoney.Subtract

            Return Add(m.Negate())

        End Function

        Public Overrides Function ToString() As String

            Return String.Format("[{0} {1}]", Amount, Currency)

        End Function

    End Class

End Namespace
