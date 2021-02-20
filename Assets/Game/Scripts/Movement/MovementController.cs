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

    public bool IsHorizontalBlocked { get { return _isHorizontalBlocked; } }

    // Is the player colliding down
    public bool IsGrounded { get { return _fallFrames < _minFallFrames; } }

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
    [SerializeField] private Vector2 _currentMoveDampenVelocity = Vector2.zero;
    [SerializeField] private Vector2 _currentStopDampenVelocity = Vector2.zero;
    [SerializeField] private int _fallFrames = 0;
    [SerializeField] private bool _shouldJump = false;
    [SerializeField] private float _lastDirection = 1f;
    [SerializeField] private bool _isGrounded = false;
    [SerializeField] private int _touchingWallDirection = 0;
    [SerializeField] private bool _isTouchingCeiling = false;
    [SerializeField] private bool _didMoveLastFrame = false;
    [SerializeField] private bool _isBlocked = false;
    [SerializeField] private bool _isHorizontalBlocked = false;

    [SerializeField] private Vector2 _relativeUp = Vector2.up;
    [SerializeField] private Vector2 _relativeRight = Vector2.right;

    [SerializeField] private Transform _groundTransform;
    [SerializeField] private Vector3 _lastGroundPosition;

    [SerializeField] private bool _simulate = true;
    [SerializeField] private ContactFilter2D _collisionContactFilter;
    [SerializeField] private ContactFilter2D _jumpPadContactFilter;
    [SerializeField] private float _jumpGraceTimer = 0f;
    [SerializeField] private float _jumpBoostTimer = 0f;
    [SerializeField] private float _minGroundDistance = 0.1f;

    private const int _minFallFrames = 10;

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
        rigidbody.velocity = Vector2.zero;
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
            dampenMovement(Time.fixedDeltaTime);
        } else {
            _currentStopDampenVelocity = Vector2.zero;
        }

        _didMoveLastFrame = false;


        float maxYVelocity = _jumpBoostTimer > 0 ? movementSettings.jumpBoostMaxVelocity : movementSettings.maxYVelocity;
        rigidbody.velocity = new Vector2(
            Mathf.Clamp(rigidbody.velocity.x, -movementSettings.maxXVelocity, movementSettings.maxXVelocity),
            Mathf.Clamp(rigidbody.velocity.y, -movementSettings.maxYDownVelocity, maxYVelocity)
        );

    }

    public void SetRelative(Vector2 up, Vector2 right) {
        _relativeUp = up;
        _relativeRight = right;
    }

    public void SetVelocity(Vector2 velocity) {
        rigidbody.velocity = velocity;
    }

    public void SetFallFrames(int frames) {
        _fallFrames = frames;
    }

    public bool Jump(bool canJumpInAir = false) {
        if (_isTouchingCeiling || movementSettings.canJump == false) _shouldJump = false;
        else if (canJumpInAir) _shouldJump = true;
        else if (_isGrounded) _shouldJump = true;
        else if (movementSettings.useJumpCushion && jumpCushionCollides()) _shouldJump = true;
        else if (movementSettings.useJumpGracePeriod && _jumpGraceTimer > 0f) _shouldJump = true;

        return _shouldJump;
    }

    public void HoldJump() {
        if (_jumpBoostTimer > 0f) {
            // float jumpBoostFraction = _jumpBoostTimer / movementSettings.jumpBoostPeriod;
            // Debug.Log(jumpBoostFraction);
            // SetVelocity(new Vector2(rigidbody.velocity.x, 0f) + (RelativeUp * movementSettings.jumpForce));
        }
    }

    public void ReleaseJump() {
        _jumpBoostTimer = 0f;
    }

    public void Move(float direction, float deltaTime) {
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
            Vector2 targetVelocity = new Vector2(direction * movementSettings.groundMoveVelocity, rigidbody.velocity.y);
            SetVelocity(Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref _currentMoveDampenVelocity, movementSettings.groundMoveSmoothing * deltaTime));
        } else if (movementSettings.canMoveInAir) {
            Vector2 targetVelocity = new Vector2(direction * movementSettings.airMoveVelocity, rigidbody.velocity.y);
            SetVelocity(Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref _currentMoveDampenVelocity, movementSettings.airMoveSmoothing * deltaTime));
        }
    }

    public void Update() {
        if (!_simulate) return;

        updateContacts();

        if (_shouldJump) {
            if (movementSettings.useJumpBoostPeriod) {
                _jumpBoostTimer = movementSettings.jumpBoostPeriod;
            }

            _shouldJump = false;
            _isGrounded = false;
            _isBlocked = false;
            _jumpGraceTimer = 0f;
            float baseYVelocity = rigidbody.velocity.y > 0f ? rigidbody.velocity.y : 0f;
            SetVelocity(new Vector2(rigidbody.velocity.x, baseYVelocity) + (RelativeUp * movementSettings.jumpForce));
        } else {
            _jumpBoostTimer -= Time.deltaTime;
            if (_jumpBoostTimer < 0f) _jumpBoostTimer = 0f;

            _jumpGraceTimer -= Time.deltaTime;
            if (_jumpGraceTimer < 0f) _jumpGraceTimer = 0f;
        }

        if (!_isGrounded) {
            _fallFrames++;
        } else {
            _fallFrames = 0;
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
        SetVelocity(Vector2.SmoothDamp(rigidbody.velocity, Vector2.zero, ref _currentMoveDampenVelocity, 0.0001f));
    }

    private void dampenMovement(float deltaTime) {
        if (_isGrounded) {
            Vector2 targetVelocity = new Vector2(0, rigidbody.velocity.y);
            // if (Mathf.Abs(_currentVelocity.x) < 0.01f) _currentVelocity.Set(0, _currentVelocity.y);
            SetVelocity(Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref _currentStopDampenVelocity, movementSettings.groundStopSmoothing * deltaTime));

        } else if (movementSettings.dampenAirMovement) {
            Vector2 targetVelocity = new Vector2(0, rigidbody.velocity.y);
            // if (Mathf.Abs(_currentVelocity.x) < 0.01f) _currentVelocity.Set(0, _currentVelocity.y);
            SetVelocity(Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref _currentStopDampenVelocity, movementSettings.airStopSmoothing, movementSettings.airStopMaxSpeed, deltaTime));
        }
    }

    private bool jumpCushionCollides() {
        Collider2D col = Physics2D.OverlapBox(transform.position, new Vector2(0.1f, 0.1f), 0f, _jumpPadContactFilter.layerMask);
        if (col != null) {
            return true;
        }

        int castResults = collider.Cast(RelativeDown, _jumpPadContactFilter, _jumpContacts);
        if (castResults > 0) {
            float zeroDist = Mathf.Abs(_jumpContacts[0].point.y - transform.position.y);
            if (zeroDist < movementSettings.jumpCushionDistance) {
                return true;
            }
        }
        return false;
    }

    public bool CheckStuck() {
        Collider2D col = Physics2D.OverlapBox(transform.position, new Vector2(0.1f, 0.1f), 0f, _collisionContactFilter.layerMask);
        if (col != null) {
            Debug.Log($"Invalid Collision with Object: {col.gameObject.name}", gameObject);
        }
        return col != null;
    }

    private void updateContacts() {
        _isGrounded = false;
        _isTouchingCeiling = false;
        _isBlocked = false;
        _isHorizontalBlocked = false;
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
                _isHorizontalBlocked = true;
            } else if (_contacts[i].normal.x < -0.5f) {
                _isBlocked = true;
                _touchingWallDirection = 1;
                _isHorizontalBlocked = true;
            }
        }

        if (!_isGrounded) {
            int groundCollisions = collider.Cast(Vector2.down, _collisionContactFilter, _jumpContacts, _minGroundDistance);
             if (groundCollisions > 0) {
                SetGrounded();
            }
        }
    }
}
