using UnityEngine;
using UnityEngine.InputSystem;

public class CueStick : MonoBehaviour
{
    public float maxOffset = -3;

    public float initialZPos = 1;

    public float maxBounceForce = 10f;

    private InputAction aimAction;
    private InputAction shootAction;
    private float bounceForce = 0f;
    private bool didShoot = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aimAction = InputSystem.actions.FindAction("Aim");
        shootAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        float aimInput = aimAction.ReadValue<float>();
        bool isShooting = shootAction.WasPressedThisFrame();

        if (isShooting && bounceForce > 0f)
        {
            transform.localPosition = new Vector3(0f, 0f, initialZPos);
            didShoot = true;
        }
        else if (!didShoot)
        {
            float zPos = Mathf.Lerp(0f, maxOffset, aimInput);

            transform.localPosition = new Vector3(0f, 0f, initialZPos + zPos);
            bounceForce = Mathf.Lerp(0f, maxBounceForce, aimInput);
        }
        else if (aimInput <= 0.01f)
        {
            didShoot = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (didShoot && other.gameObject.CompareTag("CueBall"))
        {
            Vector3 shootVector = other.gameObject.transform.position - transform.position;
            Rigidbody otherBody = other.gameObject.GetComponent<Rigidbody>();
            otherBody.AddForce(shootVector * bounceForce, ForceMode.Impulse);
        }
    }
}