using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FreeCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 5f;

    private float yaw = 0f;
    private float pitch = 0f;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.height = 1.0f;
        controller.radius = 0.5f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Vector3 startPosition = transform.position;
        startPosition.y = 2.0f;
        transform.position = startPosition;
    }

    void Update()
    {
        // Only rotate if right mouse button is held
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * lookSpeed;
            pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
            pitch = Mathf.Clamp(pitch, -90f, 90f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        // Movement input
        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f);
        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        if (Input.GetKey(KeyCode.Space)) move.y += 1f;
        if (Input.GetKey(KeyCode.LeftControl)) move.y -= 1f;

        // Apply movement through CharacterController for collision
        controller.Move(transform.TransformDirection(move) * speed * Time.deltaTime);

        // Keep from going underground
        if (transform.position.y < -0.5f)
        {
            Vector3 pos = transform.position;
            pos.y = -0.5f;
            transform.position = pos;
        }
    }
}
