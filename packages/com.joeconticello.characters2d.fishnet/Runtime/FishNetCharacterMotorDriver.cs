using UnityEngine;
using JoeConticello.Characters2D;
using FishNet.Object;


namespace JoeConticello.Characters2D.FishNet
{

    [DisallowMultipleComponent]
    public sealed class FishNetCharacterMotorDriver : NetworkBehaviour
    {
        [SerializeField] private TopDownCharacterMotor motor;
        [SerializeField] private MonoBehaviour inputSourceComponent;

        private ICharacterInputSource inputSource;

        private void Awake()
        {
            if (motor == null)
                motor = GetComponent<TopDownCharacterMotor>();

            if (inputSourceComponent == null)
                inputSourceComponent = GetComponent<UnityInputSystemCharacterInputSource>();

            inputSource = inputSourceComponent as ICharacterInputSource;
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
    }
}

