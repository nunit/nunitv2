// cppsample.h

#pragma once

#using <nunit.framework.dll>
using namespace System;
using namespace NUnit::Framework;

namespace NUnitSamples
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
