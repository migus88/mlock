# Car Example with UI Toolkit (UI Elements)

This example demonstrates how to use the `MLock` system in a simple application with Unity's UI Toolkit (UI Elements).

## Overview

The example showcases a simple car scene with a menu system that uses `MLock` to manage UI interaction and prevent unwanted input while different menus are open. 

## Key Components

- **CarExample.cs**: Main controller script that manages the car UI and demonstrates `MLock` usage
- **CarExampleUiBuilder.cs**: Creates the entire UI programmatically using UI Toolkit
- **Custom Controls**: The example includes lockable UI controls (in the Controls folder) that implement the ILockable interface

## How It Works

1. The UI is built entirely through code in `CarExampleUiBuilder.cs`
2. The example uses `MLock` to manage different UI states:
   - When opening the main menu, it locks player input and game HUD
   - When opening the language tooltip, it locks the main menu
   - The "Temp Lock" button demonstrates locking all UI temporarily

## How Lockables Are Created

Lockables are implemented by extending UI Toolkit controls and implementing the `ILockable<T>` interface:

```csharp
public class LockableButton : Button, ILockable<CarLockTags>, IDisposable
{
    public CarLockTags LockTags { get; }

    public LockableButton(CarLockTags lockTags)
    {
        LockTags = lockTags;
        CarLockService.Instance.Subscribe(this);
    }

    public void HandleLocking()
    {
        SetEnabled(false);
    }

    public void HandleUnlocking()
    {
        SetEnabled(true);
    }

    public void Dispose()
    {
        CarLockService.Instance.Unsubscribe(this);
    }
}
```

Key aspects:
1. Inherit from a UI Toolkit control (Button, Slider, etc.)
2. Implement the `ILockable<CarLockTags>` interface and `IDisposable`
3. Accept lock tags in the constructor
4. Subscribe to the lock service in the constructor
5. Unsubscribe in the Dispose method
6. Implement lock/unlock behavior in HandleLocking/HandleUnlocking methods

These controls are used in the UI builder:

```csharp
// Creating a lockable button
var menuButton = new LockableButton(CarLockTags.GameHud) { text = "Menu", name = MenuButtonName };
```

## Disclaimer

This example was created with a focus on demonstrating `MLock` functionality, not on performance or coding best practices. 

**NOT RECOMMENDED FOR PRODUCTION:**
- The use of Singletons (`CarLockService.Instance`)
- Building the entire UI programmatically instead of using UXML/USS
- Other simplifications made for clarity

In a real application, you should consider using dependency injection or other appropriate patterns for managing services and references. 