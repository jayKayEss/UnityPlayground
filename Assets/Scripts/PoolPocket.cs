using Unity.Cinemachine;
using UnityEngine;

public class PoolPocket : MonoBehaviour
{
    public int cameraPriority = 5;

    private ParticleSystem vfx;
    private CinemachineCamera camera;

    private void Awake()
    {
        vfx = GetComponentInChildren<ParticleSystem>();
        camera = GetComponentInChildren<CinemachineCamera>();
    }

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
