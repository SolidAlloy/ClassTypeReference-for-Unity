namespace TypeReferences.Editor.TypeDropdown
{
  using UnityEditor;
  using UnityEngine;

  internal static class DropdownStyle
  {
    public const float NodeHeight = 23f;
    public const float LabelHeight = 16f;
    public const float GlobalOffset = 20f;
    public const float IndentWidth = 15f;
    public const float IconSize = 16f;
    public const float MaxWindowHeight = 600f;
    public const float SearchToolbarHeight = 22f;
    public const float MinWindowWidth = 253f;

    public static readonly GUIStyle NoPadding = new GUIStyle
    {
      padding = new RectOffset(0, 0, 0, 0)
    };

    public static readonly GUIStyle DefaultLabelStyle = new GUIStyle(EditorStyles.label)
    {
      margin = new RectOffset(0, 0, 0, 0)
    };

    public static readonly GUIStyle SelectedLabelStyle = new GUIStyle(EditorStyles.label)
    {
      margin = new RectOffset(0, 0, 0, 0),
      normal = { textColor = Color.white },
      onNormal = { textColor = Color.white }
    };

    public static readonly GUIStyle SearchToolbarStyle = new GUIStyle(GUI.skin.FindStyle("ToolbarSeachTextField"));

    private static readonly Color MouseOverColorDarkSkin = new Color(1f, 1f, 1f, 0.028f);
    private static readonly Color MouseOverColorLightSkin = new Color(1f, 1f, 1f, 0.3f);

    private static readonly Color SelectedColorDarkSkin = new Color(0.243f, 0.373f, 0.588f, 1f);
    private static readonly Color SelectedColorLightSkin = new Color(0.243f, 0.49f, 0.9f, 1f);

    private static readonly Color BorderColorDarkSkin = new Color(0.11f, 0.11f, 0.11f, 0.8f);
    private static readonly Color BorderColorLightSkin = new Color(0.38f, 0.38f, 0.38f, 0.6f);

    private static readonly Color BackgroundColorDarkSkin = new Color(0.192f, 0.192f, 0.192f, 1f);
    private static readonly Color BackgroundColorLightSkin = new Color(0.0f, 0.0f, 0.0f, 0.0f);

    private static readonly Color DarkSeparatorLineDarkSkin = new Color(0.11f, 0.11f, 0.11f, 0.258f);
    private static readonly Color DarkSeparatorLineLightSkin = new Color(0.0f, 0.0f, 0.0f, 0.065f);

    private static readonly Color LightSeparatorLineDarkSkin = new Color(1f, 1f, 1f, 0.033f);
    private static readonly Color LightSeparatorLineLightSkin = new Color(1f, 1f, 1f, 0.323f);

    public static Color MouseOverColor => DarkSkin ? MouseOverColorDarkSkin : MouseOverColorLightSkin;
    public static Color SelectedColor => DarkSkin ? SelectedColorDarkSkin : SelectedColorLightSkin;
    public static Color BorderColor => DarkSkin ? BorderColorDarkSkin : BorderColorLightSkin;
    public static Color BackgroundColor => DarkSkin ? BackgroundColorDarkSkin : BackgroundColorLightSkin;
    public static Color DarkSeparatorLine => DarkSkin ? DarkSeparatorLineDarkSkin : DarkSeparatorLineLightSkin;
    public static Color LightSeparatorLine => DarkSkin ? LightSeparatorLineDarkSkin : LightSeparatorLineLightSkin;

    public static bool DarkSkin => EditorGUIUtility.isProSkin;
  }
}