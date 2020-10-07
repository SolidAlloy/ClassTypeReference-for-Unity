namespace TypeReferences.Editor.Tests
{
    using NUnit.Framework;

    internal class TypeOptionsAttributeTests
    {
        private TypeOptionsAttribute _attribute;
        private interface IExampleInterface { }
        private struct StructExample { }
        private abstract class AbstractClass { }
        private class NormalClass { }

        [SetUp]
        public void BeforeEveryTest()
        {
            _attribute = new TypeOptionsAttribute();
        }

        [Test]
        public void With_default_options_all_types_match_requirements()
        {
            Assert.That(_attribute.MatchesRequirements(typeof(IExampleInterface)), Is.True);
            Assert.That(_attribute.MatchesRequirements(typeof(StructExample)), Is.True);
            Assert.That(_attribute.MatchesRequirements(typeof(AbstractClass)), Is.True);
            Assert.That(_attribute.MatchesRequirements(typeof(NormalClass)), Is.True);
        }

        [Test]
        public void When_a_type_is_in_ExcludeTypes_it_does_not_match_requirements()
        {
            _attribute.ExcludeTypes = new[] { typeof(IExampleInterface), typeof(StructExample) };

            Assert.That(_attribute.MatchesRequirements(typeof(IExampleInterface)), Is.False);
            Assert.That(_attribute.MatchesRequirements(typeof(StructExample)), Is.False);
            Assert.That(_attribute.MatchesRequirements(typeof(AbstractClass)), Is.True);
            Assert.That(_attribute.MatchesRequirements(typeof(NormalClass)), Is.True);
        }
    }
}