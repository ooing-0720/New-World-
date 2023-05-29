using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 캐릭터 이동을 위한 변수
    public float moveSpeed = 2;
    public float turnSpeed = 200;
    public float jumpForce = 4;

    private float mouseX;
    private float mouseY;
    private float currentV;

    [SerializeField] public Rigidbody rigidbody;
    [SerializeField] public Animator animator;
    private Camera playerCamera;

    private Vector3 cameraOffset;

    // Animator 처리를 위한 변수
    private List<Collider> collisions = new List<Collider>();
    private bool isGround;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 화면 중앙에 고정합니다.

        cameraOffset = playerCamera.transform.position - transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!collisions.Contains(collision.collider))
                {
                    collisions.Add(collision.collider);
                }
                isGround = true;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        RotatePlayer();
    }

    void GetInput()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }

    void RotatePlayer()
    {
        float rotationX = mouseX * turnSpeed * Time.deltaTime;
        float rotationY = mouseY * turnSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, rotationX); // 수평 회전은 플레이어의 Y 축을 기준으로 합니다.
        playerCamera.transform.Rotate(Vector3.left, rotationY); // 수직 회전은 카메라의 X 축을 기준으로 합니다.

        Quaternion cameraRotation = playerCamera.transform.localRotation;
        Quaternion playerRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
        playerCamera.transform.localRotation = cameraRotation;

        Vector3 desiredPosition = transform.position + playerRotation * cameraOffset;
        playerCamera.transform.position = desiredPosition;
    }

    void FixedUpdate()
    {
        animator.SetBool("Grounded", isGround);
        Move();
       // Jump();
    }

    void Move()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(inputX, 0, inputZ).normalized;

        if (dir.magnitude > 0)
        {
            Vector3 movement = dir * moveSpeed * 3 * Time.fixedDeltaTime;
            rigidbody.MovePosition(transform.position + transform.TransformDirection(movement));
        }

        currentV = Mathf.Lerp(currentV, inputX, Time.fixedDeltaTime * 10);
        animator.SetFloat("MoveSpeed", dir.magnitude);
    }
    /*
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    */
}