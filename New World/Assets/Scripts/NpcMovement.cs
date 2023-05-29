using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    public float moveSpeed = 2f; // NPC 이동 속도
    public float minX = -10f; // 이동 가능한 최소 X 좌표
    public float maxX = 10f; // 이동 가능한 최대 X 좌표
    public float minY = -10f; // 이동 가능한 최소 Y 좌표
    public float maxY = 10f; // 이동 가능한 최대 Y 좌표
    public float avoidDistance = 2f; // 장애물 회피 거리

    private Vector3 targetPosition; // 다음 목표 위치
    private Vector3 moveDirection; // 이동 방향

    private Rigidbody rb; // Rigidbody 컴포넌트

    // 애니메이션 적용
    [SerializeField] public Animator animator;
    private List<Collider> collisions = new List<Collider>();
    private bool isGround;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기

        // 초기 목표 위치 및 이동 방향 설정
        targetPosition = GetRandomPosition();
        moveDirection = (targetPosition - transform.position).normalized;

        // 일정한 간격마다 방향 변경 메서드 호출
        InvokeRepeating("ChangeDirection", 5f, 5f);

        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector3 velocity = moveDirection * moveSpeed;

        // 속도를 Rigidbody에 적용하여 이동
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        // 목표 위치와의 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // 목표 위치에 도달하면 다음 목표 위치 설정
        if (distanceToTarget < 0.1f)
        {
            targetPosition = GetRandomPosition();
            moveDirection = (targetPosition - transform.position).normalized;
        }

        // 몸통을 이동 방향을 바라보도록 설정
        transform.LookAt(transform.position + moveDirection);

        // 이동 방향 기준으로 전방에 장애물이 있는지 검사
        RaycastHit hit;
        if (Physics.Raycast(transform.position, moveDirection, out hit, avoidDistance))
        {
            // 장애물이 감지되면 방향 변경
            moveDirection = GetAvoidDirection(hit.normal);
        }

        animator.SetBool("Grounded", isGround);
    }

    void ChangeDirection()
    {
        // 일정 간격마다 이동 방향을 랜덤하게 변경
        targetPosition = GetRandomPosition();
        moveDirection = (targetPosition - transform.position).normalized;
        animator.SetFloat("MoveSpeed", moveDirection.magnitude);
    }

    Vector3 GetRandomPosition()
    {
        // 랜덤한 X, Y 좌표 생성
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // 생성된 좌표를 Vector3 형태로 반환
        return new Vector3(randomX, randomY, 0f);
    }


    void OnCollisionEnter(Collision collision)
    {
        // 충돌 발생 시 이동 방향을 랜덤하게 변경
        moveDirection = GetRandomDirection();

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

    Vector3 GetRandomDirection()
    {
        // 랜덤한 이동 방향 생성
        float randomAngle = Random.Range(0f, 360f);
        Vector3 randomDirection = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward;

        // 생성된 방향을 정규화하여 반환
        return randomDirection.normalized;
    }

    Vector3 GetAvoidDirection(Vector3 obstacleNormal)
    {
        // 장애물의 법선 벡터를 기반으로 피하는 방향을 계산
        Vector3 avoidDirection = Vector3.Reflect(moveDirection, obstacleNormal).normalized;

        // 생성된 피하는 방향을 반환
        return avoidDirection;
    }
}