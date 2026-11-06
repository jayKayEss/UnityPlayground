using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 15f;
    public float lookSpeed = 5f;

    [Header("Cue Ball Follow")]
    public float minCueBallVelocity = 0.1f;

    // private Rigidbody body;
    private CharacterController body;
    private NavMeshAgent agent;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    private Rigidbody cueBall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();

        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        sprintAction = InputSystem.actions.FindAction("Sprint");

        CueStick.OnCueStrike += OnCueStrike;
    }

    void OnDestroy()
    {
        CueStick.OnCueStrike -= OnCueStrike;
    }

    void Update()
    {
        UpdateCueBallFollow();
        UpdateMovement();
    }

    void UpdateMovement()
    {
        if (agent.enabled)
        {
            return;
        }

        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        if (lookInput.magnitude > 0f)
        {
            Vector3 rotationVector = new(0f, lookInput.x, 0f);
            transform.Rotate(lookSpeed * Time.deltaTime * rotationVector);
        }

        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        if (moveInput.magnitude > 0f)
        {
            bool isSprinting = sprintAction.IsPressed();
            float moveSpeed = isSprinting ? sprintSpeed : walkSpeed;

            Vector3 moveVector =
                body.transform.rotation * new Vector3(moveInput.x, 0f, moveInput.y);

            body.Move(moveSpeed * Time.deltaTime * moveVector);
        }
    }

    void UpdateCueBallFollow()
    {
        if (agent.enabled && cueBall.linearVelocity.magnitude >= minCueBallVelocity)
        {
            agent.SetDestination(cueBall.transform.position);
            body.transform.LookAt(cueBall.transform);
        }
        else
        {
            agent.enabled = false;
        }
    }

    void OnCueStrike(Rigidbody target)
    {
        cueBall = target;
        agent.enabled = true;
    }
}
