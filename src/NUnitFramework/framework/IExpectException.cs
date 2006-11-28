// *****************************************************
// Copyright 2006, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;

namespace NUnit.Framework
{
    public interface IExpectException
    {
        void HandleException(Exception ex);
    }
}
