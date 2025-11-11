using UnityEngine;

public class NewRoboMovement : MonoBehaviour
{
    [Header("References")]
    public RoboMovementStats moveStats;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;

    private Rigidbody2D _rb;

    //movement vars
    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    //collision check vars
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    //jump vars

    public float verticalVelocity {  get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;

    //apex vars
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    //jump buffer vars
    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    //coyote time vars
    private float _coyoteTimer;

    private void Awake()
    {
        _isFacingRight = true;

        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CountTimers();
        JumpChecks();

    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();
        if (_isGrounded)
        {
            Move(moveStats.groundAcceleration, moveStats.groundDeceleration, RoboInputManager.movement);
        }
        else
        {
            Move(moveStats.airAcceleration, moveStats.airDeceleration, RoboInputManager.movement);
        }
    }
    #region Movement
    private void Move(float acceleration,float deceleration, Vector2 moveInput)
    {
       if(moveInput!= Vector2.zero)
        {

            TurnCheck(moveInput);

            Vector2 targetVelocity = Vector2.zero;
            targetVelocity = new Vector2(moveInput.x, 05) * moveStats.maxWalkSpeed;
            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_moveVelocity.x,_rb.linearVelocity.y);
        }
       else if(moveInput == Vector2.zero)
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
        }
    }
    void TurnCheck(Vector2 moveInput)
    {
        if(_isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if(!_isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }
    void Turn(bool turnRight)
    {
        if(turnRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            _isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }
    #endregion

    #region Jump

    private void JumpChecks()
    {
        //When we press the jump button
        if (RoboInputManager.jumpWasPressed)
        {
            _jumpBufferTimer = moveStats.jumpBufferTime;
            _jumpReleasedDuringBuffer = false;
        }
        //When we release the jump button
        if (RoboInputManager.jumpIsReleased)
        {
            if(_jumpBufferTimer > 0f)
            {
                _jumpReleasedDuringBuffer = true;
            }
            if(_isJumping&& verticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = moveStats.timeForUpwardsCancel;
                    verticalVelocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = verticalVelocity;
                }
            }
        }

        //Initiate jump with jump buffering and coyote time
        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
        {
            InitiateJump(1);

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = verticalVelocity;
            }
        }
        //Double jump
        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed > moveStats.numberOfJumpsAllowed)
        {
            _isFastFalling = false;
            InitiateJump(1);
        }
        //Air jump after coyote time lapsed
        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed > moveStats.numberOfJumpsAllowed -1)
        {
            InitiateJump(2);
            _isFastFalling = false;
        }
        //Landed
        if ((_isJumping|| _isFalling)&& _isGrounded&& verticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;
            verticalVelocity = Physics2D.gravity.y;
        }
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if (!_isJumping)
        {
            _isJumping = true;
        }

        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += numberOfJumpsUsed;
        verticalVelocity = moveStats.initialJumpVelocity;
    }
    private void Jump()
    {
        //Apply gravity while jumping
        if (_isJumping)
        {
            //Check for head bump
            if (_bumpedHead)
            {
                _isFastFalling = true;
            }
            //Gravity on ascending
            if(verticalVelocity >= 0f)
            {
                //Apex controls
                _apexPoint = Mathf.InverseLerp(moveStats.initialJumpVelocity, 0f, verticalVelocity);

                if (_apexPoint > moveStats.apexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }
                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if(_timePastApexThreshold < moveStats.apexHangTime)
                        {
                            verticalVelocity = 0f;
                        }
                        else
                        {
                            verticalVelocity = -0.01f;
                        }
                    }
                }
                //Gravity on ascending but not past apex threshold
                else
                {
                    verticalVelocity += moveStats.gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }

            }
            //Gravity on descending
            else if (!_isFastFalling)
            {
                verticalVelocity += moveStats.gravity * moveStats.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (verticalVelocity < 0f)
            {
                if(!_isFalling)
                {
                    _isFalling = true;
                }
            }
        }

        //Jump cut
        if (_isFastFalling)
        {
            if (_fastFallTime >= moveStats.timeForUpwardsCancel)
            {
                verticalVelocity += moveStats.gravity * moveStats.gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (_fastFallTime < moveStats.timeForUpwardsCancel)
            {
                verticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / moveStats.timeForUpwardsCancel));
            }

            _fastFallTime += Time.fixedDeltaTime;
        }
        //Normal gravity while falling

        if(!_isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }
            verticalVelocity += moveStats.gravity * Time.fixedDeltaTime;
        }
        //Clamp fall speed
        verticalVelocity = Mathf.Clamp(verticalVelocity, -moveStats.maxFallSpeed, 50f);

        _rb.linearVelocity= new Vector2(_rb.linearVelocity.x, verticalVelocity);
    }

    #endregion

    #region Collison Checks
    //Collision checks

    void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, moveStats.groundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, moveStats.groundDetectionRayLength, moveStats.groundLayer);
        if (_groundHit.collider != null)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }
    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * moveStats.headWidth, moveStats.headDetectionRayLength);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, moveStats.headDetectionRayLength, moveStats.groundLayer);
        if (_headHit.collider != null)
        {
            _bumpedHead = true;
        }
        else
        {
            _bumpedHead = false;
        }
    }
    private void CollisionChecks()
    {
        IsGrounded();
    }
    #endregion

    #region Timers
    private void CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;

        if (!_isGrounded)
        {
            _coyoteTimer -= Time.deltaTime; 
        }
        else
        {
            _coyoteTimer = moveStats.jumpCoyoteTime;
        }
    }
    #endregion

}
