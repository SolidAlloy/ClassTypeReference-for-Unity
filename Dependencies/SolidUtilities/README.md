# SolidUtilities
Different utilities that simplify development in Unity3D. They include extensions for common Unity structures such as Rect and Texture2D, different methods that make it easier to develop custom interfaces and property drawers, etc.

You can read the full documentation on the repository [Wiki](https://github.com/SolidAlloy/unity-util/wiki).

I'm adding new utilities as I write code for other Unity3D projects and discover some common patterns/methods I use throughout the codebase.

#### Projects that use SolidUtilities:

- [ClassTypeReference-for-Unity](https://github.com/SolidAlloy/ClassTypeReference-for-Unity)

# The most useful utilities

## RectExtensions

### AddHorizontalPadding(rect, leftPadding, rightPadding)

Creates padding to the left and right of a rectangle by narrowing it down.

| Name | Description |
| ---- | ----------- |
| rect | *UnityEngine.Rect*<br>The bigger rect to create padding for. |
| leftPadding | *System.Single*<br>Width of the left padding in pixels. |
| rightPadding | *System.Single*<br>Width of the right padding in pixels. |

#### Returns

The smaller rect that appeared after creating paddings.

#### Example


```
Rect innerToolbarArea = outerToolbarArea.AddHorizontalPadding(10f, 2f);
```


### AlignMiddleVertically(rect, height)

Places a rect with a smaller height vertically in the middle of a bigger rect.

| Name | Description |
| ---- | ----------- |
| rect | *UnityEngine.Rect*<br>The bigger rect. |
| height | *System.Single*<br>The height of a smaller rect. |

#### Returns

The smaller rect with a given height that was aligned vertically in the middle of a bigger rect.

#### Example


```
Rect innerToolbarArea = outerToolbarArea.AlignMiddleVertically(DropdownStyle.LabelHeight);
```


### CutVertically(originalRect, cutDistance, fromRightBorder)

Cuts a big rect into two smaller ones by placing a vertical cut at cutDistance from the left or right border of the rect.

| Name | Description |
| ---- | ----------- |
| originalRect | *UnityEngine.Rect*<br>The rect that should be split. |
| cutDistance | *System.Single*<br>The distance from the left or right border of the rect where to place vertical cut. |
| fromRightBorder | *System.Boolean*<br>Whether to count the distance from left or right border. |

#### Returns

Left and right rects that appeared after the cut.

#### Example


```
(Rect searchFieldArea, Rect buttonArea) = innerToolbarArea.CutVertically(DropdownStyle.IconSize, true);
```


### RoundUpCoordinates(rect)

Rounds up x, y, width, and height of the rect.

| Name | Description |
| ---- | ----------- |
| rect | *UnityEngine.Rect@*<br>Rect to round coordinates for. |

#### Example


```
popupArea.RoundUpCoordinates();
```

## FuzzySearch

Implementation of the fuzzy search algorithm.

### CanBeIncluded(searchString, itemName, score)

Determines if an item should be included in the search result and outputs its score (its position in the list of matching items.)

| Name | Description |
| ---- | ----------- |
| searchString | *System.String*<br>Search string to compare the item to. |
| itemName | *System.String*<br>Name of the item to include in the result list. |
| score | *System.Int32@*<br>Score of the item that determines how high in the result list it should be placed. |

#### Returns

Whether to include the result in the list.

#### Example


```
EnumerateTree()
    .Where(node => node.Type != null)
    .Select(node =>
    {
        bool includeInSearch = FuzzySearch.CanBeIncluded(_searchString, node.FullTypeName, out int score);
        return new { score, item = node, include = includeInSearch };
    })
    .Where(x => x.include)
    .OrderByDescending(x => x.score)
    .Select(x => x.item));
```
## DrawHelper

Different useful methods that simplify <a href="#unityengine.guilayout">UnityEngine.GUILayout</a> API.

### DrawHorizontally(drawContent)

Draws content in the horizontal direction.

| Name | Description |
| ---- | ----------- |
| drawContent | *System.Action*<br>Action that draws the content. |

#### Example


```
DrawHelper.DrawHorizontally(() =>
{
    selectedValue = DrawSelectorDropdownAndGetSelectedValue();

    if (Event.current.type == EventType.Repaint)
        DrawHamburgerMenuButton();
});
```


### DrawVertically(drawContent)

Draws content in the vertical direction.

| Name | Description |
| ---- | ----------- |
| drawContent | *System.Action*<br>Action that draws the content. |

#### Example


```
DrawHelper.DrawVertically(() =>
{
    EditorDrawHelper.DrawInfoMessage("No types to select.");
});
```


