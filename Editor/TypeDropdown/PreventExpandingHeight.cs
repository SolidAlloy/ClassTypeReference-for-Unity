namespace TypeReferences.Editor.TypeDropdown
{
    using UnityEngine;

    /// <summary>
    /// Represents an opposite of <see cref="GUILayout.ExpandHeight"/> and can be used as bool.
    /// </summary>
    internal class PreventExpandingHeight
    {
        private readonly bool _preventExpanding;
        private readonly GUILayoutOption _option;

        public PreventExpandingHeight(bool preventExpanding)
        {
            _preventExpanding = preventExpanding;
            _option = GUILayout.ExpandHeight( ! preventExpanding);
        }

        public static implicit operator GUILayoutOption(PreventExpandingHeight option)
        {
            return option._option;
        }

        public static implicit operator bool(PreventExpandingHeight option)
        {
            return option._preventExpanding;
        }
    }
}