namespace TypeReferences.Editor.Tests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    public class EqualityTests
    {
        private class FirstClass { }

        private class SecondClass { }

        private class ThirdClass { }

        [Test]
        public void Type_references_with_same_types_are_equal()
        {
            var firstTypeRef = new TypeReference(typeof(FirstClass));
            var secondTypeRef = new TypeReference(typeof(FirstClass));

            Assert.AreEqual(firstTypeRef, secondTypeRef);
        }

        [Test]
        public void When_type_references_have_same_guids_but_one_type_is_null_they_are_equal()
        {
            var firstTypeRef = new TypeReference(typeof(FirstClass), "testGUID");
            var secondTypeRef = new TypeReference { GUID = "testGUID" };

            Assert.AreEqual(firstTypeRef, secondTypeRef);
        }

        [Test]
        public void Sets_with_same_type_references_are_equal()
        {
            var firstTypeRef = new TypeReference(typeof(FirstClass));
            var secondTypeRef = new TypeReference(typeof(SecondClass));

            var firstSet = new HashSet<TypeReference> { firstTypeRef, secondTypeRef };
            var secondSet = new HashSet<TypeReference> { secondTypeRef, firstTypeRef };

            Assert.IsTrue(firstSet.SetEquals(secondSet));
        }

        [Test]
        public void Sets_with_different_type_references_are_not_equal()
        {
            var firstTypeRef = new TypeReference(typeof(FirstClass));
            var secondTypeRef = new TypeReference(typeof(SecondClass));
            var thirdTypeRef = new TypeReference(typeof(ThirdClass));

            var firstSet = new HashSet<TypeReference> { firstTypeRef, secondTypeRef };
            var secondSet = new HashSet<TypeReference> { firstTypeRef, thirdTypeRef };

            Assert.IsFalse(firstSet.SetEquals(secondSet));
        }

        [Test]
        public void TypeReference_with_missing_type_but_existing_GUID_can_be_found_in_HashSet_later()
        {
            var missingTypeRef = new TypeReference((Type) null, "testGUID");

            var testSet = new HashSet<TypeReference> { missingTypeRef };

            void ReAddItem(TypeReference typeRef)
            {
                var previousTypeRef = new TypeReference( (Type)null, typeRef.GUID);

                if (!testSet.Contains(previousTypeRef))
                    return;

                testSet.Remove(previousTypeRef);
                testSet.Add(typeRef);
            }

            TypeReference.TypeRestoredFromGUID += ReAddItem;

            var existingTypeRef = new TypeReference(typeof(FirstClass));

            missingTypeRef.Type = typeof(FirstClass);
            ReAddItem(missingTypeRef);

            Assert.IsTrue(testSet.Contains(existingTypeRef));
        }
    }
}