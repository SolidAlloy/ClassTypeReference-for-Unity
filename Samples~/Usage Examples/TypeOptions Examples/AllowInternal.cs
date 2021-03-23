namespace TypeReferences.Demo.TypeOptions_Examples
{
    using System;
    using UnityEngine;
    using Utils;

    public class AllowInternal : TypeReferenceExample
    {
        [InfoBox("AllowInternal makes internal types appear in the drop-down. By default only public types are shown.")]
        [Inherits(typeof(IGreetingLogger), AllowInternal = true)]
        public TypeReference GreetingLoggerType;

        [Button]
        public void LogGreeting()
        {
            if (GreetingLoggerType.Type == null)
            {
                Debug.LogWarning("No greeting logger was specified.");
            }
            else
            {
                var greetingLogger = Activator.CreateInstance(GreetingLoggerType) as IGreetingLogger;
                greetingLogger.LogGreeting();
            }
        }
    }
}