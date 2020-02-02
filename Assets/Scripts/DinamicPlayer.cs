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

    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float maxSpeed = 10f;
    [SerializeField]
    private float jumpStrength = 5;
    [SerializeField]
    private float minTimeBetweenJumps = 0.25f;
    [SerializeField]
    private float additionalDragClimbing = 6;
    public int graceFrames = 5;

    private InputMaster inputMaster;
    private float horizontalInput;
    private float verticalInput;
    private bool minTimeBetweenJumpsHasPassed = true;
    private int climbableColliders = 0;
    [HideInInspector]
    public int groundColliders = 0;
    private bool climbing => climbableColliders > 0;
    //private bool isGrounded => groundColliders > 0;
    private bool isGrounded => (rb.velocity.y < 0.01f && rb.velocity.y > -0.01f) || groundColliders > 0;
    private float originalGravity;
    public int graceFramesRemaining = 0;

    private GameObject m_BackgroundAudio;

    private UIController m_canvas;

    [SerializeField]
    private State state = State.Powerless;

    private void OnEnable() => inputMaster.Enable();

    private void OnDisable() => inputMaster.Disable();


    private bool phase = false;
    private bool canJumpNotGrab = true;
    private bool carrying = false;
    private GameObject objectToCatch;
    private bool availableCatch;

    public Transform carryingPos;

    private void Awake() {
        inputMaster = new InputMaster();
        inputMaster.Player.SetCallbacks(this);
        m_BackgroundAudio = GameObject.FindGameObjectWithTag("BackgroundSound");
        originalGravity = rb.gravityScale;
        availableCatch = false;
        m_canvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();
    }

    public void OnHorizontal(InputAction.CallbackContext ctx) => horizontalInput = ctx.ReadValue<float>();

    public void OnVertical(InputAction.CallbackContext ctx) => verticalInput = ctx.ReadValue<float>();

    public void OnJump(InputAction.CallbackContext ctx) {}

    public void SetState(State newState) {
        state = newState;
        if (m_BackgroundAudio != null) {
            m_BackgroundAudio.SendMessage("ChangeClip", newState);
        }
    }

    private void FixedUpdate() {
        Vector2 horizontalDestination = new Vector2(horizontalInput * acceleration * Time.fixedDeltaTime, 0);
        rb.AddForce(horizontalDestination, ForceMode2D.Impulse);
        animator.SetFloat("Horizontal", horizontalInput);
        if (canJumpNotGrab && inputMaster.Player.Jump.triggered && minTimeBetweenJumpsHasPassed && state >= State.CanJump && (isGrounded || climbing || graceFramesRemaining > 0)) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            minTimeBetweenJumpsHasPassed = false;
            this.RunAfter(minTimeBetweenJumps, () => minTimeBetweenJumpsHasPassed = true);
        }
        if (climbing && state >= State.CanClimb) {
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
        animator.SetBool("IsClimbing", climbing);
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
                        objectToCatch = objectToCatch;
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
