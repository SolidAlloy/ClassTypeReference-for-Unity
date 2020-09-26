namespace TypeReferences.Demo.Look_Customization
{
    using Utils;

    public class Searchbar : TypeReferenceExample
    {
        [InfoBox("By default, the searchbar appears when you have more than 10 types in the dropdown list. " +
                 "You can change this with the SearchbarMinItemsCount option. Here we used SearchbarMinItemsCount = 0")]
        [Inherits(typeof(IGreetingLogger), SearchbarMinItemsCount = 0)]
        public TypeReference GreetingLoggerType;
    }
}
