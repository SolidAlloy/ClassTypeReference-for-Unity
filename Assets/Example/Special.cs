// Copyright (c) 2014 Rotorz Limited. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

using UnityEngine;

namespace Example {

	public interface IGreetingLogger {

		void LogGreeting();

	}

	public class DefaultGreetingLogger : IGreetingLogger {

		public void LogGreeting() {
			Debug.Log("Hello, World!");
		}

	}

	public class AnotherGreetingLogger : IGreetingLogger {

		public void LogGreeting() {
			Debug.Log("Greetings!");
		}

	}

}
