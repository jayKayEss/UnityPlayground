using UnityEngine;

public class PoolPocket : MonoBehaviour
{
    public ParticleSystemRenderer vfx;
    public Material successVFX;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CueBall"))
        {
            Debug.Log("SCRATCH!!!");
        }
        else
        {
            OnSuccess();
        }
    }

    void OnSuccess()
    {
        Debug.Log("SUCCESS!!!");
        // vfx.material.color = Color.green;
        vfx.material = successVFX;
    }
}
