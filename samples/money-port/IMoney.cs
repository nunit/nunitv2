/********************************************************************************************************************
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
'*******************************************************************************************************************/
namespace NUnit.Samples.Money 
{

  /// <summary>The common interface for simple Monies and MoneyBags.</summary>
  interface IMoney {

    /// <summary>Adds a money to this money.</summary>
    IMoney Add(IMoney m);

    /// <summary>Adds a simple Money to this money. This is a helper method for
    /// implementing double dispatch.</summary>
    IMoney AddMoney(Money m);

    /// <summary>Adds a MoneyBag to this money. This is a helper method for
    /// implementing double dispatch.</summary>
    IMoney AddMoneyBag(MoneyBag s);

    /// <value>True if this money is zero.</value>
    bool IsZero { get; }

    /// <summary>Multiplies a money by the given factor.</summary>
    IMoney Multiply(int factor);

    /// <summary>Negates this money.</summary>
    IMoney Negate();

    /// <summary>Subtracts a money from this money.</summary>
    IMoney Subtract(IMoney m);
  }
}
