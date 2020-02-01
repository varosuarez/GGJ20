using HCF;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
public class DinamicPlayer : MonoBehaviour, InputMaster.IPlayerActions
{
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

    private InputMaster inputMaster;
    private float movementInput;
    private bool isGrounded;

    private void OnEnable() => inputMaster.Enable();

    private void OnDisable() => inputMaster.Disable();

    private void Awake() {
        inputMaster = new InputMaster();
        inputMaster.Player.SetCallbacks(this);
    }

    public void OnMove(InputAction.CallbackContext ctx) {
        movementInput = ctx.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        // if (isGrounded && ctx.ReadValue) {
        //     rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        // }
    }

    private void FixedUpdate() {
        rb.drag = rb.DragRequiredFromImpulse(acceleration, maxSpeed);
        Vector2 horizontalDestination = new Vector2(movementInput * acceleration * Time.fixedDeltaTime, 0);
        rb.AddForce(horizontalDestination, ForceMode2D.Impulse);
        Vector2 feet = transform.position;
        feet.y -= boxCol.bounds.extents.y;
        RaycastHit2D groundHit = Physics2D.Raycast(feet, Vector2.down, 0.1f);
        isGrounded = groundHit.collider != null;
        if (inputMaster.Player.Jump.triggered && isGrounded) {
            rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
        }
    }
}
