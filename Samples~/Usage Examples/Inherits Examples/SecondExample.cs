namespace TypeReferences.Demo.Inherits_Examples
{
    using Utils;

    public class SecondExample : TypeReferenceExample
    {
        [InfoBox("If you need to have the base type in the drop-down menu too, use IncludeBaseType like this: " +
                 "[Inherits(typeof(IGreetingLogger), IncludeBaseType = true)]")]
        [Inherits(typeof(IGreetingLogger), IncludeBaseType = true)]
        public TypeReference GreetingLoggerType;
    }
}