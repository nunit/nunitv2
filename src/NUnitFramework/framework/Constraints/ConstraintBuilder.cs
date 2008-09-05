// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org/?p=license&r=2.4
// ****************************************************************

using System;
using System.Collections;
#if NET_2_0
using System.Collections.Generic;
#endif

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// ConstraintBuilder maintains the stacks that are used in
    /// processing a ConstraintExpression. An OperatorStack
    /// is used to hold operators that are waiting for their
    /// operands to be reognized. a ConstraintStack holds 
    /// input constraints as well as the results of each
    /// operator applied.
    /// </summary>
    public class ConstraintBuilder
    {
        #region Nested Operator Stack Class
        public class OperatorStack
        {
#if NET_2_0
            private Stack<ConstraintOperator> stack = new Stack<ConstraintOperator>();
#else
		private Stack stack = new Stack();
#endif
            public OperatorStack()
            {
                stack.Clear();
            }

            /// <summary>
            /// Gets a value indicating whether this <see cref="T:OpStack"/> is empty.
            /// </summary>
            /// <value><c>true</c> if empty; otherwise, <c>false</c>.</value>
            public bool Empty
            {
                get { return stack.Count == 0; }
            }

            /// <summary>
            /// Gets the topmost operator without modifying the stack.
            /// </summary>
            /// <value>The top.</value>
            public ConstraintOperator Top
            {
                get { return (ConstraintOperator)stack.Peek(); }
            }

            /// <summary>
            /// Pushes the specified operator onto the stack.
            /// </summary>
            /// <param name="op">The op.</param>
            public void Push(ConstraintOperator op)
            {
                stack.Push(op);
            }

            /// <summary>
            /// Pops the topmost operator from the stack.
            /// </summary>
            /// <returns></returns>
            public ConstraintOperator Pop()
            {
                return (ConstraintOperator)stack.Pop();
            }
        }
        #endregion

        #region Nested Constraint Stack Class
        public class ConstraintStack
        {
#if NET_2_0
            private Stack<Constraint> stack = new Stack<Constraint>();
#else
		private Stack stack = new Stack();
#endif
            public ConstraintStack()
            {
                stack.Clear();
            }

            /// <summary>
            /// Gets a value indicating whether this <see cref="T:ConstraintStack"/> is empty.
            /// </summary>
            /// <value><c>true</c> if empty; otherwise, <c>false</c>.</value>
            public bool Empty
            {
                get { return stack.Count == 0; }
            }

            /// <summary>
            /// Gets the topmost constraint without modifying the stac.
            /// </summary>
            /// <value>The topmost constraint</value>
            public Constraint Top
            {
                get { return (Constraint)stack.Peek(); }
            }

            /// <summary>
            /// Pushes the specified constraint.
            /// </summary>
            /// <param name="constraint">The constraint.</param>
            public void Push(Constraint constraint)
            {
                stack.Push(constraint);
            }

            public Constraint Pop()
            {
                return (Constraint)stack.Pop();
            }
        }
        #endregion

        #region Instance Fields
        protected OperatorStack ops = new OperatorStack();

        protected ConstraintStack constraints = new ConstraintStack();

        private object lastPushed;
        #endregion

        #region Properties
        public bool IsResolvable
        {
            get { return lastPushed is Constraint || lastPushed is PropOperator; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Appends the specified operator to the expression by first
        /// reducing the operator stack and then pushing the new
        /// operator on the stack.
        /// </summary>
        /// <param name="op">The operator to push.</param>
        public void Append(ConstraintOperator op)
        {
            op.LeftContext = lastPushed;
            if (lastPushed is ConstraintOperator)
                SetTopOperatorRightContext(op);

            // Reduce any lower precedence operators
            ReduceOperatorStack(op.LeftPrecedence);
            
            ops.Push(op);
            lastPushed = op;
        }

        /// <summary>
        /// Appends the specified constraint to the expresson by pushing
        /// it on the constraint stack.
        /// </summary>
        /// <param name="constraint">The constraint to push.</param>
        public void Append(Constraint constraint)
        {
            if (lastPushed is ConstraintOperator)
                SetTopOperatorRightContext(constraint);

            constraints.Push(constraint);
            lastPushed = constraint;
        }

        private void SetTopOperatorRightContext(object rightContext)
        {
            // Some operators change their precedence based on
            // the right context - save current precedence.
            int oldPrecedence = ops.Top.LeftPrecedence;

            ops.Top.RightContext = rightContext;

            // If the precedence increased, we may be able to
            // reduce the region of the stack below the operator
            if (ops.Top.LeftPrecedence > oldPrecedence)
            {
                ConstraintOperator changedOp = ops.Pop();
                ReduceOperatorStack(changedOp.LeftPrecedence);
                ops.Push(changedOp);
            }
        }

        private void ReduceOperatorStack(int targetPrecedence)
        {
            while (!ops.Empty && ops.Top.RightPrecedence < targetPrecedence)
                ops.Pop().Reduce(constraints);
        }

        public Constraint Resolve()
        {
            if (!IsResolvable)
                throw new InvalidOperationException("A partial expression may not be resolved");

            while (!ops.Empty)
            {
                ConstraintOperator op = ops.Pop();
                op.Reduce(constraints);
            }

            return constraints.Pop();
        }
        #endregion
    }
}
