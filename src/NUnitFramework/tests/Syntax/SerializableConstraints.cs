namespace NUnit.Framework.Syntax
{
    [TestFixture]
    public class BinarySerializableTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<binaryserializable>";
            staticSyntax = Is.BinarySerializable;
            inheritedSyntax = Helper().BinarySerializable;
            builderSyntax = Builder().BinarySerializable;
        }
    }

    [TestFixture]
    public class XmlSerializableTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<xmlserializable>";
            staticSyntax = Is.XmlSerializable;
            inheritedSyntax = Helper().XmlSerializable;
            builderSyntax = Builder().XmlSerializable;
        }
    }
}
