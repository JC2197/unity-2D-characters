using UnityEngine;
using UnityEngine.InputSystem;
using JoeConticello.Characters2D;

#if FISHNET
using FishNet.Object;
#endif

namespace JoeConticello.Characters2D.FishNet
{
#if FISHNET
    [DisallowMultipleComponent]
    public sealed class FishNetCharacterMotorDriver : NetworkBehaviour
    {
        [SerializeField] private TopDownCharacterMotor motor;
        [SerializeField] private MonoBehaviour inputSourceComponent;
        [SerializeField] private PlayerInput playerInput;

        private ICharacterInputSource inputSource;

        private void Awake()
        {
            if (motor == null)
                motor = GetComponent<TopDownCharacterMotor>();

            if (inputSourceComponent == null)
                inputSourceComponent = GetComponent<UnityInputSystemCharacterInputSource>();

            if (playerInput == null)
                playerInput = GetComponent<PlayerInput>();

            inputSource = inputSourceComponent as ICharacterInputSource;
            SetPlayerInputEnabled(false);
        }

        public override void OnStartClient()
        {
            SetPlayerInputEnabled(IsOwner);
        }

        public override void OnStopClient()
        {
            SetPlayerInputEnabled(false);
        }

        private void Update()
        {
            if (motor == null || inputSource == null || !IsOwner)
                return;

            CharacterInputFrame input = inputSource.CaptureInput();
            motor.Simulate(in input, Time.deltaTime);
            ServerRpcSubmitInput(input.Move, input.AimWorld, input.PressedActionIds, input.HeldActionIds, input.Tick);
        }

        [ServerRpc]
        private void ServerRpcSubmitInput(Vector2 move, Vector2 aimWorld, string[] pressedActionIds, string[] heldActionIds, uint tick)
        {
            if (motor == null)
                return;

            CharacterInputFrame input = new CharacterInputFrame(move, aimWorld, pressedActionIds, heldActionIds, tick);
            motor.Simulate(in input, Time.deltaTime);
        }

        private void SetPlayerInputEnabled(bool enabled)
        {
            if (playerInput != null)
                playerInput.enabled = enabled;
        }
    }
#else
    public sealed class FishNetCharacterMotorDriver : MonoBehaviour
    {
    }
#endif
}
