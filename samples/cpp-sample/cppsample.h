// cppsample.h

#pragma once

#using <Nunit.Framework.dll>
using namespace System;
using namespace Nunit::Framework;

namespace NunitSamples
{
	[TestFixture]
	public __gc class SimpleCPPSample
	{
		int fValue1;
		int fValue2;
	public:
		[SetUp] void Init();
		[Test] void Add();
		[Test] void DivideByZero();
		[Test] void Equals();

	};
}
