namespace TypeReferences.Editor
{
    using UnityEditor;
    using UnityEngine;

    internal class TypeFieldDrawer
    {
        private readonly SerializedClassTypeReference _serializedClassTypeReference;
        private readonly TypeDropDownDrawer _dropDownDrawer;
        private Rect _position;
        private bool _triggerDropDown;

        public TypeFieldDrawer(
            SerializedClassTypeReference serializedClassTypeReference,
            Rect position,
            TypeDropDownDrawer dropDownDrawer)
        {
            _serializedClassTypeReference = serializedClassTypeReference;
            _position = position;
            _dropDownDrawer = dropDownDrawer;
        }

        public void Draw()
        {
            var valueToRestore = EditorGUI.showMixedValue;
            EditorGUI.showMixedValue = _serializedClassTypeReference.TypeNameHasMultipleDifferentValues;
            DrawTypeSelectionControl();
            EditorGUI.showMixedValue = valueToRestore;
        }

        private static string GetTypeNameForField(string typeNameAndAssembly)
        {
            // Remove assembly name and leave only type name.
            var typeParts = typeNameAndAssembly.Split(',');
            var typeName = typeParts[0].Trim();

            if (typeName == string.Empty)
            {
                typeName = ClassTypeReference.NoneElement;
            }
            else if (CachedTypeReference.GetType(typeNameAndAssembly) == null)
            {
                typeName += " {Missing}";
            }

            return typeName;
        }

        private void DrawTypeSelectionControl()
        {
            var controlID = GUIUtility.GetControlID(
                CachedTypeReference.ControlHint,
                FocusType.Keyboard,
                _position);

            _triggerDropDown = false;

            ReactToCurrentEvent(controlID);

            if ( ! _triggerDropDown)
                return;

            CachedTypeReference.SelectionControlID = controlID;
            CachedTypeReference.SelectedTypeNameAndAssembly = _serializedClassTypeReference.TypeNameAndAssembly;

            _dropDownDrawer.Draw(_position);
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
            var keyboardFocusIsOnElement = GUI.enabled && GUIUtility.keyboardControl == controlID;

            var necessaryKeyIsDown =
                Event.current.keyCode == KeyCode.Return
                || Event.current.keyCode == KeyCode.Space;

            if (keyboardFocusIsOnElement && necessaryKeyIsDown)
            {
                _triggerDropDown = true;
                Event.current.Use();
            }
        }

        private void DrawFieldContent(int controlID)
        {
            CachedTypeReference.FieldContent.text = GetTypeNameForField(_serializedClassTypeReference.TypeNameAndAssembly);
            EditorStyles.popup.Draw(_position, CachedTypeReference.FieldContent, controlID);
        }

        private void OnTypeReferenceUpdated(int controlID)
        {
            if (CachedTypeReference.SelectionControlID != controlID)
                return;

            if (_serializedClassTypeReference.TypeNameAndAssembly != CachedTypeReference.SelectedTypeNameAndAssembly)
            {
                _serializedClassTypeReference.TypeNameAndAssembly = CachedTypeReference.SelectedTypeNameAndAssembly;
                GUI.changed = true;
            }

            CachedTypeReference.SelectionControlID = 0;
            CachedTypeReference.SelectedTypeNameAndAssembly = null;
        }
    }
}