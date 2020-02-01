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
    private Rigidbody2D rb = default;
    [Autohook, SerializeField]
    private BoxCollider2D boxCol = default;

    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float maxSpeed = 10f;
    [SerializeField]
    private float jumpStrength = 5;
    [SerializeField]
    private float groundedRaycastDistance = 0.1f;
    [SerializeField]
    private float minTimeBetweenJumps = 0.25f;
    [SerializeField]
    private float additionalDragClimbing = 6;

    private InputMaster inputMaster;
    private float horizontalInput;
    private float verticalInput;
    private bool isGrounded;
    private bool minTimeBetweenJumpsHasPassed = true;
    private int climbableColliders = 0;
    private bool climbing => climbableColliders > 0;
    private float originalGravity;

    private GameObject m_BackgroundAudio;


    [SerializeField]
    private State state = State.Powerless;

    private void OnEnable() => inputMaster.Enable();

    private void OnDisable() => inputMaster.Disable();


    private bool phase = false;
    private bool carrying = false;
    private GameObject objectToCatch;
    private bool availableCatch;

    private void Awake() {
        inputMaster = new InputMaster();
        inputMaster.Player.SetCallbacks(this);
        m_BackgroundAudio = GameObject.FindGameObjectWithTag("BackgroundSound");
        originalGravity = rb.gravityScale;
        availableCatch = false;
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
        Vector2 feet = transform.position;
        feet.y -= boxCol.bounds.extents.y - 0.25f;
        RaycastHit2D groundHit = Physics2D.BoxCast(feet, new Vector2(boxCol.bounds.extents.x - 0.1f, 0.1f), 0, Vector2.down,groundedRaycastDistance, ~(LayerMask.GetMask("Player")));
        Debug.DrawRay(feet, Vector2.down * groundedRaycastDistance, Color.red);
        isGrounded = groundHit.collider != null;
        if (inputMaster.Player.Jump.triggered && isGrounded && minTimeBetweenJumpsHasPassed && state >= State.CanJump && !climbing) {
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

    // private void OnCollisionEnter2D(Collision2D col) {
    //     if (col.collider.CompareTag("Feet") && col.otherCollider.CompareTag("Floor")) {
    //         isGrounded = true;
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D col) {
    //     if (col.collider.CompareTag("Feet") && col.otherCollider.CompareTag("Floor")) {
    //         isGrounded = false;
    //     }
    // }

   

    public void OnCatch(InputAction.CallbackContext context)
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
           if (availableCatch)
            {
                objectToCatch.transform.SetParent(transform);
                Destroy(objectToCatch.GetComponent<Rigidbody2D>());
                carrying = true;
            }
        }
    }


    public void OnRightPhase(InputAction.CallbackContext context)
    {
        if (state == State.CanPhase)
        {
            phase = !phase;
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



}
