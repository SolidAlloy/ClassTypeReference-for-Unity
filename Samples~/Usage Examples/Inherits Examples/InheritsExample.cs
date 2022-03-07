namespace TypeReferences.Demo.Inherits_Examples
{
    using UnityEngine;
    using Utils;

    public class InheritsExample : TypeReferenceExample
    {
        [InfoBox("[Inherits] attribute allows you to choose only from the classes that implement a certain " +
                 "interface or extend a class. It has all the arguments TypeOptions provides. This one lists only " +
                 "classes that implement IGreetingLogger:")]
        [Inherits(typeof(IGreetingLogger))]
        public TypeReference GreetingLoggerType;

        [InfoBox("This one lists only classes that extend MonoBehaviour with help of [Inherits(typeof(MonoBehaviour))].")]
        [Inherits(typeof(MonoBehaviour))]
        public TypeReference OnlyMonoBehaviours;

        [InfoBox("All the TypeOptions arguments are available with Inherits too.")]
        [Inherits(typeof(IGreetingLogger), ShowNoneElement = false)]
        public TypeReference NoneExcluded;
    }
}