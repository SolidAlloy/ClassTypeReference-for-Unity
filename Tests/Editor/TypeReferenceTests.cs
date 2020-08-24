namespace TypeReferences.Tests.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    internal class TypeReferenceTests
    {
        private static Type GetGenericType()
        {
            return typeof(IEnumerable<>).GetGenericArguments().First();
        }

        private struct NotAClass { }
        private class ClassExample { }

        internal class TheConstructor
        {
            [Test]
            public void When_no_arguments_are_passed_creates_instance_with_null_type()
            {
                var typeRef = new TypeReference();
                Assert.That(typeRef.Type, Is.Null);
            }

            [Test]
            public void When_null_type_is_passed_creates_instance_with_null_type()
            {
                var typeRef = new TypeReference( (Type)null );
                Assert.That(typeRef.Type, Is.Null);
            }

            [Test]
            public void When_not_a_class_type_is_passed_creates_instance_with_this_type()
            {
                var notAClassType = typeof(NotAClass);
                var typeRef = new TypeReference(notAClassType);
                Assert.That(typeRef.Type, Is.EqualTo(notAClassType));
            }

            [Test]
            public void When_a_class_type_is_passed_creates_instance_with_this_type()
            {
                var classType = typeof(ClassExample);
                var typeRef = new TypeReference(classType);
                Assert.That(typeRef.Type, Is.EqualTo(classType));
            }

            [Test]
            public void When_a_type_without_name_is_passed_throws_argument_exception()
            {
                var genericType = GetGenericType();

                Assert.Throws<ArgumentException>(() =>
                {
                    var typeRef = new TypeReference(genericType);
                });
            }

            [Test]
            public void When_null_string_is_passed_creates_instance_with_null_type()
            {
                var typeRef = new TypeReference( (string)null );
                Assert.That(typeRef.Type, Is.Null);
            }

            [Test]
            public void When_empty_string_is_passed_creates_instance_with_null_type()
            {
                var typeRef = new TypeReference(string.Empty);
                Assert.That(typeRef.Type, Is.Null);
            }

            [Test]
            public void When_not_assembly_qualified_name_string_is_passed_creates_instance_with_null_type()
            {
                string typeName = "wrongTypeName";
                var typeRef = new TypeReference(typeName);
                Assert.That(typeRef.Type, Is.Null);
            }

            [Test]
            public void When_assembly_qualified_struct_name_string_is_passed_creates_instance_with_this_type()
            {
                Type notAClassType = typeof(NotAClass);
                string typeName = notAClassType.AssemblyQualifiedName;
                var typeRef = new TypeReference(typeName);
                Assert.That(typeRef.Type, Is.EqualTo(notAClassType));
            }

            [Test]
            public void When_assembly_qualified_class_name_string_is_passed_creates_instance_with_this_type()
            {
                Type classType = typeof(ClassExample);
                string typeName = classType.AssemblyQualifiedName;
                var typeRef = new TypeReference(typeName);
                Assert.That(typeRef.Type, Is.EqualTo(classType));
            }
        }

        internal class ImplicitTypeConversion
        {
            [Test]
            public void Class_type_can_be_converted_to_ClassTypeReference()
            {
                Type classType = typeof(ClassExample);
                TypeReference typeRef = classType;
                Assert.That(typeRef.Type, Is.EqualTo(classType));
            }

            [Test]
            public void Not_a_class_type_can_be_converted_to_ClassTypeReference()
            {
                Type notAClassType = typeof(NotAClass);
                TypeReference typeRef = notAClassType;
                Assert.That(typeRef.Type, Is.EqualTo(notAClassType));
            }

            [Test]
            public void When_type_without_name_is_converted_to_ClassTypeReference_ArgumentException_is_thrown()
            {
                var genericType = GetGenericType();

                Assert.Throws<ArgumentException>(() =>
                {
                    TypeReference typeRef = genericType;
                });
            }

            [Test]
            public void ClassTypeReference_can_be_converted_to_Type()
            {
                Type initialType = typeof(ClassExample);
                var typeRef = new TypeReference(initialType);
                Type convertedType = typeRef;
                Assert.That(convertedType, Is.EqualTo(initialType));
            }
        }

        internal class TheTypeProperty
        {
            private TypeReference _typeRef;

            [SetUp]
            public void BeforeEveryTest()
            {
                _typeRef = new TypeReference();
            }

            [Test]
            public void When_not_a_class_type_is_set_returns_the_set_type()
            {
                Type notAClassType = typeof(NotAClass);
                _typeRef.Type = notAClassType;
                Assert.That(_typeRef.Type, Is.EqualTo(notAClassType));
            }

            [Test]
            public void When_class_type_is_set_returns_the_set_type()
            {
                Type classType = typeof(ClassExample);
                _typeRef.Type = classType;
                Assert.That(_typeRef.Type, Is.EqualTo(classType));
            }

            [Test]
            public void When_type_without_name_is_set_throws_ArgumentException()
            {
                var genericType = GetGenericType();

                Assert.Throws<ArgumentException>(() =>
                {
                    _typeRef.Type = genericType;
                });
            }
        }

        internal class TheGetTypeNameAndAssemblyMethod
        {
            [Test]
            public void When_null_is_passed_returns_empty_string()
            {
                string typeAndAssembly = TypeReference.GetTypeNameAndAssembly(null);
                Assert.That(typeAndAssembly, Is.EqualTo(string.Empty));
            }

            [Test]
            public void When_type_is_passed_returns_string_that_contains_full_type_name_and_assembly_name()
            {
                var exampleType = typeof(ClassExample);
                string typeAndAssembly = TypeReference.GetTypeNameAndAssembly(exampleType);
                Assert.That(typeAndAssembly.Contains(exampleType.FullName));
                Assert.That(typeAndAssembly.Contains(exampleType.Assembly.GetName().Name));
            }

            [Test]
            public void When_type_without_name_is_passed_throws_ArgumentException()
            {
                var genericType = GetGenericType();

                Assert.Throws<ArgumentException>(() =>
                {
                    TypeReference.GetTypeNameAndAssembly(genericType);
                });
            }
        }

        internal class TheGetClassGuidMethod
        {
            [Test]
            public void When_type_is_null_returns_empty_string()
            {
                Assert.That(TypeReference.GetClassGUID(null), Is.EqualTo(string.Empty));
            }

            [Test]
            public void When_type_full_name_is_null_returns_empty_string()
            {
                var genericType = GetGenericType();

                Assert.That(TypeReference.GetClassGUID(genericType), Is.EqualTo(string.Empty));
            }

            [Test]
            public void When_one_asset_is_found_by_type_name_returns_its_GUID()
            {
                // I found no easy way to test this. Perhaps, someone can help.
            }
        }

        internal class ToStringMethod
        {
            [Test]
            public void When_type_is_null_returns_NoneElement()
            {
                var nullTypeRef = new TypeReference( (Type)null );
                Assert.That(nullTypeRef.ToString(), Is.EqualTo(TypeReference.NoneElement));
            }

            [Test]
            public void When_type_is_not_null_returns_its_full_name()
            {
                Type exampleType = typeof(ClassExample);
                var typeRef = new TypeReference(exampleType);
                Assert.That(typeRef.ToString(), Is.EqualTo(exampleType.FullName));
            }
        }
    }
}