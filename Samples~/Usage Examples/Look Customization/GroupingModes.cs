namespace TypeReferences.Demo.Look_Customization
{
    using UnityEngine;
    using Utils;

    public class GroupingModes : TypeReferenceExample
    {
        [InfoBox("You can customize the look of the drop-down menu and what types it contains by using the " +
                 "[TypeOptions] attribute. First thing you might want to change is how classes are grouped. By default, " +
                 "they are grouped by namespace. In this example, we use [TypeOptions(Grouping = Grouping.None)] " +
                 "to not group classes at all and show them in a single list.")]
        [TypeOptions(Grouping = Grouping.None)] public TypeReference FlatList;

        [InfoBox("A separate folder can be created for each nested namespace, with Grouping.ByNamespace.")]
        [TypeOptions(Grouping = Grouping.ByNamespace)] public TypeReference GroupedInFolders;

        [InfoBox("Finally, types can be grouped in the same way as Unity does for its component menu. This " +
                 "grouping method must only be used for MonoBehaviour types. Enable it with Grouping.ByAddComponentMenu.")]
        [Inherits(typeof(MonoBehaviour), Grouping = Grouping.ByAddComponentMenu)]
        public TypeReference LikeAddComponent;
    }
}