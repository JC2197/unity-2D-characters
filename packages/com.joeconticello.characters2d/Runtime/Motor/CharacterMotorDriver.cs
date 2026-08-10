using UnityEngine;

namespace JoeConticello.Characters2D
{
    [DisallowMultipleComponent]
    public sealed class CharacterMotorDriver : MonoBehaviour
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
            if (motor == null || inputSource == null)
                return;

            CharacterInputFrame input = inputSource.CaptureInput();
            motor.Simulate(in input, Time.deltaTime);
        }
    }
}