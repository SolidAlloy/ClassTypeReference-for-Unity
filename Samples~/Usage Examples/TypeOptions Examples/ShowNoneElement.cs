namespace TypeReferences.Demo.TypeOptions_Examples
{
    using Utils;

    public class ShowNoneElement : TypeReferenceExample
    {
        [InfoBox("You can hide the (None) element so that no one can choose it from the dropdown. Note that the type can still be null by default or if set through code.")]
        [Inherits(typeof(IGreetingLogger), ShowNoneElement = false, ExpandAllFolders = true)]
        public TypeReference GreetingLoggerType;
    }
}