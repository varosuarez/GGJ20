using HCF;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, InputMaster.IPlayerActions
{
    [Autohook, SerializeField]
    private Rigidbody2D rb = default;

    [SerializeField]
    private float speed = 10f;

    private InputMaster inputMaster;
    private Vector2 movementInput;

    private void OnEnable() => inputMaster.Enable();

    private void OnDisable() => inputMaster.Disable();

    private void Awake() {
        inputMaster = new InputMaster();
        inputMaster.Player.SetCallbacks(this);
    }

    public void OnMove(InputAction.CallbackContext ctx) {
        movementInput = ctx.ReadValue<Vector2>();
    }

    private void FixedUpdate() {
        Vector3 destination = movementInput * speed;
        rb.MovePosition(transform.position + destination * Time.fixedDeltaTime);
    }
}
