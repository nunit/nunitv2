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

	using System;
	using System.Text;

	/// <summary>A simple Money.</summary>
	class Money: IMoney 
	{

		private int fAmount;
		private String fCurrency;
        
		/// <summary>Constructs a money from the given amount and
		/// currency.</summary>
		public Money(int amount, String currency) 
		{
			fAmount= amount;
			fCurrency= currency;
		}

		/// <summary>Adds a money to this money. Forwards the request to
		/// the AddMoney helper.</summary>
		public IMoney Add(IMoney m) 
		{
			return m.AddMoney(this);
		}

		public IMoney AddMoney(Money m) 
		{
			if (m.Currency.Equals(Currency) )
				return new Money(Amount+m.Amount, Currency);
			return new MoneyBag(this, m);
		}

		public IMoney AddMoneyBag(MoneyBag s) 
		{
			return s.AddMoney(this);
		}

		public int Amount 
		{
			get { return fAmount; }
		}

		public String Currency 
		{
			get { return fCurrency; }
		}

		public override bool Equals(Object anObject) 
		{
			if (IsZero)
				if (anObject is IMoney)
					return ((IMoney)anObject).IsZero;
			if (anObject is Money) 
			{
				Money aMoney= (Money)anObject;
				return aMoney.Currency.Equals(Currency)
					&& Amount == aMoney.Amount;
			}
			return false;
		}

		public override int GetHashCode() 
		{
			return fCurrency.GetHashCode()+fAmount;
		}

		public bool IsZero 
		{
			get { return Amount == 0; }
		}

		public IMoney Multiply(int factor) 
		{
			return new Money(Amount*factor, Currency);
		}

		public IMoney Negate() 
		{
			return new Money(-Amount, Currency);
		}

		public IMoney Subtract(IMoney m) 
		{
			return Add(m.Negate());
		}

		public override String ToString() 
		{
			StringBuilder buffer = new StringBuilder();
			buffer.Append("["+Amount+" "+Currency+"]");
			return buffer.ToString();
		}
	}
}
