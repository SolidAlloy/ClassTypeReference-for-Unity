namespace TypeReferences.Demo.Utils
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class InfoBoxAttribute : Attribute
    {
        public string Text { get; }

        public InfoBoxAttribute(string text)
        {
            Text = text;
        }
    }
}