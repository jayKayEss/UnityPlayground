using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class CueStick : MonoBehaviour
{
    public static event Action<Vector3, float> OnCueTarget;
    public static event Action OnCueTargetLost;

    private static readonly int IsAimingHash = Animator.StringToHash("isAiming");
    private static readonly int IsShootingHash = Animator.StringToHash("isShooting");

    public LayerMask hitMask;
    public float maxBounceForce = 25f;
    public float aimSpeed = 5f;
    public float maxAimZDistance = 1.5f;
    public float maxAimYOffset = 0.8f;

    private Animator animator;
    private InputAction aimAction;
    private InputAction lookAction;
    private InputAction shootAction;

    private Vector3 initialPosition;
    private Vector3 aimPosition = Vector3.zero;
    private bool isAiming = false;
    private bool isShooting = false;
    private bool hasTarget = false;
    private float shootFactor = 0;
    private bool didShoot = false;
    private RaycastHit hit;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.localPosition;
    }

    void Start()
    {
        aimAction = InputSystem.actions.FindAction("Aim");
        lookAction = InputSystem.actions.FindAction("Look");
        shootAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ReadInputs();
        UpdateAim();
        FindTarget();
        DebugForceVector();
        ApplyForce();
    }

    private void ReadInputs()
    {
        bool wasAiming = isAiming;
        bool wasShooting = isShooting;

        isAiming = aimAction.IsPressed();
        isShooting = shootAction.IsPressed();

        animator.SetBool(IsAimingHash, isAiming);
        animator.SetBool(IsShootingHash, isShooting);

        if (wasShooting && !isShooting)
        {
            didShoot = false;
        }

        if (wasAiming && !isAiming)
        {
            ResetAim();
            OnCueTargetLost.Invoke();
        }
    }

    private void UpdateAim()
    {
        if (!isAiming)
        {
            return;
        }

        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        if (lookInput.magnitude > 0f)
        {
            aimPosition = Vector3.ClampMagnitude(
                new(0f, aimPosition.y + lookInput.y * aimSpeed * Time.deltaTime, 0f),
                0.8f
            );

            transform.localPosition = initialPosition + aimPosition;
        }
    }

    private void ResetAim()
    {
        transform.localPosition = initialPosition;
        aimPosition = Vector3.zero;
    }

    private void FindTarget()
    {
        if (!isAiming)
        {
            hasTarget = false;
            return;
        }

        if (
            Physics.Raycast(
                transform.position,
                transform.forward,
                out hit,
                maxAimZDistance,
                hitMask
            )
        )
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            shootFactor = 1 - hit.distance / maxAimZDistance;
            hasTarget = true;
            OnCueTarget.Invoke(hit.point, shootFactor);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * maxAimZDistance, Color.green);
            hasTarget = false;
            OnCueTargetLost.Invoke();
        }
    }

    private Vector3 ShootVector()
    {
        Vector3 shootVector = hit.point - transform.position;
        float shootForce = maxBounceForce * shootFactor;
        return shootForce * shootVector;
    }

    private void DebugForceVector()
    {
        if (isAiming && hasTarget)
        {
            Debug.DrawRay(hit.point, ShootVector(), Color.yellow);
        }
    }

    private void ApplyForce()
    {
        if (!(isShooting && hasTarget && !didShoot))
        {
            return;
        }

        hit.rigidbody.AddForceAtPosition(ShootVector(), hit.point, ForceMode.Impulse);

        didShoot = true;
    }
}
