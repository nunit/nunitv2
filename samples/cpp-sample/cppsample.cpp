#include "stdafx.h"

#include "cppsample.h"

namespace NUnitSamples {

	void SimpleCPPSample::Init() {
		fValue1 = 2;
		fValue2 = 3;
	}

	void SimpleCPPSample::Add() {
		int result = fValue1 + fValue2;
		Assertion::AssertEquals(6,result);
	}

	void SimpleCPPSample::DivideByZero()
	{
		int zero= 0;
		int result= 8/zero;
	}

	void SimpleCPPSample::Equals() {
		Assertion::AssertEquals("Integer.",12, 12);
		Assertion::AssertEquals("Long.",12L, 12L);
		Assertion::AssertEquals("Char.",'a', 'a');


		Assertion::AssertEquals("Expected Failure (Integer).", 12, 13);
		Assertion::AssertEquals("Expected Failure (Double).", 12.0, 11.99, 0.0);
	}

}

