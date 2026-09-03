using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 15f;
    public float lookSpeed = 5f;

    // private Rigidbody body;
    private CharacterController body;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // body = GetComponent<Rigidbody>();
        body = GetComponent<CharacterController>();

        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    // void FixedUpdate()
    // {
    //     Vector2 lookInput = lookAction.ReadValue<Vector2>();
    //     Vector3 rotationVector = new Vector3(0f, lookInput.x * lookSpeed, 0f);
    //     transform.Rotate(rotationVector);

    //     bool isSprinting = sprintAction.IsPressed();
    //     float moveSpeed = isSprinting ? sprintSpeed : walkSpeed;

    //     Quaternion currentRotation = body.transform.rotation;
    //     Vector2 moveInput = moveAction.ReadValue<Vector2>();
    //     Vector3 moveDirection = new Vector3(
    //         moveInput.x,
    //         0f,
    //         moveInput.y
    //     );

    //     body.linearVelocity = currentRotation * moveDirection * moveSpeed;
    // }

    // void FixedUpdate()
    // {
    //     if (body.transform.position.y < -50f)
    //     {
    //         body.linearVelocity = Vector3.zero;
    //         body.angularVelocity = Vector3.zero;
    //         body.transform.position = Vector3.zero;
    //         body.transform.rotation = Quaternion.identity;
    //         return;
    //     }

    //     Vector2 lookInput = lookAction.ReadValue<Vector2>();

    //     if (lookInput.magnitude > 0f)
    //     {
    //         Quaternion rotation = Quaternion.Euler(0f, lookInput.x * lookSpeed, 0f);
    //         body.MoveRotation(body.transform.rotation * rotation);
    //     }

    //     Vector2 moveInput = moveAction.ReadValue<Vector2>();

    //     if (moveInput.magnitude > 0f)
    //     {
    //         bool isSprinting = sprintAction.IsPressed();
    //         float moveSpeed = isSprinting ? sprintSpeed : walkSpeed;

    //         Vector3 moveVector = body.transform.rotation * new Vector3(
    //             moveInput.x * moveSpeed,
    //             0f,
    //             moveInput.y * moveSpeed
    //         );

    //         body.MovePosition(body.transform.position + moveVector);
    //     }
    // }

    void Update()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        if (lookInput.magnitude > 0f)
        {
            Vector3 rotationVector = new Vector3(0f, lookInput.x, 0f);
            transform.Rotate(lookSpeed * Time.deltaTime * rotationVector);
        }

        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        if (moveInput.magnitude > 0f)
        {
            bool isSprinting = sprintAction.IsPressed();
            float moveSpeed = isSprinting ? sprintSpeed : walkSpeed;

            Vector3 moveVector = body.transform.rotation * new Vector3(
                moveInput.x,
                0f,
                moveInput.y
            );

            body.Move(moveSpeed * Time.deltaTime * moveVector);
        }
    }
}
