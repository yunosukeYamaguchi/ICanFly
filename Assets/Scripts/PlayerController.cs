using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private bool isGrounded;

    private int leftInputCount = 0;
    private int rightInputCount = 0;

    private TimeLimitController timeLimitController;

    [SerializeField] private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timeLimitController = FindObjectOfType<TimeLimitController>();
    }

    void Update()
    {
        //タイムリミット内なら入力可
        if (timeLimitController != null && timeLimitController.inputEnabled)
        {
            Fly();
        }
        else if (isGrounded)
        {
            gameManager.ResultScene();
        }
    }

    void Fly()
    {
            // 左右キーが押されたらカウントアップ
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rightInputCount++;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            leftInputCount++;
        }

        // どちらかのキーが押された瞬間にジャンプ処理（着地中限定）
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            // 入力の差を求める
            int inputDiff = rightInputCount - leftInputCount;

            // 差に応じて角度を決める（正：左に曲がる、負：右に曲がる）
            float angle = Mathf.Clamp(inputDiff * 10f, -45f, 45f); // 最大 ±45度まで傾ける

            // 角度をラジアンに変換
            float angleRad = angle * Mathf.Deg2Rad;

            // ベクトルを計算（角度による方向）
            Vector2 jumpDirection = new Vector2(Mathf.Sin(angleRad), Mathf.Cos(angleRad)).normalized;

            // ジャンプ！
            rb.linearVelocity = jumpDirection * jumpForce;

            // カウントリセット
            leftInputCount = 0;
            rightInputCount = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
