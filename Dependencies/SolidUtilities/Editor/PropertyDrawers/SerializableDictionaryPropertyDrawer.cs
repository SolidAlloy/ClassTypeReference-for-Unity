namespace SolidUtilities.Editor.PropertyDrawers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using SerializableCollections;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>Property drawer for <see cref="SerializableDictionary{TKey,TValue}"/>.</summary>
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
    public class SerializableDictionaryPropertyDrawer : PropertyDrawer
    {
        private const string KeysFieldName = "_keys";
        private const string ValuesFieldName = "_values";
        private const float IndentWidth = 15f;

        private static readonly GUIContent IconPlus = IconContent("Toolbar Plus", "Add entry");
        private static readonly GUIContent IconMinus = IconContent("Toolbar Minus", "Remove entry");

        private static readonly GUIContent WarningIconConflict =
            IconContent("console.warnicon.sml", "Conflicting key, this entry will be lost");

        private static readonly GUIContent WarningIconOther = IconContent("console.infoicon.sml", "Conflicting key");

        private static readonly GUIContent WarningIconNull =
            IconContent("console.warnicon.sml", "Null key, this entry will be lost");

        private static readonly GUIStyle ButtonStyle = GUIStyle.none;
        private static readonly GUIContent _tempContent = new GUIContent();
        private static readonly Dictionary<SerializedPropertyType, PropertyInfo> SerializedPropertyValueAccessorsDict;

        private static readonly Dictionary<PropertyIdentity, ConflictState> ConflictStateDict =
            new Dictionary<PropertyIdentity, ConflictState>();

        static SerializableDictionaryPropertyDrawer()
        {
            var serializedPropertyValueAccessorsNameDict = new Dictionary<SerializedPropertyType, string>
            {
                { SerializedPropertyType.Integer, "intValue" },
                { SerializedPropertyType.Boolean, "boolValue" },
                { SerializedPropertyType.Float, "floatValue" },
                { SerializedPropertyType.String, "stringValue" },
                { SerializedPropertyType.Color, "colorValue" },
                { SerializedPropertyType.ObjectReference, "objectReferenceValue" },
                { SerializedPropertyType.LayerMask, "intValue" },
                { SerializedPropertyType.Enum, "intValue" },
                { SerializedPropertyType.Vector2, "vector2Value" },
                { SerializedPropertyType.Vector3, "vector3Value" },
                { SerializedPropertyType.Vector4, "vector4Value" },
                { SerializedPropertyType.Rect, "rectValue" },
                { SerializedPropertyType.ArraySize, "intValue" },
                { SerializedPropertyType.Character, "intValue" },
                { SerializedPropertyType.AnimationCurve, "animationCurveValue" },
                { SerializedPropertyType.Bounds, "boundsValue" },
                { SerializedPropertyType.Quaternion, "quaternionValue" }
            };
            Type serializedPropertyType = typeof(SerializedProperty);

            SerializedPropertyValueAccessorsDict = new Dictionary<SerializedPropertyType, PropertyInfo>();
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

            foreach (var kvp in serializedPropertyValueAccessorsNameDict)
            {
                PropertyInfo propertyInfo = serializedPropertyType.GetProperty(kvp.Value, flags);
                SerializedPropertyValueAccessorsDict.Add(kvp.Key, propertyInfo);
            }
        }

        private enum Action
        {
            None,
            Add,
            Remove
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            var buttonAction = Action.None;
            int buttonActionIndex = 0;

            SerializedProperty keyArrayProperty = property.FindPropertyRelative(KeysFieldName);
            SerializedProperty valueArrayProperty = property.FindPropertyRelative(ValuesFieldName);

            ConflictState conflictState = GetConflictState(property);

            if (conflictState.ConflictIndex != -1)
            {
                keyArrayProperty.InsertArrayElementAtIndex(conflictState.ConflictIndex);
                SerializedProperty keyProperty = keyArrayProperty.GetArrayElementAtIndex(conflictState.ConflictIndex);
                SetPropertyValue(keyProperty, conflictState.ConflictKey);
                keyProperty.isExpanded = conflictState.ConflictKeyPropertyExpanded;

                if (valueArrayProperty != null)
                {
                    valueArrayProperty.InsertArrayElementAtIndex(conflictState.ConflictIndex);
                    SerializedProperty valueProperty =
                        valueArrayProperty.GetArrayElementAtIndex(conflictState.ConflictIndex);
                    SetPropertyValue(valueProperty, conflictState.ConflictValue);
                    valueProperty.isExpanded = conflictState.ConflictValuePropertyExpanded;
                }
            }

            float buttonWidth = ButtonStyle.CalcSize(IconPlus).x;

            Rect labelPosition = position;
            labelPosition.height = EditorGUIUtility.singleLineHeight;
            if (property.isExpanded)
                labelPosition.xMax -= ButtonStyle.CalcSize(IconPlus).x;

            EditorGUI.PropertyField(labelPosition, property, label, false);

            if (property.isExpanded)
            {
                Rect buttonPosition = position;
                buttonPosition.xMin = buttonPosition.xMax - buttonWidth;
                buttonPosition.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.BeginDisabledGroup(conflictState.ConflictIndex != -1);
                if (GUI.Button(buttonPosition, IconPlus, ButtonStyle))
                {
                    buttonAction = Action.Add;
                    buttonActionIndex = keyArrayProperty.arraySize;
                }

                EditorGUI.EndDisabledGroup();

                EditorGUI.indentLevel++;
                Rect linePosition = position;
                linePosition.y += EditorGUIUtility.singleLineHeight;
                linePosition.xMax -= buttonWidth;

                foreach (EnumerationEntry entry in EnumerateEntries(keyArrayProperty, valueArrayProperty))
                {
                    SerializedProperty keyProperty = entry.KeyProperty;
                    SerializedProperty valueProperty = entry.ValueProperty;
                    int i = entry.Index;

                    float lineHeight = DrawKeyValueLine(keyProperty, valueProperty, linePosition, i);

                    buttonPosition = linePosition;
                    buttonPosition.x = linePosition.xMax;
                    buttonPosition.height = EditorGUIUtility.singleLineHeight;
                    if (GUI.Button(buttonPosition, IconMinus, ButtonStyle))
                    {
                        buttonAction = Action.Remove;
                        buttonActionIndex = i;
                    }

                    if (i == conflictState.ConflictIndex && conflictState.ConflictOtherIndex == -1)
                    {
                        Rect iconPosition = linePosition;
                        iconPosition.size = ButtonStyle.CalcSize(WarningIconNull);
                        GUI.Label(iconPosition, WarningIconNull);
                    }
                    else if (i == conflictState.ConflictIndex)
                    {
                        Rect iconPosition = linePosition;
                        iconPosition.size = ButtonStyle.CalcSize(WarningIconConflict);
                        GUI.Label(iconPosition, WarningIconConflict);
                    }
                    else if (i == conflictState.ConflictOtherIndex)
                    {
                        Rect iconPosition = linePosition;
                        iconPosition.size = ButtonStyle.CalcSize(WarningIconOther);
                        GUI.Label(iconPosition, WarningIconOther);
                    }

                    linePosition.y += lineHeight;
                }

                EditorGUI.indentLevel--;
            }

            switch (buttonAction)
            {
                case Action.Add:
                {
                    keyArrayProperty.InsertArrayElementAtIndex(buttonActionIndex);
                    valueArrayProperty?.InsertArrayElementAtIndex(buttonActionIndex);
                    break;
                }

                case Action.Remove:
                {
                    DeleteArrayElementAtIndex(keyArrayProperty, buttonActionIndex);
                    if (valueArrayProperty != null)
                        DeleteArrayElementAtIndex(valueArrayProperty, buttonActionIndex);
                    break;
                }
            }

            conflictState.ConflictKey = null;
            conflictState.ConflictValue = null;
            conflictState.ConflictIndex = -1;
            conflictState.ConflictOtherIndex = -1;
            conflictState.ConflictLineHeight = 0f;
            conflictState.ConflictKeyPropertyExpanded = false;
            conflictState.ConflictValuePropertyExpanded = false;

            foreach (EnumerationEntry entry1 in EnumerateEntries(keyArrayProperty, valueArrayProperty))
            {
                SerializedProperty keyProperty1 = entry1.KeyProperty;
                int i = entry1.Index;
                object keyProperty1Value = GetPropertyValue(keyProperty1);

                if (keyProperty1Value == null)
                {
                    SerializedProperty valueProperty1 = entry1.ValueProperty;
                    SaveProperty(keyProperty1, valueProperty1, i, -1, conflictState);
                    DeleteArrayElementAtIndex(keyArrayProperty, i);
                    if (valueArrayProperty != null)
                        DeleteArrayElementAtIndex(valueArrayProperty, i);

                    break;
                }

                foreach (EnumerationEntry entry2 in EnumerateEntries(keyArrayProperty, valueArrayProperty, i + 1))
                {
                    SerializedProperty keyProperty2 = entry2.KeyProperty;
                    int j = entry2.Index;
                    object keyProperty2Value = GetPropertyValue(keyProperty2);

                    if ( ! ComparePropertyValues(keyProperty1Value, keyProperty2Value))
                        continue;
                    SerializedProperty valueProperty2 = entry2.ValueProperty;
                    SaveProperty(keyProperty2, valueProperty2, j, i, conflictState);
                    DeleteArrayElementAtIndex(keyArrayProperty, j);
                    if (valueArrayProperty != null)
                        DeleteArrayElementAtIndex(valueArrayProperty, j);

                    goto breakLoops;
                }
            }

            breakLoops:

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float propertyHeight = EditorGUIUtility.singleLineHeight;

            if (!property.isExpanded)
                return propertyHeight;

            SerializedProperty keysProperty = property.FindPropertyRelative(KeysFieldName);
            SerializedProperty valuesProperty = property.FindPropertyRelative(ValuesFieldName);

            foreach (EnumerationEntry entry in EnumerateEntries(keysProperty, valuesProperty))
            {
                SerializedProperty keyProperty = entry.KeyProperty;
                SerializedProperty valueProperty = entry.ValueProperty;
                float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
                float valuePropertyHeight = valueProperty != null ? EditorGUI.GetPropertyHeight(valueProperty) : 0f;
                float lineHeight = Mathf.Max(keyPropertyHeight, valuePropertyHeight);
                propertyHeight += lineHeight;
            }

            ConflictState conflictState = GetConflictState(property);

            if (conflictState.ConflictIndex != -1) propertyHeight += conflictState.ConflictLineHeight;

            return propertyHeight;
        }

        private static float DrawKeyValueLine(SerializedProperty keyProperty, SerializedProperty valueProperty,
            Rect linePosition, int index)
        {
            bool keyCanBeExpanded = CanPropertyBeExpanded(keyProperty);
            string keyLabel;

            if (valueProperty != null)
            {
                bool valueCanBeExpanded = CanPropertyBeExpanded(valueProperty);

                if ( ! keyCanBeExpanded && valueCanBeExpanded)
                    return DrawKeyValueLineExpand(keyProperty, valueProperty, linePosition);

                keyLabel = keyCanBeExpanded ? "Key " + index : string.Empty;
                string valueLabel = valueCanBeExpanded ? "Value " + index : string.Empty;
                return DrawKeyValueLineSimple(keyProperty, valueProperty, keyLabel, valueLabel, linePosition);
            }

            if ( ! keyCanBeExpanded)
                return DrawKeyLine(keyProperty, linePosition, null);

            keyLabel = $"{ObjectNames.NicifyVariableName(keyProperty.type)} {index}";
            return DrawKeyLine(keyProperty, linePosition, keyLabel);
        }

        private static float DrawKeyValueLineSimple(SerializedProperty keyProperty, SerializedProperty valueProperty,
            string keyLabel, string valueLabel, Rect linePosition)
        {
            float labelWidth = EditorGUIUtility.labelWidth;
            float labelWidthRelative = labelWidth / linePosition.width;

            float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
            Rect keyPosition = linePosition;
            keyPosition.height = keyPropertyHeight;
            keyPosition.width = labelWidth - IndentWidth;
            EditorGUIUtility.labelWidth = keyPosition.width * labelWidthRelative;
            EditorGUI.PropertyField(keyPosition, keyProperty, TempContent(keyLabel), true);

            float valuePropertyHeight = EditorGUI.GetPropertyHeight(valueProperty);
            Rect valuePosition = linePosition;
            valuePosition.height = valuePropertyHeight;
            valuePosition.xMin += labelWidth;
            EditorGUIUtility.labelWidth = valuePosition.width * labelWidthRelative;
            EditorGUI.indentLevel--;
            EditorGUI.PropertyField(valuePosition, valueProperty, TempContent(valueLabel), true);
            EditorGUI.indentLevel++;

            EditorGUIUtility.labelWidth = labelWidth;

            return Mathf.Max(keyPropertyHeight, valuePropertyHeight);
        }

        private static float DrawKeyValueLineExpand(SerializedProperty keyProperty, SerializedProperty valueProperty,
            Rect linePosition)
        {
            float labelWidth = EditorGUIUtility.labelWidth;

            float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
            Rect keyPosition = linePosition;
            keyPosition.height = keyPropertyHeight;
            keyPosition.width = labelWidth - IndentWidth;
            EditorGUI.PropertyField(keyPosition, keyProperty, GUIContent.none, true);

            float valuePropertyHeight = EditorGUI.GetPropertyHeight(valueProperty);
            Rect valuePosition = linePosition;
            valuePosition.height = valuePropertyHeight;
            EditorGUI.PropertyField(valuePosition, valueProperty, GUIContent.none, true);

            EditorGUIUtility.labelWidth = labelWidth;

            return Mathf.Max(keyPropertyHeight, valuePropertyHeight);
        }

        private static float DrawKeyLine(SerializedProperty keyProperty, Rect linePosition, string keyLabel)
        {
            float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
            Rect keyPosition = linePosition;
            keyPosition.height = keyPropertyHeight;
            keyPosition.width = linePosition.width;

            GUIContent keyLabelContent = keyLabel != null ? TempContent(keyLabel) : GUIContent.none;
            EditorGUI.PropertyField(keyPosition, keyProperty, keyLabelContent, true);

            return keyPropertyHeight;
        }

        private static bool CanPropertyBeExpanded(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Generic:
                case SerializedPropertyType.Vector4:
                case SerializedPropertyType.Quaternion:
                    return true;
                default:
                    return false;
            }
        }

        private static void SaveProperty(SerializedProperty keyProperty, SerializedProperty valueProperty, int index,
            int otherIndex, ConflictState conflictState)
        {
            conflictState.ConflictKey = GetPropertyValue(keyProperty);
            conflictState.ConflictValue = valueProperty != null ? GetPropertyValue(valueProperty) : null;
            float keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
            float valuePropertyHeight = valueProperty != null ? EditorGUI.GetPropertyHeight(valueProperty) : 0f;
            float lineHeight = Mathf.Max(keyPropertyHeight, valuePropertyHeight);
            conflictState.ConflictLineHeight = lineHeight;
            conflictState.ConflictIndex = index;
            conflictState.ConflictOtherIndex = otherIndex;
            conflictState.ConflictKeyPropertyExpanded = keyProperty.isExpanded;
            conflictState.ConflictValuePropertyExpanded = valueProperty?.isExpanded ?? false;
        }

        private static ConflictState GetConflictState(SerializedProperty property)
        {
            var propId = new PropertyIdentity(property);
            if ( ! ConflictStateDict.TryGetValue(propId, out ConflictState conflictState))
            {
                conflictState = new ConflictState();
                ConflictStateDict.Add(propId, conflictState);
            }

            return conflictState;
        }

        private static GUIContent IconContent(string name, string tooltip)
        {
            GUIContent builtinIcon = EditorGUIUtility.IconContent(name);
            return new GUIContent(builtinIcon.image, tooltip);
        }

        private static GUIContent TempContent(string text)
        {
            _tempContent.text = text;
            return _tempContent;
        }

        private static void DeleteArrayElementAtIndex(SerializedProperty arrayProperty, int index)
        {
            SerializedProperty property = arrayProperty.GetArrayElementAtIndex(index);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
                property.objectReferenceValue = null;

            arrayProperty.DeleteArrayElementAtIndex(index);
        }

        private static object GetPropertyValue(SerializedProperty p)
        {
            if (SerializedPropertyValueAccessorsDict.TryGetValue(p.propertyType, out PropertyInfo propertyInfo))
                return propertyInfo.GetValue(p, null);

            return p.isArray ? GetPropertyValueArray(p) : GetPropertyValueGeneric(p);
        }

        private static void SetPropertyValue(SerializedProperty p, object v)
        {
            if (SerializedPropertyValueAccessorsDict.TryGetValue(p.propertyType, out PropertyInfo propertyInfo))
            {
                propertyInfo.SetValue(p, v, null);
            }
            else
            {
                if (p.isArray)
                    SetPropertyValueArray(p, v);
                else
                    SetPropertyValueGeneric(p, v);
            }
        }

        private static object GetPropertyValueArray(SerializedProperty property)
        {
            var array = new object[property.arraySize];

            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty item = property.GetArrayElementAtIndex(i);
                array[i] = GetPropertyValue(item);
            }

            return array;
        }

        private static object GetPropertyValueGeneric(SerializedProperty property)
        {
            var dict = new Dictionary<string, object>();
            SerializedProperty iterator = property.Copy();
            if ( ! iterator.Next(true))
                return dict;

            SerializedProperty end = property.GetEndProperty();

            do
            {
                string name = iterator.name;
                object value = GetPropertyValue(iterator);
                dict.Add(name, value);
            }
            while (iterator.Next(false) && iterator.propertyPath != end.propertyPath);

            return dict;
        }

        private static void SetPropertyValueArray(SerializedProperty property, object v)
        {
            var array = (object[]) v;
            property.arraySize = array.Length;

            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty item = property.GetArrayElementAtIndex(i);
                SetPropertyValue(item, array[i]);
            }
        }

        private static void SetPropertyValueGeneric(SerializedProperty property, object v)
        {
            var dict = (Dictionary<string, object>) v;
            SerializedProperty iterator = property.Copy();
            if ( ! iterator.Next(true))
                return;

            SerializedProperty end = property.GetEndProperty();

            do
            {
                string name = iterator.name;
                SetPropertyValue(iterator, dict[name]);
            }
            while (iterator.Next(false) && iterator.propertyPath != end.propertyPath);
        }

        private static bool ComparePropertyValues(object value1, object value2)
        {
            if (value1 is Dictionary<string, object> dict1 && value2 is Dictionary<string, object> dict2)
                return CompareDictionaries(dict1, dict2);

            return Equals(value1, value2);
        }

        private static bool CompareDictionaries(Dictionary<string, object> dict1, Dictionary<string, object> dict2)
        {
            if (dict1.Count != dict2.Count)
                return false;

            foreach (var kvp1 in dict1)
            {
                string key1 = kvp1.Key;
                object value1 = kvp1.Value;

                if ( ! dict2.TryGetValue(key1, out object value2))
                    return false;

                if ( ! ComparePropertyValues(value1, value2))
                    return false;
            }

            return true;
        }

        private static IEnumerable<EnumerationEntry> EnumerateEntries(
            SerializedProperty keyArrayProperty,
            SerializedProperty valueArrayProperty, int startIndex = 0)
        {
            if (keyArrayProperty.arraySize <= startIndex)
                yield break;

            int index = startIndex;
            SerializedProperty keyProperty = keyArrayProperty.GetArrayElementAtIndex(startIndex);
            SerializedProperty valueProperty = valueArrayProperty?.GetArrayElementAtIndex(startIndex);
            SerializedProperty endProperty = keyArrayProperty.GetEndProperty();

            do
            {
                yield return new EnumerationEntry(keyProperty, valueProperty, index);
                index++;
            }
            while (keyProperty.Next(false) && (valueProperty?.Next(false) ?? true) &&
                   ! SerializedProperty.EqualContents(keyProperty, endProperty));
        }

        private readonly struct EnumerationEntry
        {
            public readonly SerializedProperty KeyProperty;
            public readonly SerializedProperty ValueProperty;
            public readonly int Index;

            public EnumerationEntry(SerializedProperty keyProperty, SerializedProperty valueProperty, int index)
            {
                KeyProperty = keyProperty;
                ValueProperty = valueProperty;
                Index = index;
            }
        }

        private struct PropertyIdentity
        {
            public PropertyIdentity(SerializedProperty property)
            {
                Instance = property.serializedObject.targetObject;
                PropertyPath = property.propertyPath;
            }

            public Object Instance;
            public string PropertyPath;
        }

        private class ConflictState
        {
            public object ConflictKey;
            public object ConflictValue;
            public int ConflictIndex = -1;
            public int ConflictOtherIndex = -1;
            public bool ConflictKeyPropertyExpanded;
            public bool ConflictValuePropertyExpanded;
            public float ConflictLineHeight;
        }
    }
}