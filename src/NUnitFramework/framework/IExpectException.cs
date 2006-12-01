// *****************************************************
// Copyright 2006, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;

namespace NUnit.Framework
{
    /// <summary>
    /// Interface implemented by a user fixture in order to
    /// validate any expected exceptions. It is only called
    /// for test methods marked with the ExpectedException
    /// attribute.
    /// </summary>
	public interface IExpectException
    {
        void HandleException(Exception ex);
    }
}
