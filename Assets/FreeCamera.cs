using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 5f;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Set the initial spawn height to 3
        Vector3 startPosition = transform.position;
        startPosition.y = 3f;
        transform.position = startPosition;
    }

    void Update()
    {
        // Only rotate if right mouse button is held (like in Scene view)
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * lookSpeed;
            pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
            pitch = Mathf.Clamp(pitch, -90f, 90f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        // Move even without holding right mouse
        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f);
        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        if (Input.GetKey(KeyCode.Space)) move.y += 1f;
        if (Input.GetKey(KeyCode.LeftControl)) move.y -= 1f;

        // Apply movement
        transform.Translate(move * speed * Time.deltaTime);

        // Clamp the vertical position to not go below 0
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Max(clampedPosition.y, 0f);
        transform.position = clampedPosition;
    }
}
