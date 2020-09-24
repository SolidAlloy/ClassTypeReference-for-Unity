namespace SolidUtilities.Editor.Helpers
{
    using System;
    using UnityEngine;

    /// <summary>Helps to create new textures.</summary>
    public static class TextureHelper
    {
        /// <summary>Temporarily sets <see cref="GL.sRGBWrite"/> to true, then executes the action.</summary>
        /// <param name="createTexture">
        /// Action that creates a texture while <see cref="GL.sRGBWrite"/> is set to true.
        /// </param>
        /// <example><code>
        /// TextureHelper.WithSRGBWrite(() =>
        /// {
        ///     GL.Clear(false, true, new Color(1f, 1f, 1f, 0f));
        ///     Graphics.Blit(Default, temporary, material);
        /// });
        /// </code></example>
        public static void WithSRGBWrite(Action createTexture)
        {
            bool previousValue = GL.sRGBWrite;
            GL.sRGBWrite = true;
            createTexture();
            GL.sRGBWrite = previousValue;
        }

        /// <summary>
        /// Creates a temporary texture, sets it as active in <see cref="RenderTexture.active"/>, executes an action,
        /// then returns the previous active texture.
        /// </summary>
        /// <param name="width">Width of the temporary texture in pixels.</param>
        /// <param name="height">Height of the temporary texture in pixels.</param>
        /// <param name="depthBuffer">Depth buffer of the temporary texture.</param>
        /// <param name="useTexture">Action that uses the temporary texture.</param>
        /// <example><code>
        /// TextureHelper.WithTemporaryActiveTexture(icon.width, icon.height, 0, temporary =>
        /// {
        ///     Graphics.Blit(icon, temporary, material);
        /// });
        /// </code></example>
        public static void WithTemporaryActiveTexture(int width, int height, int depthBuffer, Action<RenderTexture> useTexture)
        {
            WithTemporaryTexture(width, height, depthBuffer, temporary =>
            {
                RenderTexture previousActiveTexture = RenderTexture.active;
                RenderTexture.active = temporary;

                useTexture(temporary);

                RenderTexture.active = previousActiveTexture;
            });
        }

        /// <summary>Creates a temporary texture, executes an action that uses it, then removes it.</summary>
        /// <param name="width">Width of the temporary texture in pixels.</param>
        /// <param name="height">Height of the temporary texture in pixels.</param>
        /// <param name="depthBuffer">Depth buffer of the temporary texture.</param>
        /// <param name="useTexture">Action that uses the temporary texture.</param>
        /// <example><code>
        /// WithTemporaryTexture(icon.width, icon.height, 0, temporary =>
        /// {
        ///     Graphics.Blit(icon, temporary, material);
        /// });
        /// </code></example>
        public static void WithTemporaryTexture(int width, int height, int depthBuffer, Action<RenderTexture> useTexture)
        {
            RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer);
            useTexture(temporary);
            RenderTexture.ReleaseTemporary(temporary);
        }
    }
}