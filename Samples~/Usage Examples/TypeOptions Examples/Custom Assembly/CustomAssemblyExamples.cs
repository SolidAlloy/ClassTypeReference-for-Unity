namespace TypeReferences.Demo.TypeOptions_Examples
{
    using System;
    using TypeReferences;

    [Serializable]
    public struct NoAttributeStruct
    {
        public TypeReference NoAttribute;
    }

    [Serializable]
    public struct DefaultAssemblyIncludedStruct
    {
        [TypeOptions(IncludeAdditionalAssemblies = new[] { "Assembly-CSharp" })] public TypeReference DefaultAssemblyIncluded;
    }
}
