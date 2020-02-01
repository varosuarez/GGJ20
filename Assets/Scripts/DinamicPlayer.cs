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

    private InputMaster inputMaster;
    private float movementInput;
    private bool isGrounded;
    private bool minTimeBetweenJumpsHasPassed = true;
    private int climbableColliders = 0;
    private bool climbing => climbableColliders > 0;
    private float originalGravity;

    [SerializeField]
    private State state = State.Powerless;

    private void OnEnable() => inputMaster.Enable();

    private void OnDisable() => inputMaster.Disable();

    private void Awake() {
        inputMaster = new InputMaster();
        inputMaster.Player.SetCallbacks(this);
        originalGravity = rb.gravityScale;
    }

    public void OnMove(InputAction.CallbackContext ctx) {
        movementInput = ctx.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext ctx) {}

    public void SetState(State newState) => state = newState;

    private void FixedUpdate() {
        rb.drag = rb.DragRequiredFromImpulse(acceleration, maxSpeed);
        Vector2 horizontalDestination = new Vector2(movementInput * acceleration * Time.fixedDeltaTime, 0);
        rb.AddForce(horizontalDestination, ForceMode2D.Impulse);
        Vector2 feet = transform.position;
        feet.y -= boxCol.bounds.extents.y - 0.25f;
        RaycastHit2D groundHit = Physics2D.BoxCast(feet, new Vector2(boxCol.bounds.extents.x - 0.1f, 0.1f), 0, Vector2.down,groundedRaycastDistance, ~(LayerMask.GetMask("Player")));
        Debug.DrawRay(feet, Vector2.down * groundedRaycastDistance, Color.red);
        isGrounded = groundHit.collider != null;
        if (inputMaster.Player.Jump.triggered && isGrounded && minTimeBetweenJumpsHasPassed && state >= State.CanJump) {
            rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            minTimeBetweenJumpsHasPassed = false;
            this.RunAfter(minTimeBetweenJumps, () => minTimeBetweenJumpsHasPassed = true);
        }
        if (climbing && state >= State.CanClimb) {
            rb.gravityScale = 0;
        }
        else {
            rb.gravityScale = originalGravity;
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

    private bool phase = false;
    private bool carrying = false;
    private GameObject objectToCatch = null;
    private bool availableCatch = false;

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

}
