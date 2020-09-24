namespace SolidUtilities
{
    using System;
    using System.Diagnostics;
    using Debug = UnityEngine.Debug;

    /// <summary>
    /// Basic timer that logs execution time of a method or part of the method. It does not warm up the execution and
    /// runs the actions only once.
    /// </summary>
    public static class Timer
    {
        /// <summary>Log time in ms the action took.</summary>
        /// <param name="actionName">Name of the action which execution is measured.</param>
        /// <param name="action">Action to execute.</param>
        /// <example><code>
        /// LogTime("Show popup", () =>
        /// {
        ///     var dropdownWindow = new DropdownWindow(rect);
        ///     dropdownWindow.ShowInPopup();
        /// });
        /// </code></example>
        public static void LogTime(string actionName, Action action)
        {
            var stopWatch = Stopwatch.StartNew();
            action();
            stopWatch.Stop();
            Debug.Log($"{actionName} took {Convert.ToInt32(stopWatch.ElapsedMilliseconds)} ms.");
        }
    }
}