namespace TypeReferences.Editor
{
    using UnityEditor;
    using UnityEngine;

    internal class TypeFieldDrawer
    {
        private const string MissingSuffix = " {Missing}";

        private readonly SerializedClassTypeReference _serializedTypeRef;
        private readonly TypeDropDownDrawer _dropDownDrawer;
        private Rect _position;
        private bool _triggerDropDown;

        public TypeFieldDrawer(
            SerializedClassTypeReference serializedTypeRef,
            Rect position,
            TypeDropDownDrawer dropDownDrawer)
        {
            _serializedTypeRef = serializedTypeRef;
            _position = position;
            _dropDownDrawer = dropDownDrawer;
        }

        public void Draw()
        {
            bool valueToRestore = EditorGUI.showMixedValue;
            EditorGUI.showMixedValue = _serializedTypeRef.TypeNameHasMultipleDifferentValues;
            DrawTypeSelectionControl();
            EditorGUI.showMixedValue = valueToRestore;
        }

        private void DrawTypeSelectionControl()
        {
            int controlID = GUIUtility.GetControlID(
                CachedTypeReference.ControlHint,
                FocusType.Keyboard,
                _position);

            _triggerDropDown = false;

            ReactToCurrentEvent(controlID);

            if ( ! _triggerDropDown)
                return;

            CachedTypeReference.SelectionControlID = controlID;
            CachedTypeReference.SelectedTypeNameAndAssembly = _serializedTypeRef.TypeNameAndAssembly;

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
            bool keyboardFocusIsOnElement = GUI.enabled && GUIUtility.keyboardControl == controlID;

            bool necessaryKeyIsDown =
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
            CachedTypeReference.FieldContent.text = GetTypeNameForField();
            EditorStyles.popup.Draw(_position, CachedTypeReference.FieldContent, controlID);
        }

        private string GetTypeNameForField()
        {
            var typeParts = _serializedTypeRef.TypeNameAndAssembly.Split(',');
            string typeName = typeParts[0].Trim();

            if (typeName == string.Empty)
            {
                typeName = ClassTypeReference.NoneElement;
            }
            else if (CachedTypeReference.GetType(_serializedTypeRef.TypeNameAndAssembly) == null)
            {
                _serializedTypeRef.TryUpdatingTypeUsingGUID();

                if (CachedTypeReference.GetType(_serializedTypeRef.TypeNameAndAssembly) == null)
                    typeName += MissingSuffix;
            }

            return typeName;
        }

        private void OnTypeReferenceUpdated(int controlID)
        {
            if (CachedTypeReference.SelectionControlID != controlID)
                return;

            if (_serializedTypeRef.TypeNameAndAssembly != CachedTypeReference.SelectedTypeNameAndAssembly)
            {
                _serializedTypeRef.TypeNameAndAssembly = CachedTypeReference.SelectedTypeNameAndAssembly;
                GUI.changed = true;
            }

            CachedTypeReference.SelectionControlID = 0;
            CachedTypeReference.SelectedTypeNameAndAssembly = null;
        }
    }
}