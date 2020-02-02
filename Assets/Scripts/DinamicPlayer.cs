using HCF;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
public class DinamicPlayer : MonoBehaviour, InputMaster.IPlayerActions
{
    public enum State {
        Powerless,
        CanJump,
        CanLoad,
        CanClimb,
        CanPhase
    }

    [Autohook, SerializeField]
    public Rigidbody2D rb = default;
    [Autohook, SerializeField]
    private Animator animator = default;
    [Autohook, SerializeField]
    private SpriteRenderer sprite;
    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float maxSpeed = 10f;
    [SerializeField]
    private float additionalDragClimbing = 6;
    [SerializeField]
    private int graceFrames = 5;
    [SerializeField]
    private State state = State.Powerless;

    [Header("Movement")]
    [Tooltip("Amount of force added when the player jumps.")]
    [SerializeField]
    private float m_JumpForce = 400f;
    [Tooltip("How much to smooth out the movement")]
    [Range(0, .3f)]
    [SerializeField]
    private float m_MovementSmoothing = .05f;
    [Tooltip("Whether or not a player can steer while jumping")]
    [SerializeField]
    private bool m_AirControl = false;
    [Tooltip("A mask determining what is ground to the character")]
    [SerializeField]
    private LayerMask m_WhatIsGround = default;
    [Tooltip("A position marking where to check if the player is grounded")]
    [SerializeField]
    private Transform m_GroundCheck = default;
    [SerializeField]
    [Tooltip("Radius of the overlap circle to determine if grounded")]
    private float k_GroundedRadius = .2f;
    [SerializeField]
    private float runSpeed = 40f;

    private InputMaster inputMaster;
    private float horizontalInput;
    private float verticalInput;
    private int climbableColliders = 0;
    private bool climbing => climbableColliders > 0;
    private float originalGravity;
    private int graceFramesRemaining = 0;
    private GameObject m_BackgroundAudio;
    private UIController m_canvas;

    private bool phase = false;
    private bool canJumpNotGrab = true;
    private bool carrying = false;
    private GameObject objectToCatch;
    private bool availableCatch;
    private bool m_Grounded;
    private Vector3 m_Velocity = Vector3.zero;
    private bool jump;
    float horizontalMove = 0f, verticalMove = 0f;
    public Transform carryingPos;

    private void OnEnable() => inputMaster.Enable();

    private void OnDisable() => inputMaster.Disable();

    private void Awake() {
        inputMaster = new InputMaster();
        inputMaster.Player.SetCallbacks(this);
        m_BackgroundAudio = GameObject.FindGameObjectWithTag("BackgroundSound");
        originalGravity = rb.gravityScale;
        availableCatch = false;
        m_canvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();
    }

    public void OnHorizontal(InputAction.CallbackContext ctx) {
        horizontalInput = ctx.ReadValue<float>();
        sprite.flipX = horizontalInput < 0;
        horizontalMove = horizontalInput * runSpeed;
    }

    public void OnVertical(InputAction.CallbackContext ctx) => verticalInput = ctx.ReadValue<float>();

    // Update is called once per frame	
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;
    }

    public void SetState(State newState) {
        state = newState;
        if (m_BackgroundAudio != null) {
            m_BackgroundAudio.SendMessage("ChangeClip", newState);
        }
    }


    
    public State GetState() => state;

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);
    }

    private void FixedUpdate() {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject) {
                m_Grounded = true;
                break;
            }
        }

        Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, jump);
        jump = false;
        /*
        bool climbing2 = climbing && !phase;
        if (climbing2 && state >= State.CanClimb) {
            rb.gravityScale = 0;
            rb.drag = rb.DragRequiredFromImpulse(acceleration, maxSpeed) + additionalDragClimbing;
            rb.AddForce(Vector2.up * verticalInput * acceleration * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        else {
            rb.gravityScale = originalGravity;
            rb.drag = rb.DragRequiredFromImpulse(acceleration, maxSpeed);
        }
        if (graceFramesRemaining > 0) {
            graceFramesRemaining--;
        }
        animator.SetBool("IsClimbing", climbing2);
        */
        if (phase)
        {
            GetComponent<SpriteRenderer>().color = new Color32(102, 140, 255, 200);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Climbable")) {
            climbableColliders++;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.CompareTag("Climbable")) {
            climbableColliders--;
        }
    }

    public void OnCatch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!canJumpNotGrab)
            {
                if (carrying)
                {
                    //DROP
                    objectToCatch.transform.SetParent(null);
                    objectToCatch.AddComponent<Rigidbody2D>();

                    carrying = false;
                }
                else
                {
                    //GRAB
                    if (availableCatch && objectToCatch != null)
                    {
                        objectToCatch.transform.SetParent(carryingPos);
                        objectToCatch.transform.position = new Vector3(0, 0, 1);
                        objectToCatch.transform.localPosition = new Vector3(0, 0, 1);
                        Destroy(objectToCatch.GetComponent<Rigidbody2D>());
                        carrying = true;
                    }
                }
            }
        }
    }


    public void OnLeftPhase(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (state == State.CanPhase)
            {
                m_canvas.changeLeft();
                phase = !phase;
            }
        }
    }

    public void OnRightPhase(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (state >= State.CanLoad)
            {
                m_canvas.changeRight();
                canJumpNotGrab = !canJumpNotGrab;

                if (carrying && objectToCatch)
                {
                    //DROP
                    objectToCatch.transform.SetParent(null);
                    objectToCatch.AddComponent<Rigidbody2D>();
                    carrying = false;
                }
            }
        }

    }

    public bool IsInPhase()
    {
        return phase;
    }

    public void EnableCatch(GameObject objeto)
    {
        availableCatch = true;
        objectToCatch = objeto;

    }

    public void DisableCatch()
    {
        availableCatch = false;
        objectToCatch = null;
    }

    public bool isPlayerCarrying()
    {
        return carrying;
    }

    public void DiscoverUI(State state)
    {  
        if (m_canvas != null) {
            switch (state)
            {
                case State.CanJump:
                    m_canvas.discoverJump();
                    break;
                case State.CanClimb:
                    m_canvas.discoverClimb();
                    break;
                case State.CanLoad:
                    m_canvas.discoverGrab();
                    break;
                case State.CanPhase:
                    m_canvas.discoverPhase();
                    break;
            }
        }
    }

    public void Move(float move, float moveV, bool jump)
    {
        bool climbing2 = climbing && !phase;

        if (climbing2 && state >= State.CanClimb)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(rb.velocity.x, moveV * 10f);
            // And then smoothing it out and applying it to the character
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
            
            rb.gravityScale = 0;
            rb.drag = rb.DragRequiredFromImpulse(acceleration, maxSpeed) + additionalDragClimbing;
            rb.AddForce(Vector2.up * verticalInput * acceleration * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
        else
        {
            rb.gravityScale = originalGravity;
            rb.drag = rb.DragRequiredFromImpulse(acceleration, maxSpeed);
        }
        if (graceFramesRemaining > 0)
        {
            graceFramesRemaining--;
        }
        
        animator.SetBool("IsClimbing", climbing2);

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            animator.SetFloat("Horizontal", horizontalInput);
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
            // And then smoothing it out and applying it to the character
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }
        // If the player should jump...
        if (canJumpNotGrab && m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            rb.AddForce(new Vector2(0f, m_JumpForce));
            animator.SetTrigger("Jump");
        }
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        jump = true;
    }
}
