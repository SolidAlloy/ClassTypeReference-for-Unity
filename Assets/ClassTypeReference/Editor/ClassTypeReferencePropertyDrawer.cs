// Copyright (c) 2014 Rotorz Limited. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEditor;

namespace TypeReferences.Editor {

	/// <summary>
	/// Custom property drawer for <see cref="ClassTypeReference"/> properties.
	/// </summary>
	[CustomPropertyDrawer(typeof(ClassTypeReference))]
	[CustomPropertyDrawer(typeof(ClassTypeConstraintAttribute), true)]
	class ClassTypeReferencePropertyDrawer : PropertyDrawer {

		#region Type Filtering
		
		private static List<Type> GetFilteredTypes(ClassTypeConstraintAttribute filter) {
			var types = new List<Type>();

			var assembly = Assembly.GetExecutingAssembly();
			FilterTypes(assembly, filter, types);

			foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
				FilterTypes(Assembly.Load(referencedAssembly), filter, types);

			types.Sort((a, b) => a.FullName.CompareTo(b.FullName));

			return types;
		}

		private static void FilterTypes(Assembly assembly, ClassTypeConstraintAttribute filter, List<Type> output) {
			foreach (var type in assembly.GetTypes()) {
				if (!type.IsPublic || !type.IsClass)
					continue;

				if (filter != null && !filter.IsConstraintSatisfied(type))
					continue;

				output.Add(type);
			}
		}

		#endregion

		#region Control Drawing / Event Handling

		private static GUIContent s_TempContent = new GUIContent();

		private static string DrawTypeSelectionControl(Rect position, GUIContent label, string classRef, ClassTypeConstraintAttribute filter) {
			if (label != null && label != GUIContent.none)
				position = EditorGUI.PrefixLabel(position, label);

			int controlID = GUIUtility.GetControlID(FocusType.Keyboard, position);

			bool triggerDropDown = false;

			switch (Event.current.GetTypeForControl(controlID)) {
				case EventType.ExecuteCommand:
					if (Event.current.commandName == "TypeReferenceUpdated") {
						if (s_SelectionControlID == controlID) {
							if (classRef != s_SelectedClassRef) {
								classRef = s_SelectedClassRef;
								GUI.changed = true;
							}

							s_SelectionControlID = 0;
							s_SelectedClassRef = null;
						}
					}
					break;

				case EventType.MouseDown:
					if (GUI.enabled && position.Contains(Event.current.mousePosition)) {
						GUIUtility.keyboardControl = controlID;
						triggerDropDown = true;
						Event.current.Use();
					}
					break;

				case EventType.KeyDown:
					if (GUI.enabled && GUIUtility.keyboardControl == controlID) {
						if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.Space) {
							triggerDropDown = true;
							Event.current.Use();
						}
					}
					break;

				case EventType.Repaint:
					// Remove assembly name from content of popup control.
					var classRefParts = classRef.Split(',');

					s_TempContent.text = classRefParts[0].Trim();
					if (s_TempContent.text == "")
						s_TempContent.text = "(None)";

					EditorStyles.popup.Draw(position, s_TempContent, controlID);
					break;
			}
			
			if (triggerDropDown) {
				s_SelectionControlID = controlID;
				s_SelectedClassRef = classRef;
				
				var selectedType = Type.GetType(classRef);
				
				var filteredTypes = GetFilteredTypes(filter);
				DisplayDropDown(position, filteredTypes, selectedType, filter.Grouping);
			}

			return classRef;
		}

		private static void DrawTypeSelectionControl(Rect position, SerializedProperty property, GUIContent label, ClassTypeConstraintAttribute filter) {
			bool restoreShowMixedValue = EditorGUI.showMixedValue;
			EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

			property.stringValue = DrawTypeSelectionControl(position, label, property.stringValue, filter);

			EditorGUI.showMixedValue = restoreShowMixedValue;
		}

		private static void DisplayDropDown(Rect position, List<Type> types, Type selectedType, ClassGrouping grouping) {
			var menu = new GenericMenu();

			menu.AddItem(new GUIContent("(None)"), selectedType == null, s_OnSelectedTypeName, null);
			menu.AddSeparator("");

			for (int i = 0; i < types.Count; ++i) {
				var type = types[i];

				string menuLabel = FormatGroupedTypeName(type, grouping);
				if (string.IsNullOrEmpty(menuLabel))
					continue;

				var content = new GUIContent(menuLabel);
				menu.AddItem(content, type == selectedType, s_OnSelectedTypeName, type);
			}

			menu.DropDown(position);
		}

		private static string FormatGroupedTypeName(Type type, ClassGrouping grouping) {
			string name = type.FullName;

			switch (grouping) {
				default:
				case ClassGrouping.None:
					return name;

				case ClassGrouping.ByNamespace:
					return name.Replace('.', '/');

				case ClassGrouping.ByNamespaceFlat:
					int lastPeriodIndex = name.LastIndexOf('.');
					if (lastPeriodIndex != -1)
						name = name.Substring(0, lastPeriodIndex) + "/" + name.Substring(lastPeriodIndex + 1);

					return name;

				case ClassGrouping.ByAddComponentMenu:
					var addComponentMenuAttributes = type.GetCustomAttributes(typeof(AddComponentMenu), false);
					if (addComponentMenuAttributes.Length == 1)
						return ((AddComponentMenu)addComponentMenuAttributes[0]).componentMenu;

					return "Scripts/" + type.FullName.Replace('.', '/');
			}
		}

		private static int s_SelectionControlID;
		private static string s_SelectedClassRef;

		private static readonly GenericMenu.MenuFunction2 s_OnSelectedTypeName = OnSelectedTypeName;

		private static void OnSelectedTypeName(object userData) {
			var selectedType = userData as Type;

			s_SelectedClassRef = ClassTypeReference.GetClassRef(selectedType);
			
			var typeReferenceUpdatedEvent = EditorGUIUtility.CommandEvent("TypeReferenceUpdated");
			EditorWindow.focusedWindow.SendEvent(typeReferenceUpdatedEvent);
		}

		#endregion

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorStyles.popup.CalcHeight(GUIContent.none, 0);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			DrawTypeSelectionControl(position, property.FindPropertyRelative("_classRef"), label, attribute as ClassTypeConstraintAttribute);
		}

	}

}
