using Unity.Cinemachine;
using UnityEngine;

public class PoolPocket : MonoBehaviour
{
    public ParticleSystem vfx;
    public CinemachineCamera camera;
    public int cameraPriority = 5;

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
        vfx.Play();
        camera.Priority = cameraPriority;
    }
}
