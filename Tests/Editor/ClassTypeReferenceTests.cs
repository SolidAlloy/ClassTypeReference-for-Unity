namespace TypeReferences.Tests.Editor
{
    using System;
    using NUnit.Framework;

    internal class ClassTypeReferenceTests
    {
        private struct NotAClass { }
        private class ClassExample { }

        internal class TheConstructor
        {
            [Test]
            public void When_no_arguments_are_passed_creates_instance_with_null_type()
            {
                var typeRef = new ClassTypeReference();
                Assert.That(typeRef.Type, Is.Null);
            }

            [Test]
            public void When_null_type_is_passed_creates_instance_with_null_type()
            {
                var typeRef = new ClassTypeReference( (Type)null );
                Assert.That(typeRef.Type, Is.Null);
            }

            [Test]
            public void When_not_a_class_type_is_passed_throws_ArgumentException()
            {
                var notAClassType = typeof(NotAClass);

                Assert.Throws<ArgumentException>(() =>
                {
                    var typeRef = new ClassTypeReference(notAClassType);
                });
            }

            [Test]
            public void When_a_class_type_is_passed_creates_instance_with_this_type()
            {
                var classType = typeof(ClassExample);
                var typeRef = new ClassTypeReference(classType);
                Assert.That(typeRef.Type, Is.EqualTo(classType));
            }

            [Test]
            public void When_null_string_is_passed_creates_instance_with_null_type()
            {
                var typeRef = new ClassTypeReference( (string)null );
                Assert.That(typeRef.Type, Is.Null);
            }

            [Test]
            public void When_empty_string_is_passed_creates_istance_with_null_type()
            {
                var typeRef = new ClassTypeReference(string.Empty);
                Assert.That(typeRef.Type, Is.Null);
            }

            [Test]
            public void When_not_assembly_qualified_name_string_is_passed_type_will_be_null()
            {
                string typeName = "wrongTypeName";
                var typeRef = new ClassTypeReference(typeName);
                Assert.That(typeRef.Type, Is.Null);
            }

            [Test]
            public void When_assembly_qualified_struct_name_string_is_passed_throws_ArgumentException()
            {
                Type classType = typeof(NotAClass);
                string typeName = classType.AssemblyQualifiedName;

                Assert.Throws<ArgumentException>(() =>
                {
                    var typeRef = new ClassTypeReference(typeName);
                });
            }

            [Test]
            public void When_assembly_qualified_class_name_string_is_passed_creates_instance_with_this_type()
            {
                Type classType = typeof(ClassExample);
                string typeName = classType.AssemblyQualifiedName;
                var typeRef = new ClassTypeReference(typeName);
                Assert.That(typeRef.Type, Is.EqualTo(classType));
            }
        }

        internal class ImplicitTypeConversion
        {
            [Test]
            public void Type_can_be_converted_to_ClassTypeReference()
            {
                Type classType = typeof(ClassExample);
                ClassTypeReference typeRef = classType;
                Assert.That(typeRef.Type, Is.EqualTo(classType));
            }

            [Test]
            public void ClassTypeReference_can_be_converted_to_Type()
            {
                Type initialType = typeof(ClassExample);
                var typeRef = new ClassTypeReference(initialType);
                Type convertedType = typeRef;
                Assert.That(convertedType, Is.EqualTo(initialType));
            }
        }

        internal class TheTypeProperty
        {
            private ClassTypeReference _typeRef;

            [SetUp]
            public void BeforeEveryTest()
            {
                _typeRef = new ClassTypeReference();
            }

            [Test]
            public void When_not_a_class_type_is_set_ArgumentException_is_thrown()
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    _typeRef.Type = typeof(NotAClass);
                });
            }

            [Test]
            public void When_class_type_is_set_returns_the_set_type()
            {
                Type classType = typeof(ClassExample);
                _typeRef.Type = classType;
                Assert.That(_typeRef.Type, Is.EqualTo(classType));
            }
        }

        internal class TheGetClassRefMethod
        {
            [Test]
            public void When_null_is_passed_returns_empty_string()
            {
                string classRef = ClassTypeReference.GetClassRef(null);
                Assert.That(classRef, Is.EqualTo(string.Empty));
            }

            [Test]
            public void When_type_is_passed_returns_string_that_contains_full_type_name_and_assembly_name()
            {
                Type exampleType = typeof(ClassExample);
                string classRef = ClassTypeReference.GetClassRef(exampleType);
                Assert.That(classRef.Contains(exampleType.FullName));
                Assert.That(classRef.Contains(exampleType.Assembly.GetName().Name));
            }
        }

        internal class ToStringMethod
        {
            [Test]
            public void When_type_is_null_returns_NoneElement()
            {
                var nullTypeRef = new ClassTypeReference( (Type)null );
                Assert.That(nullTypeRef.ToString(), Is.EqualTo(ClassTypeReference.NoneElement));
            }

            [Test]
            public void When_type_is_not_null_returns_its_full_name()
            {
                Type exampleType = typeof(ClassExample);
                var typeRef = new ClassTypeReference(exampleType);
                Assert.That(typeRef.ToString(), Is.EqualTo(exampleType.FullName));
            }
        }
    }
}
