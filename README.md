# unity-2d-characters

Monorepo containing reusable Unity 2D character packages.

## Packages

- `com.joeconticello.characters2d`: Core 2D motor, aim, and input abstraction. No networking dependency.
- `com.joeconticello.characters2d.fishnet`: FishNet networking adapter for server-authoritative play with prediction/reconciliation hooks.

## Why two packages?

This keeps gameplay code reusable in offline/local games while networking remains an optional integration layer.

## Input Model

The core package uses Unity Input System and sends generic action IDs in each input frame:

- `PressedActionIds`: actions pressed this frame
- `HeldActionIds`: actions currently held

Configure any number of actions in `UnityInputSystemCharacterInputSource` by adding named action bindings (for example: `Jump`, `Interact`, `Ability1`, `Ability2`, `Reload`).
