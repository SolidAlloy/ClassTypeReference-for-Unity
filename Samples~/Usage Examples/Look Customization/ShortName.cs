namespace TypeReferences.Demo.Look_Customization
{
    using Utils;

    public class ShortName : TypeReferenceExample
    {
        [InfoBox("You can make the field show just the type name without its namespace with " +
                 "[TypeOptions(ShortName = true)].")]
        [Inherits(typeof(IGreetingLogger), ShortName = true)] public TypeReference GreetingLoggerType;
    }
}