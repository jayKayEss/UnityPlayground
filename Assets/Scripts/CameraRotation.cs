using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
  [Header("Camera Settings")]
  public float lookSpeed = 2.5f;

  private Camera camera;
  private InputAction lookAction;

  void Start()
  {
    camera = GetComponent<Camera>();
    lookAction = InputSystem.actions.FindAction("Look");
  }

  void FixedUpdate()
  {
    Vector2 lookInput = lookAction.ReadValue<Vector2>();

    float xRotation = -lookInput.y * lookSpeed + camera.transform.localRotation.y;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    Quaternion rotation = Quaternion.Euler(new Vector3(xRotation, lookInput.x, 0f));
    camera.transform.localRotation = rotation;
    // camera.transform.Rotate(rotation);
  }
}