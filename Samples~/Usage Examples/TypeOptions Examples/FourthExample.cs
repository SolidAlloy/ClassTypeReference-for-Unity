namespace TypeReferences.Demo.TypeOptions_Examples
{
    using UnityEngine;
    using Utils;

    public class FourthExample : TypeReferenceExample
    {
        [InfoBox("You can include the types that are not in the list originally by using the IncludeTypes " +
                 "option, and exclude types with ExcludeTypes. In this example, we use " +
                 "IncludeTypes = new[] { typeof(MonoBehaviour) }, ExcludeTypes = new[] { typeof(DefaultGreetingLogger) }")]
        [Inherits(
            typeof(IGreetingLogger),
            ExpandAllFolders = true,
            IncludeTypes = new[] { typeof(MonoBehaviour) },
            ExcludeTypes = new[] { typeof(DefaultGreetingLogger) })]
        public TypeReference LoggerWithMonoBehaviourAndWithoutDefault;
    }
}