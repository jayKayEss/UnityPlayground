using UnityEngine;

// "spin force" or curve force. A.K.A Magnus effect: https://en.wikipedia.org/wiki/Magnus_effect

public class PoolBall : MonoBehaviour
{
    private Rigidbody rb;

    public float linearDampingFactor = 0.01f;
    public float angularDampingFactor = 0.01f;
    public Vector3 magnusForceScale = new(0.25f, 0.05f, 0.25f);
    public float minLinearVelocity = 0.01f;
    private float initialLinearDamping;
    private float initialAngularDamping;

    void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        initialLinearDamping = rb.linearDamping;
        initialAngularDamping = rb.angularDamping;
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > minLinearVelocity)
        {
            rb.linearDamping += linearDampingFactor * Time.fixedDeltaTime;
            rb.angularDamping += angularDampingFactor * Time.fixedDeltaTime;

            Vector3 rawMagnusForce = Vector3.Cross(-rb.linearVelocity, rb.angularVelocity);
            Vector3 magnusForce = Vector3.Scale(rawMagnusForce, magnusForceScale);
            Debug.DrawRay(rb.position, magnusForce, Color.orange);
            rb.AddForce(Time.fixedDeltaTime * magnusForce);
        }
        else
        {
            rb.linearVelocity = new(0f, 0f, 0f);
            rb.angularVelocity = new(0f, 0f, 0f);
            rb.linearDamping = initialLinearDamping;
            rb.angularDamping = initialAngularDamping;
        }
    }
}
