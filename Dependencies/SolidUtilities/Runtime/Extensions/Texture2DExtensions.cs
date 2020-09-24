namespace SolidUtilities.Extensions
{
    using UnityEngine;

    /// <summary>Different useful extensions for <see cref="Texture2D"/></summary>
    public static class Texture2DExtensions
    {
        /// <summary>Rotates the input texture by 90 degrees and returns the new rotated texture.</summary>
        /// <param name="texture">Texture to rotate.</param>
        /// <param name="clockwise">Whether to rotate the texture clockwise.</param>
        /// <returns>The rotated texture.</returns>
        /// <example><code>EditorIcon TriangleDown = new EditorIcon(Database.TriangleRight.Rotate());</code></example>
        public static Texture2D Rotate(this Texture2D texture, bool clockwise = true)
        {
            var original = texture.GetPixels32();
            var rotated = new Color32[original.Length];
            int textureWidth = texture.width;
            int textureHeight = texture.height;
            int origLength = original.Length;

            for (int heightIndex = 0; heightIndex < textureHeight; ++heightIndex)
            {
                for (int widthIndex = 0; widthIndex < textureWidth; ++widthIndex)
                {
                    int rotIndex = (widthIndex + 1) * textureHeight - heightIndex - 1;

                    int origIndex = clockwise
                        ? origLength - 1 - (heightIndex * textureWidth + widthIndex)
                        : heightIndex * textureWidth + widthIndex;

                    rotated[rotIndex] = original[origIndex];
                }
            }

            var rotatedTexture = new Texture2D(textureHeight, textureWidth);
            rotatedTexture.SetPixels32(rotated);
            rotatedTexture.Apply();
            return rotatedTexture;
        }

        /// <summary>Draws the texture in a given rect.</summary>
        /// <param name="texture">The texture to draw.</param>
        /// <param name="rect">Rectangle in which to draw the texture.</param>
        /// <example><code>tintedIcon.Draw(triangleRect);</code></example>
        public static void Draw(this Texture2D texture, Rect rect) => GUI.DrawTexture(rect, texture);
    }
}