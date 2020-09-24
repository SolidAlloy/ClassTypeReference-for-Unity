namespace TypeReferences.Demo.Advanced_Look_Customization
{
    using Utils;

    public class SecondExample : TypeReferenceExample
    {
        [InfoBox("By default, folders are closed. If you want them all to be expanded when you open the dropdown, " +
                 "add [TypeOptions(ExpandAllFolders = true)]")]
        [TypeOptions(ExpandAllFolders = true)]
        public TypeReference AllTypes;
    }
}
