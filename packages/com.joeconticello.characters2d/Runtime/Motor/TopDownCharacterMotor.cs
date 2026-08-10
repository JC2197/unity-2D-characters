using UnityEngine;

namespace JoeConticello.Characters2D
{
    [DisallowMultipleComponent]
    public sealed class TopDownCharacterMotor : MonoBehaviour, ICharacterMotor
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private Rigidbody2D body;

        private CharacterMotorState state;
        public CharacterMotorState State => state;

        private void Awake()
        {
            if (body == null)
                body = GetComponent<Rigidbody2D>();
        }

        public void Simulate(in CharacterInputFrame input, float deltaTime)
        {
            Vector2 velocity = input.Move * moveSpeed;
            Vector2 position = body != null ? body.position + velocity * deltaTime : (Vector2)transform.position + velocity * deltaTime;

            float angle = Mathf.Atan2(input.AimWorld.y - position.y, input.AimWorld.x - position.x) * Mathf.Rad2Deg;
            bool facingLeft = input.AimWorld.x < position.x;

            if (body != null)
            {
                body.linearVelocity = velocity;
            }
            else
            {
                transform.position = position;
            }

            state = new CharacterMotorState(position, velocity, angle, facingLeft);
        }

        public void Teleport(Vector2 worldPosition)
        {
            if (body != null)
            {
                body.position = worldPosition;
                body.linearVelocity = Vector2.zero;
            }
            else
            {
                transform.position = worldPosition;
            }

            state = new CharacterMotorState(worldPosition, Vector2.zero, state.AimAngleDeg, state.FacingLeft);
        }
    }
}
