using UnityEngine;

namespace JoeConticello.Characters2D
{
    [DisallowMultipleComponent]
    public sealed class TopDownCharacterMotor : MonoBehaviour, ICharacterMotor
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private CharacterFacingMode facingMode = CharacterFacingMode.Aim;

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
            bool facingLeft = GetFacingLeft(input, position);

            if (body != null)
            {
                body.linearVelocity = velocity;
            }
            else
            {
                transform.position = position;
            }

            ApplyFacing(facingLeft);
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

        private bool GetFacingLeft(in CharacterInputFrame input, Vector2 position)
        {
            float directionX = facingMode == CharacterFacingMode.Aim
                ? input.AimWorld.x - position.x
                : input.Move.x;

            if (Mathf.Approximately(directionX, 0f))
                return state.FacingLeft;

            return directionX < 0f;
        }

        private void ApplyFacing(bool facingLeft)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (facingLeft ? -1f : 1f);
            transform.localScale = scale;
        }
    }
}
