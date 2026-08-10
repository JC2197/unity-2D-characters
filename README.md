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

## Local Movement Setup

For a non-networked player, add these components to the player GameObject:

- `Rigidbody2D`
- `TopDownCharacterMotor`
- `UnityInputSystemCharacterInputSource`
- `CharacterMotorDriver`
- `PlayerInput`

Assign a `Vector2` Move action to the input source. `CharacterMotorDriver` captures the enabled Input System actions every frame and simulates the motor. For a FishNet player, use `FishNetCharacterMotorDriver` instead; it simulates only on the local network owner.

## Movement Animation

`TopDownCharacterMotor` can update an `Animator` bool parameter named `IsMoving` whenever its velocity is non-zero. It finds an Animator on the player or its children automatically, or you can assign one directly.

Create a base Animator Controller with `Idle` and `Move` states, then make transitions between them conditional on `IsMoving` being false and true respectively. Assign an `AnimatorOverrideController` based on that controller to each character Animator and override its `Idle` and `Move` clips. The motor continues to drive the same parameter for every character.
