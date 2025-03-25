# MLock Car Control Example

This example demonstrates how to use the MLock system to control access to game functionality.

## Overview

In this example:
- A car is placed on a plane that the user can control with keyboard inputs.
- A static camera observes the scene.
- A menu button opens a popup menu UI.
- When the menu is open, the car controls are locked using MLock.
- When the menu closes, car controls are unlocked.
- The menu contains a language selection button that opens a tooltip with language options.
- Opening the language tooltip locks the main menu interaction.
- Selecting a language closes the tooltip and unlocks the main menu.

## Setup Instructions

1. Create a new scene or open the example scene.
2. Create a plane for the car to drive on.
3. Add a car model to the scene.
4. Add a static camera that views the plane.
5. Add an empty GameObject and attach the following components:
   - `UIDocument` (for UI)
   - `MLockCarUIBuilder` (builds the UI programmatically)
   - `MLockCarExample` (main example script)
6. Add the `CarController` component to your car model.
7. Assign the car controller reference in the `MLockCarExample` component.
8. Assign the UI Document reference in the `MLockCarExample` component.

## How MLock Is Used

The example shows three key MLock features:

1. **Using the `using` statement for automatic lock disposal**: 
   ```csharp
   using var _ = _lockService.Lock(CarLockTags.PlayerControl);
   ```
   This ensures that locks are automatically disposed when they go out of scope, unlocking the components.

2. **Tag-based locking**: Different components can be locked based on their tags (`CarLockTags`).

3. **Nested locks**: The language selection tooltip can be opened from the menu, demonstrating nested locking behavior.

## Code Structure

- `CarController.cs` - Shared component that handles car movement and implements `ILockable<CarLockTags>`
- `MLockCarExample.cs` - Main example script that manages UI interaction and MLock locking
- `MLockCarUIBuilder.cs` - Builds the UI programmatically using UIElements
- `CarLockTags.cs` - Flag enum defining lock tags for different components

## Controls

- Arrow keys or WASD - Move the car
- Menu button - Open/close the menu
- X button - Close the menu
- Select Language - Open language tooltip
- Language options - Select language and close tooltip 