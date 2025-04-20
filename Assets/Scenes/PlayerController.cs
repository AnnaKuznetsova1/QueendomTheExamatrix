using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // �������� �������� �����/������
    public float jumpForce = 5f; // ���� ������
    public float climbSpeed = 3f; // �������� ������� �� ��������

    private Rigidbody2D rb;
    private bool isGrounded; // �� ����� �� �����
    private bool isClimbing; // ��������� �� ����� �� ��������
    private float originalGravityScale; // �������� ����������

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale; // ��������� �������� ����������
    }

    void Update()
    {
        // �������� �����/������
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // ������
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }

        // �������� �� ��������
        if (isClimbing)
        {
            float climbInput = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, climbInput * climbSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            Debug.Log("Hero is on the ladder!");
            isClimbing = true;
            rb.gravityScale = 0f; // ��������� ����������
            rb.velocity = new Vector2(rb.velocity.x, 0f); // ���������� ������������ ��������
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            Debug.Log("Hero left the ladder!");
            isClimbing = false;
            rb.gravityScale = originalGravityScale; // ���������� ����������
        }
    }
}