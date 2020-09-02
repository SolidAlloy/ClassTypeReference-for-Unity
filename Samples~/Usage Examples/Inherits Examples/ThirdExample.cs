namespace TypeReferences.Demo.Inherits_Examples
{
    using Utils;

    public class ThirdExample : TypeReferenceExample
    {
        [InfoBox("By default, abstract types (abstract classes and interfaces) are not included in the " +
                 "drop-down list. However, you can allow them: [Inherits(typeof(IGreetingLogger), AllowAbstract = true)]")]
        [Inherits(typeof(IGreetingLogger), AllowAbstract = true)]
        public TypeReference GreetingLoggerType;
    }
}