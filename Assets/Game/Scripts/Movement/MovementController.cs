using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    // Last direction of movement (by input)
    public float LastDirection { get { return _lastDirection; } }

    // Is there anything beneath this
    public bool IsBlocked { get { return _isBlocked; } }

    // Is the player colliding down
    public bool IsGrounded { get { return _isGrounded; } }

    // Rigidbody's current velocity
    public Vector2 Velocity { get { return rigidbody.velocity; } }

    // Is this calculating physics updates
    public bool Simulating { get { return _simulate; } }

    public Vector2 RelativeUp { get { return _relativeUp; } }
    public Vector2 RelativeRight { get { return _relativeRight; } }
    public Vector2 RelativeLeft { get { return _relativeRight * -1f; } }
    public Vector2 RelativeDown { get { return _relativeUp * -1f; } }

    [SerializeField] public MovementSettings movementSettings;
    [SerializeField] public new Collider2D collider;
    [SerializeField] public new Rigidbody2D rigidbody;

    [SerializeField] private Transform _transform;
    [SerializeField] private ContactPoint2D[] _contacts = new ContactPoint2D[10];
    [SerializeField] private RaycastHit2D[] _jumpContacts = new RaycastHit2D[1];
    [SerializeField] private Vector2 _currentVelocity = Vector2.zero;
    [SerializeField] private bool _shouldJump = false;
    [SerializeField] private float _lastDirection = 1f;
    [SerializeField] private bool _isGrounded = false;
    [SerializeField] private int _touchingWallDirection = 0;
    [SerializeField] private bool _isTouchingCeiling = false;
    [SerializeField] private bool _didMoveLastFrame = false;
    [SerializeField] private bool _isBlocked = false;

    [SerializeField] private Vector2 _relativeUp = Vector2.up;
    [SerializeField] private Vector2 _relativeRight = Vector2.right;

    [SerializeField] private Transform _groundTransform;
    [SerializeField] private Vector3 _lastGroundPosition;

    [SerializeField] private bool _simulate = true;
    [SerializeField] private ContactFilter2D _contactFilter;
    [SerializeField] private float _jumpGraceTimer = 0f;

    public void Start() {
        if (!_transform) _transform = GetComponent<Transform>();
        if (!rigidbody) rigidbody = GetComponent<Rigidbody2D>();
        if (!collider) collider = GetComponent<BoxCollider2D>();

        //gameState = FindObjectOfType<GameStateController>();

        rigidbody.isKinematic = false;
        rigidbody.simulated = true;
        rigidbody.freezeRotation = true;
    }

    public void SetGravityScale(float scale) {
        rigidbody.gravityScale = scale;
    }

    public void SetSimulated(bool simulate) {
        _simulate = simulate;
        if (simulate == false) rigidbody.simulated = false;
        else rigidbody.simulated = true;
    }

    public void FixedUpdate() {
        if (!_simulate) return;

        if (_groundTransform) {
            if (_lastGroundPosition != _groundTransform.position) {
                Vector3 diff = _groundTransform.position - _lastGroundPosition;
                transform.Translate(diff);
                _lastGroundPosition = _groundTransform.position;
            }
        }

        if (!_didMoveLastFrame) {
            dampenMovement();
        }
        _didMoveLastFrame = false;


        rigidbody.velocity = new Vector2(
            Mathf.Clamp(rigidbody.velocity.x, -movementSettings.maxXVelocity, movementSettings.maxXVelocity),
            Mathf.Clamp(rigidbody.velocity.y, -movementSettings.maxYVelocity, movementSettings.maxYVelocity)
        );
    }

    public void SetRelative(Vector2 up, Vector2 right) {
        _relativeUp = up;
        _relativeRight = right;
    }

    public void SetVelocity(Vector2 velocity) {
        rigidbody.velocity = velocity;
    }

    public bool Jump(bool canJumpInAir = false) {
        if (_isTouchingCeiling || movementSettings.canJump == false) _shouldJump = false;
        else if (canJumpInAir) _shouldJump = true;
        else if (_isGrounded) _shouldJump = true;
        else if (movementSettings.useJumpCushion && jumpCushionCollides()) _shouldJump = true;
        else if (movementSettings.useJumpGracePeriod && _jumpGraceTimer > 0f) _shouldJump = true;

        return _shouldJump;
    }

    public void Move(float direction) {
        float adjustedDirection = direction * RelativeRight.x;

        if (adjustedDirection > 0) _lastDirection = 1f;
        else if (adjustedDirection < 0) _lastDirection = -1f;

        // prevent wall sticking
        if (Mathf.Sign(adjustedDirection) == _touchingWallDirection) {
            direction = 0;
        }

        // prevent affecting velocity with move when input is 0
        _didMoveLastFrame = direction > 0f || direction < 0f;
        if (!_didMoveLastFrame) return;

        if (_isGrounded) {
            float desiredDirection = Mathf.Sign(direction);
            Vector2 targetVelocity = new Vector2(direction * movementSettings.groundMoveVelocity, rigidbody.velocity.y);
            SetVelocity(Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref _currentVelocity, movementSettings.groundMoveSmoothing));
        } else if (movementSettings.canMoveInAir) {
            float desiredDirection = Mathf.Sign(direction);
            Vector2 targetVelocity = new Vector2(direction * movementSettings.airMoveVelocity, rigidbody.velocity.y);
            SetVelocity(Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref _currentVelocity, movementSettings.airMoveSmoothing));
        }
    }

    public void Update() {
        if (!_simulate) return;

        updateContacts();

        if (_shouldJump) {
            _shouldJump = false;
            _isGrounded = false;
            _isBlocked = false;
            _jumpGraceTimer = 0f;
            SetVelocity(new Vector2(rigidbody.velocity.x, 0f) + (RelativeUp * movementSettings.jumpForce));
        } else {
            _jumpGraceTimer -= Time.deltaTime;
            if (_jumpGraceTimer < 0f) _jumpGraceTimer = 0f;
        }
    }

    public void SetGrounded() {
        _isGrounded = true;
        if (Velocity.y < 0.1f) {
            _jumpGraceTimer = movementSettings.jumpGracePeriod;
        }
    }

    public void AddForce(Vector2 force) {
        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    public void ClearVelocity() {
        SetVelocity(Vector2.SmoothDamp(rigidbody.velocity, Vector2.zero, ref _currentVelocity, 0.0001f));
    }

    private void dampenMovement() {
        if (_isGrounded) {
            Vector2 targetVelocity = new Vector2(0, rigidbody.velocity.y);
            if (Mathf.Abs(_currentVelocity.x) < 0.01f) _currentVelocity.Set(0, _currentVelocity.y);
            SetVelocity(Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref _currentVelocity, movementSettings.groundStopSmoothing));

        } else if (movementSettings.dampenAirMovement) {
            Vector2 targetVelocity = new Vector2(0, rigidbody.velocity.y);
            if (Mathf.Abs(_currentVelocity.x) < 0.01f) _currentVelocity.Set(0, _currentVelocity.y);
            SetVelocity(Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref _currentVelocity, movementSettings.airStopSmoothing));
        }
    }

    private bool jumpCushionCollides() {
        int castResults = collider.Cast(RelativeDown * movementSettings.jumpCushionDistance, _contactFilter, _jumpContacts);
        return (castResults > 0 && _jumpContacts[0].distance < movementSettings.jumpCushionDistance);
    }

    private void updateContacts() {
        _isGrounded = false;
        _isTouchingCeiling = false;
        _isBlocked = false;
        _touchingWallDirection = 0;

        int contactCount = collider.GetContacts(_contacts);
        for (int i = 0; i < contactCount; i++) {
            Debug.DrawLine(_contacts[i].point, _contacts[i].point + _contacts[i].normal, Color.green);

            if (_contacts[i].normal.y > 0.5f) {
                SetGrounded();
                _groundTransform = _contacts[i].collider.transform;
                _lastGroundPosition = _contacts[i].collider.transform.position;
            } else if (_contacts[i].normal.y < -0.5f) {
                _isTouchingCeiling = true;
            }

            if (_contacts[i].normal.x > 0.5f) {
                _isBlocked = true;
                _touchingWallDirection = -1;
            } else if (_contacts[i].normal.x < -0.5f) {
                _isBlocked = true;
                _touchingWallDirection = 1;
            }
        }
    }
}
