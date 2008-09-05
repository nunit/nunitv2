using System;

namespace NUnit.Framework.Syntax
{
    public abstract class SyntaxTest
    {
        protected string parseTree;
        protected object staticSyntax;
        protected object inheritedSyntax;
        protected object builderSyntax;

        protected AssertionHelper Helper()
        {
            return new AssertionHelper();
        }

        protected Constraints.PartialConstraintExpression Builder()
        {
            return new Constraints.PartialConstraintExpression();
        }

        [Test]
        public void SupportedByStaticSyntax()
        {
            Assert.That(
                staticSyntax.ToString(),
                Is.EqualTo(parseTree).NoClip);
        }

        [Test]
        public void SupportedByConstraintBuilder()
        {
            Assert.That(
                builderSyntax.ToString(),
                Is.EqualTo(parseTree).NoClip);
        }

        [Test]
        public void SupportedByInheritedSyntax()
        {
            Assert.That(
                inheritedSyntax.ToString(),
                Is.EqualTo(parseTree).NoClip);
        }
    }
}
