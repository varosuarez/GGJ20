using HCF;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
public class DinamicPlayer : MonoBehaviour, InputMaster.IPlayerActions
{
    private enum State {
        Powerless,
        CanJump,
        CanLoad,
        CanClimb,
        CanRay
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

    [SerializeField]
    private State state = State.Powerless;

    private void OnEnable() => inputMaster.Enable();

    private void OnDisable() => inputMaster.Disable();

    private void Awake() {
        inputMaster = new InputMaster();
        inputMaster.Player.SetCallbacks(this);
    }

    public void OnMove(InputAction.CallbackContext ctx) {
        movementInput = ctx.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext ctx) {}

    private void FixedUpdate() {
        rb.drag = rb.DragRequiredFromImpulse(acceleration, maxSpeed);
        Vector2 horizontalDestination = new Vector2(movementInput * acceleration * Time.fixedDeltaTime, 0);
        rb.AddForce(horizontalDestination, ForceMode2D.Impulse);
        Vector2 feet = transform.position;
        feet.y -= boxCol.bounds.extents.y - 0.25f;
        RaycastHit2D groundHit = Physics2D.Raycast(feet, Vector2.down, groundedRaycastDistance, ~(LayerMask.GetMask("Player")));
        Debug.DrawRay(feet, Vector2.down * groundedRaycastDistance, Color.red);
        isGrounded = groundHit.collider != null;
        if (inputMaster.Player.Jump.triggered && isGrounded && minTimeBetweenJumpsHasPassed && state >= State.CanJump) {
            rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            minTimeBetweenJumpsHasPassed = false;
            this.RunAfter(minTimeBetweenJumps, () => minTimeBetweenJumpsHasPassed = true);
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
}
