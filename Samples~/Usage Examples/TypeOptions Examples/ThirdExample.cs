namespace TypeReferences.Demo.TypeOptions_Examples
{
    using Utils;

    public class ThirdExample : TypeReferenceExample
    {
        [InfoBox("You can customize the look of the drop-down menu and what types it contains by using the " +
                 "[TypeOptions] attribute. In this example, we use [TypeOptions(Grouping = Grouping.None)] to not " +
                 "group classes at all and show them in the single list.")]
        [TypeOptions(Grouping = Grouping.None)] public TypeReference FlatList;
    }
}