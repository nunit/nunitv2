// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

using System;

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// ExceptionTypeConstraint is a special version of ExactTypeConstraint
    /// used to provided detailed info about the exception thrown in
    /// an error message.
    /// </summary>
    public class ExceptionTypeConstraint : ExactTypeConstraint
    {
        /// <summary>
        /// Constructs an ExceptionTypeConstraint
        /// </summary>
        public ExceptionTypeConstraint(Type type) : base(type) { }

        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter. Overriden to write additional information 
        /// in the case of an Exception.
        /// </summary>
        /// <param name="writer">The MessageWriter to use</param>
        public override void WriteActualValueTo(MessageWriter writer)
        {
            Exception ex = actual as Exception;
            base.WriteActualValueTo(writer);

            if (ex != null)
            {
                writer.WriteLine(" ({0})", ex.Message);
                writer.Write(ex.StackTrace);
            }
        }
    }
}
