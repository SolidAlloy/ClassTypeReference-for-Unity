namespace TypeReferences.Editor
{
    using System;
    using UnityEditor;
    using UnityEngine;

    internal class TypeField
    {
        private readonly SerializedProperty _property;
        private readonly TypeDropDown _dropDown;
        private Rect _position;
        private bool _triggerDropDown;

        public TypeField(
            SerializedProperty property,
            Rect position,
            ClassTypeConstraintAttribute filter,
            Type declaringType)
        {
            _property = property;
            _position = position;

            _dropDown = new TypeDropDown(_property.stringValue, filter, declaringType);
        }

        public void Draw()
        {
            var valueToRestore = EditorGUI.showMixedValue;
            EditorGUI.showMixedValue = _property.hasMultipleDifferentValues;
            DrawTypeSelectionControl();
            EditorGUI.showMixedValue = valueToRestore;
        }

        private static string GetTypeNameForField(string classRef)
        {
            // Remove assembly name and leave only type name.
            var classRefParts = classRef.Split(',');
            var typeName = classRefParts[0].Trim();

            if (typeName == string.Empty)
            {
                typeName = CachedTypeReference.NoneElement;
            }
            else if (CachedTypeReference.GetType(classRef) == null)
            {
                typeName += " {Missing}";
            }

            return typeName;
        }

        private void DrawTypeSelectionControl()
        {
            var controlID = GUIUtility.GetControlID(CachedTypeReference.ControlHint, FocusType.Keyboard, _position);
            _triggerDropDown = false;

            ReactToCurrentEvent(controlID);

            if ( ! _triggerDropDown)
                return;

            CachedTypeReference.SelectionControlID = controlID;
            CachedTypeReference.SelectedClassRef = _property.stringValue;

            _dropDown.Display(_position);
        }

        private void ReactToCurrentEvent(int controlID)
        {
            switch (Event.current.GetTypeForControl(controlID))
            {
                case EventType.ExecuteCommand:
                    if (Event.current.commandName == CachedTypeReference.ReferenceUpdatedCommandName)
                        OnTypeReferenceUpdated(controlID);

                    break;

                case EventType.MouseDown:
                    OnMouseDown(controlID);
                    break;

                case EventType.KeyDown:
                    OnKeyDown(controlID);
                    break;

                case EventType.Repaint:
                    DrawFieldContent(controlID);
                    break;
            }
        }

        private void OnMouseDown(int controlID)
        {
            if (!GUI.enabled || !_position.Contains(Event.current.mousePosition))
                return;

            GUIUtility.keyboardControl = controlID;
            _triggerDropDown = true;
            Event.current.Use();
        }

        private void OnKeyDown(int controlID)
        {
            if (GUI.enabled && GUIUtility.keyboardControl == controlID)
            {
                if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.Space)
                {
                    _triggerDropDown = true;
                    Event.current.Use();
                }
            }
        }

        private void DrawFieldContent(int controlID)
        {
            CachedTypeReference.FieldContent.text = GetTypeNameForField(_property.stringValue);
            EditorStyles.popup.Draw(_position, CachedTypeReference.FieldContent, controlID);
        }

        private void OnTypeReferenceUpdated(int controlID)
        {
            if (CachedTypeReference.SelectionControlID != controlID)
                return;

            if (_property.stringValue != CachedTypeReference.SelectedClassRef)
            {
                _property.stringValue = CachedTypeReference.SelectedClassRef;
                GUI.changed = true;
            }

            CachedTypeReference.SelectionControlID = 0;
            CachedTypeReference.SelectedClassRef = null;
        }
    }
}