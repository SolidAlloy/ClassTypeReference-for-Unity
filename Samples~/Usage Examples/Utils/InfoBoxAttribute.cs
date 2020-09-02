namespace TypeReferences.Demo.Utils
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class InfoBoxAttribute : Attribute
    {
        public string Text { get; }

        public InfoBoxAttribute(string text)
        {
            Text = text;
        }
    }
}