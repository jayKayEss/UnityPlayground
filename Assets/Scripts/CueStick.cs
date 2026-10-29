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
    public float maxOffset = -3;

    public float initialZPos = 1;

    public float maxBounceForce = 10f;
    public float aimSpeed = 40f;
    public float maxAimDistance = 2f;

    private Animator animator;
    private InputAction aimAction;
    private InputAction lookAction;
    private InputAction shootAction;

    private Quaternion initialRotation;
    private bool isAiming = false;
    private bool isShooting = false;
    private bool hasTarget = false;
    private float shootFactor = 0;
    private bool didShoot = false;
    private RaycastHit hit;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        initialRotation = transform.localRotation;
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
            Vector3 rotationVector = new(-lookInput.y, 0f, 0f);
            transform.Rotate(aimSpeed * Time.deltaTime * rotationVector);
        }
    }

    private void ResetAim()
    {
        transform.localRotation = initialRotation;
    }

    private void FindTarget()
    {
        if (!isAiming)
        {
            hasTarget = false;
            return;
        }

        if (
            Physics.Raycast(transform.position, transform.forward, out hit, maxAimDistance, hitMask)
        )
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            shootFactor = 1 - hit.distance / maxAimDistance;
            hasTarget = true;
            OnCueTarget.Invoke(hit.point, shootFactor);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * maxAimDistance, Color.green);
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
