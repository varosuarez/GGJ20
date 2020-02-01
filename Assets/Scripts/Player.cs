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

    private GameManager.RayCrossColor m_RayColor = GameManager.RayCrossColor.RED;

    private InputMaster inputMaster;
    private float movementInput;

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
        Vector3 destination = new Vector2(movementInput * speed, 0);
        rb.MovePosition(transform.position + destination * Time.fixedDeltaTime);
    }

    public GameManager.RayCrossColor GetRayColor()
    {
       return m_RayColor;
    }
}
