using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JoeConticello.Characters2D
{
    [DisallowMultipleComponent]
    public sealed class UnityInputSystemCharacterInputSource : MonoBehaviour, ICharacterInputSource
    {
        [Serializable]
        public sealed class NamedActionBinding
        {
            [SerializeField] private string actionId = "Action";
            [SerializeField] private InputActionReference action;

            public string ActionId => actionId;
            public InputAction Action => action != null ? action.action : null;
        }

        [Header("Core Axes")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference aimAction;

        [Header("Aim")]
        [SerializeField] private Camera aimCamera;
        [SerializeField] private bool aimActionIsScreenPosition = true;

        [Header("Custom Actions")]
        [SerializeField] private List<NamedActionBinding> actions = new List<NamedActionBinding>();

        private uint tick;

        public CharacterInputFrame CaptureInput()
        {
            Vector2 move = ReadMove();
            Vector2 aimWorld = ReadAimWorld();

            List<string> pressed = new List<string>(actions.Count);
            List<string> held = new List<string>(actions.Count);

            for (int i = 0; i < actions.Count; i++)
            {
                NamedActionBinding binding = actions[i];
                if (binding == null || string.IsNullOrWhiteSpace(binding.ActionId) || binding.Action == null)
                    continue;

                if (binding.Action.WasPressedThisFrame())
                    pressed.Add(binding.ActionId);

                if (binding.Action.IsPressed())
                    held.Add(binding.ActionId);
            }

            tick++;
            return new CharacterInputFrame(move, aimWorld, pressed.ToArray(), held.ToArray(), tick);
        }

        private Vector2 ReadMove()
        {
            InputAction action = moveAction != null ? moveAction.action : null;
            return action != null ? Vector2.ClampMagnitude(action.ReadValue<Vector2>(), 1f) : Vector2.zero;
        }

        private Vector2 ReadAimWorld()
        {
            Camera cam = aimCamera != null ? aimCamera : Camera.main;
            InputAction action = aimAction != null ? aimAction.action : null;

            if (action == null)
                return (Vector2)transform.position;

            Vector2 aimValue = action.ReadValue<Vector2>();

            if (!aimActionIsScreenPosition || cam == null)
                return aimValue;

            Vector3 world = cam.ScreenToWorldPoint(new Vector3(aimValue.x, aimValue.y, Mathf.Abs(cam.transform.position.z)));
            return world;
        }
    }
}