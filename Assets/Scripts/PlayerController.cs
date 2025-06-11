using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private bool isGrounded;

    private TimeLimitController timeLimitController;

    [SerializeField] private GameManager gameManager;

    [Header("羽オブジェクトの設定")]
    public Transform rightWing;
    public Transform leftWing;

    [Header("羽ばたき設定")]
    public float baseSpeed = 10f;
    public float flapAngle = 20f;
    public float minFlapDuration = 0.05f;

    private Coroutine rightFlapCoroutine = null;
    private Coroutine leftFlapCoroutine = null;
    private float lastRightInputTime = 0f;
    private float lastLeftInputTime = 0f;

    [Header("プレイヤーのSpriteRenderer")]
    public SpriteRenderer playerSpriteRenderer;

    [Header("警告用の画像スプライト")]
    public Sprite warningSprite;

    private bool hasSwitchedImage = false;
    bool karaage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeLimitController = FindObjectOfType<TimeLimitController>();
        karaage = true;
    }

    void Update()
    {
        if (timeLimitController != null && timeLimitController.inputEnabled)
        {
            Fly();
        }
        else if (isGrounded)
        {
            if (playerSpriteRenderer != null && warningSprite != null && karaage)
            {
                playerSpriteRenderer.sprite = warningSprite;
                hasSwitchedImage = true;
                karaage = false;
            }
            gameManager.ResultScene();
        }

        if (rightWing != null) rightWing.gameObject.SetActive(!isGrounded);
        if (leftWing != null) leftWing.gameObject.SetActive(!isGrounded);

        // 「/ . : ;」キーが押されたら両羽を同時に回転（ジャンプ準備）
        if (Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.Colon) || Input.GetKeyDown(KeyCode.Semicolon))
        {
            // 右羽回転（右回転）
            if (rightFlapCoroutine != null) StopCoroutine(rightFlapCoroutine);
            rightFlapCoroutine = StartCoroutine(FlapOnce(rightWing, -flapAngle, baseSpeed));

            // 左羽回転（左回転）
            if (leftFlapCoroutine != null) StopCoroutine(leftFlapCoroutine);
            leftFlapCoroutine = StartCoroutine(FlapOnce(leftWing, flapAngle, baseSpeed));
        }

        // 「/ . : ;」キーが離されたら両羽リセット
        if (Input.GetKeyUp(KeyCode.Slash) || Input.GetKeyUp(KeyCode.Period) || Input.GetKeyUp(KeyCode.Colon) || Input.GetKeyUp(KeyCode.Semicolon))
        {
            // 右羽リセット
            if (rightFlapCoroutine != null)
            {
                StopCoroutine(rightFlapCoroutine);
                rightFlapCoroutine = null;
                if (rightWing != null) rightWing.localRotation = Quaternion.Euler(0, 0, 0);
            }

            // 左羽リセット
            if (leftFlapCoroutine != null)
            {
                StopCoroutine(leftFlapCoroutine);
                leftFlapCoroutine = null;
                if (leftWing != null) leftWing.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    void Fly()
    {
        Vector2 jumpDirection = Vector2.zero;

        // 「/ . : ;」キーが押されたら真上にジャンプ
        if (Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.Colon) || Input.GetKeyDown(KeyCode.Semicolon))
        {
            jumpDirection = Vector2.up;
        }

        if (jumpDirection != Vector2.zero)
        {
            rb.linearVelocity = jumpDirection * jumpForce;
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

    // 羽ばたき1回分の処理（回転アニメーション）
    System.Collections.IEnumerator FlapOnce(Transform wing, float angle, float speed)
    {
        if (wing == null) yield break;

        Quaternion startRot = Quaternion.Euler(0, 0, 0);
        Quaternion downRot = Quaternion.Euler(0, 0, angle);
        Quaternion upRot = startRot;

        float t = 0f;

        // 羽を下げる
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            wing.localRotation = Quaternion.Slerp(startRot, downRot, t);
            yield return null;
        }

        t = 0f;

        // 羽を戻す
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            wing.localRotation = Quaternion.Slerp(downRot, upRot, t);
            yield return null;
        }
    }
}