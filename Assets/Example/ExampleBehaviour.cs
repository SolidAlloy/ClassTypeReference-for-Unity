// Copyright (c) 2014 Rotorz Limited. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

using System;

using UnityEngine;

using TypeReferences;

namespace Example {

	public class ExampleBehaviour : MonoBehaviour {

		[ClassImplements(typeof(IGreetingLogger))]
		public ClassTypeReference greetingLoggerType = typeof(DefaultGreetingLogger);

		private void Start() {
			if (greetingLoggerType.Type == null) {
				Debug.LogWarning("No greeting logger was specified.");
			}
			else {
				var greetingLogger = Activator.CreateInstance(greetingLoggerType) as IGreetingLogger;
				greetingLogger.LogGreeting();
			}
		}

	}

}
