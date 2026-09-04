using UnityEngine;

public class BumperCollision : MonoBehaviour
{
    public float bounceForce = 2f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("CueBall"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                contact.otherCollider.attachedRigidbody.AddForce(contact.impulse * bounceForce, ForceMode.Impulse);
            }
        }
    }
}
