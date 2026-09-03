using UnityEngine;

public class CueBall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("CUE BALL COLLISION " + collision.gameObject.tag);
        // if (collision.gameObject.CompareTag("Obstacle"))
        // {
        //     foreach (ContactPoint contact in collision.contacts)
        //     {
        //         contact.otherCollider.attachedRigidbody.AddForce(-1f * contact.normal * bounceForce, ForceMode.Impulse);
        //     }
        // }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("CUE BALL TRIGGER " + other.tag);
    }
}
