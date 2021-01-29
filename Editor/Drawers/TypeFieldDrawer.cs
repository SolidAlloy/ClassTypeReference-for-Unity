namespace TypeReferences.Editor.Drawers
{
    using System;
    using SolidUtilities.Editor.Helpers;
    using SolidUtilities.Helpers;
    using TypeReferences;
    using UnityEditor;
    using UnityEngine;
    using Util;
    using TypeCache = Util.TypeCache;

    /// <summary>
    /// Draws a <see cref="TypeReference"/> field and handles control over the drop-down list.
    /// </summary>
    internal class TypeFieldDrawer
    {
        private const string MissingSuffix = " {Missing}";
        private static readonly int _controlHint = typeof(TypeReferencePropertyDrawer).GetHashCode();
        private static readonly ContentCache _contentCache = new ContentCache();

        private readonly SerializedTypeReference _serializedTypeRef;
        private readonly TypeDropdownDrawer _dropdownDrawer;
        private readonly bool _showShortName;
        private readonly bool _useBuiltInNames;
        private readonly Rect _position;
        private readonly Action<Type> _onTypeSelected;

        private bool _triggerDropDown;

        public TypeFieldDrawer(
            SerializedTypeReference serializedTypeRef,
            Rect position,
            TypeDropdownDrawer dropdownDrawer,
            bool showShortName,
            bool useBuiltInNames,
            Action<Type> onTypeSelected = null,
            bool triggerDropDown = false)
        {
            _serializedTypeRef = serializedTypeRef;
            _position = position;
            _dropdownDrawer = dropdownDrawer;
            _showShortName = showShortName;
            _useBuiltInNames = useBuiltInNames;
            _onTypeSelected = onTypeSelected;
            _triggerDropDown = triggerDropDown;
        }

        public void Draw()
        {
            using (new EditorDrawHelper.MixedValue(_serializedTypeRef.TypeNameHasMultipleDifferentValues))
            {
                DrawTypeSelectionControl();
            }
        }

        private void DrawTypeSelectionControl()
        {
            int controlID = GUIUtility.GetControlID(_controlHint, FocusType.Keyboard, _position);
            ReactToCurrentEvent(controlID);

            if ( ! _triggerDropDown)
                return;

            _triggerDropDown = false;

            _dropdownDrawer.Draw(type =>
            {
                OnTypeSelected(type);
                _onTypeSelected?.Invoke(type);
            });
        }

        private void ReactToCurrentEvent(int controlID)
        {
            switch (Event.current.GetTypeForControl(controlID))
            {
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
            // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable
            bool mouseFocusedOnElement = GUI.enabled && _position.Contains(Event.current.mousePosition);
            if (! mouseFocusedOnElement)
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
            int indexOfComma = _serializedTypeRef.TypeNameAndAssembly.IndexOf(',');
            string fullTypeName = indexOfComma == -1 ? string.Empty : _serializedTypeRef.TypeNameAndAssembly.Substring(0, indexOfComma);
            GUIContent fieldContent = _contentCache.GetItem(GetTypeToShow(fullTypeName));
            EditorStyles.popup.Draw(_position, fieldContent, controlID);
        }

        private string GetTypeToShow(string typeName)
        {
            if (_useBuiltInNames)
            {
                string builtInName = typeName.ReplaceWithBuiltInName();
                if (builtInName != typeName)
                    return builtInName;
            }

            if (_showShortName)
                typeName = TypeNameFormatter.GetShortName(typeName);

            if (typeName == string.Empty)
                return TypeReference.NoneElement;

            if (TypeCache.GetType(_serializedTypeRef.TypeNameAndAssembly) == null)
                return typeName + MissingSuffix;

            return typeName;
        }

        private void OnTypeSelected(Type selectedType)
        {
            string selectedTypeNameAndAssembly = TypeReference.GetTypeNameAndAssembly(selectedType);

            if (_serializedTypeRef.TypeNameAndAssembly == selectedTypeNameAndAssembly)
                return;

            _serializedTypeRef.TypeNameAndAssembly = selectedTypeNameAndAssembly;
            GUI.changed = true;
        }
    }
}