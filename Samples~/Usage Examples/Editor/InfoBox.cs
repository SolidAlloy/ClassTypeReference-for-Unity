namespace TypeReferences.Demo.Editor
{
    using UnityEditor;
    using UnityEngine;

    internal static class InfoBox
    {
        public static void Draw(string text)
        {
            // Used to add some margin between the the HelpBox and the property.
            const float marginHeight = 2f;

            Rect infoBoxArea = EditorGUILayout.GetControlRect(false, GetInfoBoxHeight(text, marginHeight));
            infoBoxArea.height -= marginHeight;
            EditorGUI.HelpBox(infoBoxArea, text, MessageType.Info);
        }
        
        private static float GetInfoBoxHeight(string text, float marginHeight)
        {
            // This stops icon shrinking if text content doesn't fill out the container enough.
            const float minHeight = 40f;

            // currentViewWidth is the whole width of Inspector. Text box width is smaller than that.
            float textBoxWidth = EditorGUIUtility.currentViewWidth - 65f;

            var content = new GUIContent(text);
            var style = GUI.skin.GetStyle("helpbox");

            float height = style.CalcHeight(content, textBoxWidth);

            // We add tiny padding here to make sure the text is not overflowing the HelpBox from the top
            // and bottom.
            height += marginHeight * 2;

            return height > minHeight ? height : minHeight;
        }
    }
}