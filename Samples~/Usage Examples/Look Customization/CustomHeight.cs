namespace TypeReferences.Demo.Look_Customization
{
    using Utils;

    public class CustomHeight : TypeReferenceExample
    {
        [InfoBox("If you are not satisfied with the auto-adjusted height, you can set the custom one with " +
                 "DropdownHeight option. Use it like this: [Inherits(typeof(IGreetingLogger), DropdownHeight = 300)]")]
        [Inherits(typeof(IGreetingLogger), ExpandAllFolders = true, DropdownHeight = 300)]
        public TypeReference GreetingLoggerType;
    }
}
