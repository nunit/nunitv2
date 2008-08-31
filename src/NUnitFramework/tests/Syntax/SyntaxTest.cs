using System;

namespace NUnit.Framework.Syntax
{
    public abstract class StaticOnlySyntaxTest
    {
        protected string parseTree;
        protected object staticSyntax;

        [Test]
        public void SupportedByStaticSyntax()
        {
            Assert.That(
                staticSyntax.ToString(),
                Is.EqualTo(parseTree).NoClip);
        }
    }

    public abstract class SyntaxTest : StaticOnlySyntaxTest
    {
        protected object inheritedSyntax;
        protected object builderSyntax;

        protected AssertionHelper Helper()
        {
            return new AssertionHelper();
        }

        protected Constraints.ConstraintBuilder Builder()
        {
            return new Constraints.ConstraintBuilder();
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
