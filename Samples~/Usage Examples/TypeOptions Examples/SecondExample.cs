namespace TypeReferences.Demo.TypeOptions_Examples
{
    using Utils;

    public class SecondExample : TypeReferenceExample
    {
        [InfoBox("If you use TypeReference without any attributes, it lists all the classes, structs, " +
                 "and interfaces the assembly has access to.")]
        public TypeReference AnyType;
    }
}