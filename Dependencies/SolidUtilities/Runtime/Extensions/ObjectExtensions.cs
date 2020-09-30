namespace SolidUtilities.Extensions
{
    using System;

    /// <summary> Different useful extensions for <see cref="object"/>.</summary>
    internal static class ObjectExtensions
    {
        /// <summary>
        /// Casts <paramref name="obj"/> to <typeparamref name="T"/> or returns a default instance of
        /// <typeparamref name="T"/> if the cast fails.
        /// </summary>
        /// <param name="obj">The object to cast.</param>
        /// <typeparam name="T">The type to cast to.</typeparam>
        /// <returns>The cast object of type <typeparamref name="T"/> or default instance.</returns>
        public static T CastOrDefault<T>(this object obj)
        {
            try
            {
                return (T) obj;
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }
    }
}