using UnityEngine;

public class BumperCollision : MonoBehaviour
{
    public float bounceForce = 10f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                contact.otherCollider.attachedRigidbody.AddForce(-1f * contact.normal * bounceForce, ForceMode.Impulse);
            }
        }
    }
}
