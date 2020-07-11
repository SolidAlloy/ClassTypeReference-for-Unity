// Copyright ClassTypeReference Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

namespace Example
{
    using UnityEngine;

    public interface IGreetingLogger
    {
        void LogGreeting();
    }

    public class DefaultGreetingLogger : IGreetingLogger
    {
        public void LogGreeting()
        {
            Debug.Log("Hello, World!");
        }
    }

    public class AnotherGreetingLogger : IGreetingLogger
    {
        public void LogGreeting()
        {
            Debug.Log("Greetings!");
        }
    }
}
