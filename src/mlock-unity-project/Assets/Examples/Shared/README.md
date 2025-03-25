# MLock Unity Examples - Shared Components

This folder contains shared components used by both MLock Unity examples (Canvas and UI Toolkit).

## Key Components

### Code

- **CarLockTags.cs**: Enum defining the lockable areas of the application:
  - `PlayerInput`: Controls player input
  - `GameHud`: UI elements for game HUD
  - `MainMenu`: UI elements for the main menu

- **LockService.cs**: Base implementation of the lock service with singleton pattern
  - Manages lock registration and status checking
  - Provides methods for locking/unlocking different parts of the UI

- **CarLockService.cs**: Specific implementation of LockService for the car examples

- **CarController.cs**: Simple car controller with movement logic

### Prefabs and Materials

Contains shared assets used by both examples.

## Disclaimer

These shared components were created with a focus on demonstrating MLock functionality, not on performance or coding best practices.

**NOT RECOMMENDED FOR PRODUCTION:**
- The use of Singletons (`CarLockService.Instance`)
- Other simplifications made for clarity

In a real application, you should consider using dependency injection or other appropriate patterns for managing services and references. 