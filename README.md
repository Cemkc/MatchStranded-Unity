# Match 2 Game System Details and Personal Notes

## System Details and Explanations

Before explaining the system, let me start by defining some key terms:
- **Tile Object**: Represents the main objects in the game (Match Objects, Harpoon, Glass, etc.).
- **Tile**: The container for Tile Objects, managing them.
- **Grid**: A rectangular area that creates and manages tiles.

### Level Design Tools - Level Editor Window (Unity Custom Editor Window)

The **Level Editor Window** is a custom Unity editor window designed to help modify the `Grid Blueprint` Scriptable Object, which serves as the foundation of a level. Thanks to its grid-based visualization, it allows easy editing of all tiles within a grid, making the level design process more efficient.

![Level Editor Window](/githubAssets/Images/ConfigMenu.png)

### Object Generation

This Match-2 game integrates two object generation methods:
- **Dynamic Object Generation**: Creates objects dynamically as needed and destroys them when they are no longer in use.
- **Object Pooling**: Pre-generates most objects at the beginning of the level, activating them when needed and deactivating them afterward.

The object generation method can be set by adding the desired generation component to the `TileObjectGenerator` GameObject in the scene. However, the pooling method currently encounters issues with generating and destroying the 'Duck' Tile Object, causing it to behave incorrectly. Other Tile Objects work as expected.

### Match Object Animations

The `TileMoveAnimation` script is a Scriptable Object that defines and stores the movement behavior of Tile Objects. By utilizing the `AnimationCurve` class, it enables the creation of generic animations. Examples of this can be found in the **Assets/ScriptableObjects** folder as `TileToGoalAnimation` and `TileFallAnimation`.

## Personal Notes
- While coding Tile Objects, I had uncertainties regarding how to properly apply **Object-Oriented Programming (OOP)**. For example, all Tile Objects inherit from the `TileObject` class, and while `TileObject` includes some **abstract** or **virtual** functions, it does not serve as a strict template for all Tile Objects. This means that each Tile Object can define its own unique behaviors and functions. Consequently, using Tile Objects requires **type casting**, which increases the chance of errors (even though flag checks were implemented for safety, avoiding dynamic type checks). Additionally, it makes the code harder to read.  

  An alternative approach, which I used in the `Tile` class, is to make the base class more **inclusive** by defining more abstract or virtual functions and handling everything through **polymorphism**. However, this has its own downside—some derived classes, like `TileAbsent`, may need to implement functions that are irrelevant to them, resulting in multiple empty function definitions.

- If I had more time, I would further improve the `LevelEditorWindow` custom editor. I believe this editor is the most critical part of the game because **easily designing levels is essential for maintaining long-term support**. Some planned improvements include:
  - The editor should not only define the initial grid but also allow the configuration of **falling objects from above**. Tools like "Generate the rest randomly" or "Add 5 red blocks" should be available for falling objects.
  - Allow **multiple tiles to be selected and assigned with a single click**.
  - **Render images of Tile Objects** within the editor.
