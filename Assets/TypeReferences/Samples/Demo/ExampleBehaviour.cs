// Copyright ClassTypeReference Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

namespace Example
{
    using System;
    using TypeReferences;
    using UnityEngine;

    public class ExampleBehaviour : MonoBehaviour
    {
        [ClassImplements(typeof(IGreetingLogger))]
        public ClassTypeReference GreetingLoggerType = typeof(DefaultGreetingLogger);

        private void Start()
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
