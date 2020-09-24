namespace TypeReferences.Demo.TypeOptions_Examples
{
    using Utils;

    public class FifthExample : TypeReferenceExample
    {
        [InfoBox("You can exclude (None) so that no one can choose it from the dropdown. Note that the type " +
                 "can still be null by default or if set through code.")]
        [Inherits(typeof(IGreetingLogger), ExcludeNone = true, ExpandAllFolders = true)]
        public TypeReference GreetingLoggerType;
    }
}