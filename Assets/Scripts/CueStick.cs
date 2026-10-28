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
    public float maxAimDistance = 2f;

    private Animator animator;
    private InputAction aimAction;
    private InputAction shootAction;

    private bool isAiming = false;
    private bool isShooting = false;
    private bool hasTarget = false;
    private float shootFactor = 0;
    private bool didShoot = false;
    private RaycastHit hit;

    void Start()
    {
        animator = GetComponent<Animator>();
        aimAction = InputSystem.actions.FindAction("Aim");
        shootAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateAim();
        FindTarget();
        ApplyForce();
    }

    private void UpdateAim()
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
            OnCueTargetLost.Invoke();
        }
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

    private void ApplyForce()
    {
        if (!(isShooting && hasTarget && !didShoot))
        {
            return;
        }

        Vector3 shootVector = hit.point - transform.position;
        float shootForce = maxBounceForce * shootFactor;
        hit.rigidbody.AddForce(shootForce * shootVector, ForceMode.Impulse);

        didShoot = true;

        Debug.Log($"SHOOT FORCE: {shootForce} ({shootFactor}) ({hit.distance})");
    }
}
