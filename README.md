Type References for Unity3D
======
[![openupm](https://img.shields.io/npm/v/com.solidalloy.type.references?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.solidalloy.type.references/)


A class that provides serializable references to `System.Type` with an accompanying custom property drawer that allows type selection from drop-down.

![screenshot](https://raw.githubusercontent.com/SolidAlloy/ClassTypeReference-for-Unity/master/.screenshot.png)

This is a fork of the currently inactive project by Rotorz: [ClassTypeReference for Unity](https://bitbucket.org/rotorz/classtypereference-for-unity/src/master/)
## Install with OpenUPM
Once you have the OpenUPM cli, run the following command:

```openupm install com.solidalloy.type.references```

Or if you don't have it, add the scoped registry to manifest.json with the desired dependency semantic version: 
```
  "scopedRegistries": [
    {
      "name": "package.openupm.com",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.solidalloy.util",
        "com.solidalloy.type.references",
        "com.openupm"
      ]
    }
  ],
  "dependencies": {
    "com.solidalloy.type.references": "2.1.0"
  },

```

## Install via Git URL

Project supports Unity Package Manager. To install the project as a Git package do the following:

1. In Unity, open **Window** -> **Package Manager**.
2. Press the **+** button, choose "**Add package from git URL...**"
3. Enter "https://github.com/SolidAlloy/SolidUtilities.git" and press **Add**.
4. When the first package is installed, add the main one: "https://github.com/SolidAlloy/ClassTypeReference-for-Unity.git".

## Simple Usage

Types can be assigned via inspector simply by using `TypeReference`:

```csharp
using TypeReferences;

public class ExampleBehaviour: MonoBehaviour
{
    [SerializeField] private TypeReference greetingLoggerType;
}
```

&nbsp;  

Usually, you would want to choose between two or three classes that extend a common parent class or implement certain interface. Use **[Inherits]** attribute for such cases:

```csharp
using TypeReferences;

public class ExampleBehaviour: MonoBehaviour
{
    [Inherits(typeof(IGreetingLogger))]
    public TypeReference greetingLoggerType;
    
    [Inherits(typeof(MonoBehaviour))]
    public TypeReference onlyMonoBehaviours;
}
```

&nbsp;  

TypeReference can be used in place of `System.Type` most of the time:

```csharp
TypeReference greetingLoggerType = typeof(DefaultGreetingLogger);
var logger = (IGreetingLogger) System.Activator.CreateInstance(greetingLoggerType);
logger.LogGreeting();
```

But if you need to refer to the `System.Type` object directly, use the **Type** property:

```csharp
bool isLoggerAbstract = greetingLoggerType.Type.IsAbstract;
```

## TypeOptions Attribute

If you need to customize the look of the drop-down menu or change what types are included in the list, use the `[TypeOptions]` attribute.

Presentation of drop-down list can be customized with the `Grouping` enum:

- **Grouping.None** - No grouping, just show type names in a list; for instance, "Some.Nested.Namespace.SpecialClass".

- **Grouping.ByNamespace** - Group classes by namespace and show foldout menus for nested namespaces; for instance, "Some > Nested > Namespace > SpecialClass".

- **Grouping.ByNamespaceFlat** ***(default)*** - Group classes by namespace; for instance, "Some.Nested.Namespace > SpecialClass".

- **Grouping.ByAddComponentMenu** - Group classes in the same way as Unity does for its component menu. This grouping method must only be used for `MonoBehaviour` types.

For instance,

  ```csharp
[TypeOptions(Grouping = Grouping.ByAddComponentMenu)]
public TypeReference greetingLoggerType;
  ```

  &nbsp;  

There are situations when you need to include a few types in the drop-down menu or exclude some of the listed types. Use `IncludeTypes` and `ExcludeTypes` for this:

```csharp
[Inherits(typeof(IGreetingLogger), IncludeTypes = new[] { MonoBehaviour })]
public TypeReference greetingLoggerType;

[TypeOptions(ExcludeTypes = new[] { DebugModeClass, TestClass })]
public TypeReference productionType;
```

&nbsp;  

You can exclude **(None)** so that no one can choose it from the dropdown.

```csharp
[TypeOptions(ExcludeNone = true)]
public TypeReference greetingLogger;
```

Note that the type can still be null by default or if set through code.

&nbsp;  

By default, only the types the class can reference directly are included in the drop-down list.

```csharp
public class ExampleBehaviour
{
    // If this gives an error because it cannot find CustomPlugin type
    public CustomPlugin plugin;
    
    // Then CustomPlugin will not be in the drop-down.
    public TypeReference pluginType;
}
```

You might need to add a reference to the assembly where `CustomPlugin` is located to make it appear in the drop-down menu. However, if it is not possible or you just need to test out some things, there is an option to include assemblies your class does not have access to - `IncludeAdditionalAssemblies`. Use it like this:

```csharp
[Inherits(typeof(IAttribute), IncludeAdditionalAssemblies = new[] { "Assembly-CSharp" })]
public TypeReference attribute;
```

&nbsp;  

If you are not satisfied with the auto-adjusted height, you can set the custom one with the DropdownHeight option. Use it like this: 

```csharp
[Inherits(typeof(IGreetingLogger), DropdownHeight = 300)]
public TypeReference greetingLoggerType;
```

&nbsp;  

By default, folders are closed. If you want them all to be expanded when you open the dropdown, use ExpandAllFolders = true:

```csharp
[TypeOptions(ExpandAllFolders = true)]
public TypeReference allTypes;
```

&nbsp;  

By default, the searchbar appears when you have more than 10 types in the dropdown list. You can change this behaviour with the SearchbarMinItemsCount option. Here we used SearchbarMinItemsCount = 0 to make searchbar appear even though there are less than 10 types to choose from.

```csharp
[Inherits(typeof(IGreetingLogger), SearchbarMinItemsCount = 0)]
public TypeReference greetingLoggerType;
```

&nbsp;  

You can make the field show just the type name without its namespace. For example, in this case, the field will show *DefaultGreetingLogger* instead of *TypeReferences.Demo.Utils.DefaultGreetingLogger*:

```csharp
[Inherits(typeof(IGreetingLogger), ShortName = true)] public TypeReference GreetingLoggerType;
```

&nbsp;  

By default, the field shows built-in types by their keyword name instead of the full name (e.g. `int` instead of `System.Int32`). You can change this by setting the *UseBuiltInNames* option to false.

&nbsp;  

The *SerializableOnly* option allows you to show only the classes that can be serialized by Unity. It is useful when creating custom generic classes using the types selected from the dropdown. It is a [new feature in Unity 2020](https://unity.com/releases/2020-1/programmer-tools#create-fields-generic-types-directly).

```csharp
[SerializeField, TypeOptions(SerializableOnly = true)]
private TypeReference serializableTypes;
```

&nbsp;  

## Inherits Attribute

This attribute allows you to choose only from the classes that implement a certain interface or extend a class. It has all the arguments `TypeOptions` provides.

```csharp
[Inherits(typeof(IGreetingLogger))]
public TypeReference greetingLoggerType;

[Inherits(typeof(MonoBehaviour))]
public TypeReference onlyMonoBehaviours;

// All the TypeOptions arguments are available with Inherits too.
[Inherits(typeof(IGreetingLogger), ExcludeNone = true)]
public TypeReference greetingLoggerType;
```

&nbsp;  

If you need to have the base type in the drop-down menu too, use `IncludeBaseType`

```csharp
[Inherits(typeof(MonoBehaviour), IncludeBaseType = true)]
public TypeReference onlyMonoBehaviours;
```

&nbsp;  

By default, abstract types (abstract classes and interfaces) are not included in the drop-down list. However, you can allow them:

```csharp
[Inherits(typeof(IGreetingLogger), AllowAbstract = true)]
public TypeReference greetingLoggerType;
```

  

Contribution Agreement
----------------------

This project is licensed under the MIT license (see LICENSE). To be in the best
position to enforce these licenses the copyright status of this project needs to
be as simple as possible. To achieve this the following terms and conditions
must be met:

- All contributed content (including but not limited to source code, text,
  image, videos, bug reports, suggestions, ideas, etc.) must be the
  contributors own work.

- The contributor disclaims all copyright and accepts that their contributed
  content will be released to the public domain.

- The act of submitting a contribution indicates that the contributor agrees
  with this agreement. This includes (but is not limited to) pull requests, issues,
  tickets, e-mails, newsgroups, blogs, forums, etc.

