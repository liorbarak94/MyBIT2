using UnityEngine;

public class PlayerMovmentScript : MonoBehaviour
{
    public Joystick joystick_move;
    public Joystick joystick_rotate;

    public Transform playerBody;

    public float rotationSpeed = 100f;
    public float moveSpeed = 100f;

    public float xRotation;
    public float yRotation;

    public float playerSpeed;

    public CharacterController characterController;
    private Vector3 move;

    void Start()
    {
        if (joystick_move == null)
        {
            joystick_move = FindObjectOfType<Joystick>();
        }
        if (joystick_rotate == null)
        {
            joystick_rotate = FindObjectOfType<Joystick>();
        }
    }

    void Update()
    {
        if (joystick_move.gameObject.activeSelf)
        {
            MovePlayer();
        }
        if (joystick_rotate.gameObject.activeSelf)
        {
            RotatePlayer();
        }
    }

    private void MovePlayer()
    {
        float horizontal_moveX = joystick_move.Horizontal;
        float vertical_moveZ = joystick_move.Vertical;

        move = transform.right * moveSpeed * Time.deltaTime * horizontal_moveX +
            transform.forward * moveSpeed * Time.deltaTime * vertical_moveZ;
        move *= playerSpeed;

        transform.localPosition += move;
    }

    private void RotatePlayer()
    {
        float horizontal_RotateX = joystick_rotate.Horizontal * rotationSpeed * Time.deltaTime;
        float vertical_RotateY = joystick_rotate.Vertical * rotationSpeed * Time.deltaTime;

        xRotation -= vertical_RotateY;
        yRotation += horizontal_RotateX;

        xRotation = Mathf.Clamp(xRotation, -50f, 50f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        playerBody.Rotate(Vector3.up * horizontal_RotateX);
    }
}
