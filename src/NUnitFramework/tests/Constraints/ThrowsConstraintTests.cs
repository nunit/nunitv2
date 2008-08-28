using System;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class ThrowsConstraintTests
    {
        Constraint throwsConstraint1 = new ThrowsConstraint(
            new ExactTypeConstraint(typeof(ArgumentException)));

        Constraint throwsConstraint2 = new ThrowsConstraint(
            new AndConstraint(
                new ExactTypeConstraint(typeof(ArgumentException)), 
                new PropertyConstraint("ParamName", new EqualConstraint("x") ) ) );

        [Test]
        public void ProvidesProperDescription()
        {
            MessageWriter writer = new TextMessageWriter();
            throwsConstraint1.WriteDescriptionTo(writer);
            Assert.AreEqual("<System.ArgumentException>", writer.ToString());
        }

        [Test]
        public void ProvidesProperDescription_WithConstraint()
        {
            MessageWriter writer = new TextMessageWriter();
            throwsConstraint2.WriteDescriptionTo(writer);
            Assert.AreEqual(
                @"<System.ArgumentException> and property ParamName equal to ""x""",
                writer.ToString());
        }

        [Test]
        public void ProvidesProperStringRepresentation()
        {
            Assert.AreEqual(
                "<throws <typeof System.ArgumentException>>", 
                throwsConstraint1.ToString());
        }

        [Test]
        public void ProvidesProperStringRepresentation_WithConstraint()
        {
            Assert.AreEqual(
                @"<throws <and <typeof System.ArgumentException> <property ParamName <equal ""x"">>>>", 
                throwsConstraint2.ToString());
        }
    }
}
