namespace TypeReferences.Editor.Tests
{
    using NUnit.Framework;

    internal class InheritsAttributeTests
    {
        private interface IParentInterface { }

        private interface IChildInterface : IParentInterface { }

        private abstract class ParentAbstractClass : IParentInterface { }

        private class ChildClass : ParentAbstractClass { }

        private readonly struct ChildStruct : IParentInterface { }

        [Test]
        public void When_interface_is_base_type_classes_that_implement_interface_match_requirements()
        {
            var attribute = new InheritsAttribute(typeof(IParentInterface));
            Assert.That(attribute.MatchesRequirements(typeof(ChildClass)), Is.True);
        }

        [Test]
        public void When_interface_is_base_type_derived_interfaces_do_not_match_requirements()
        {
            var attribute = new InheritsAttribute(typeof(IParentInterface));
            Assert.That(attribute.MatchesRequirements(typeof(IChildInterface)), Is.False);
        }

        [Test]
        public void When_interface_is_base_type_and_AllowAbstract_is_true_derived_interfaces_match_requirements()
        {
            var attribute = new InheritsAttribute(typeof(IParentInterface)) { IncludeBaseType = true };
            Assert.That(attribute.MatchesRequirements(typeof(IChildInterface)), Is.False);
        }

        [Test]
        public void When_interface_is_base_type_a_struct_that_implements_it_matches_requirements()
        {
            var attribute = new InheritsAttribute(typeof(IParentInterface));
            Assert.That(attribute.MatchesRequirements(typeof(ChildStruct)), Is.True);
        }

        [Test]
        public void Base_type_does_not_match_requirements_by_default()
        {
            var attribute = new InheritsAttribute(typeof(IParentInterface));
            Assert.That(attribute.MatchesRequirements(typeof(IParentInterface)), Is.False);
        }

        [Test]
        public void When_IncludeBaseType_is_true_base_type_matches_requirements()
        {
            var attribute = new InheritsAttribute(typeof(IParentInterface)) { IncludeBaseType = true };
            Assert.That(attribute.MatchesRequirements(typeof(IParentInterface)), Is.True);
        }

        [Test]
        public void Abstract_classes_are_not_included_by_default()
        {
            var attribute = new InheritsAttribute(typeof(IParentInterface));
            Assert.That(attribute.MatchesRequirements(typeof(ParentAbstractClass)), Is.False);
        }

        [Test]
        public void When_abstract_class_is_base_type_and_IncludeBaseType_is_true_it_matches_requirements()
        {
            var attribute = new InheritsAttribute(typeof(ParentAbstractClass)) { IncludeBaseType = true };
            Assert.That(attribute.MatchesRequirements(typeof(ParentAbstractClass)), Is.True);
        }
    }
}