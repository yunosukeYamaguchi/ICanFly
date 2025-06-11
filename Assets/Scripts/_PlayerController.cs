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

    [Header("羽オブジェクトの設定")]
    public Transform rightWing;           // 右羽（インスペクターで設定）
    public Transform leftWing;            // 左羽（インスペクターで設定）

    [Header("羽ばたき設定")]
    public float baseSpeed = 10f;         // 羽ばたきの基本速度（連打速度の掛け算用）
    public float flapAngle = 20f;         // 羽ばたきの角度（Z軸回転）
    public float minFlapDuration = 0.05f; // 羽ばたきの最短速度（連打上限）

    private Coroutine rightFlapCoroutine = null; // 右羽の羽ばたき処理
    private Coroutine leftFlapCoroutine = null;  // 左羽の羽ばたき処理
    private float lastRightInputTime = 0f;
    private float lastLeftInputTime = 0f;

    [Header("プレイヤーのSpriteRenderer")]
    public SpriteRenderer playerSpriteRenderer; // プレイヤーのSpriteRendererを指定

    [Header("警告用の画像スプライト")]
    public Sprite warningSprite; // 残り5秒で切り替える画像

    private bool hasSwitchedImage = false; // 一度だけ切り替える用

    bool karaage; //唐揚げ化したか

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timeLimitController = FindObjectOfType<TimeLimitController>();

        karaage = true;
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
            if (playerSpriteRenderer != null && warningSprite != null && karaage)
                {
                    playerSpriteRenderer.sprite = warningSprite;
                    hasSwitchedImage = true;

                    karaage = false;
                }

            gameManager.ResultScene();
        }

        // 羽オブジェクトの表示切り替え
        if (rightWing != null) rightWing.gameObject.SetActive(!isGrounded);
        if (leftWing != null) leftWing.gameObject.SetActive(!isGrounded);

        // 右キーを押した瞬間に右羽ばたき（押しっぱなしは無効）
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            float now = Time.time;
            float interval = now - lastRightInputTime;
            lastRightInputTime = now;

            float flapDuration = Mathf.Max(interval, minFlapDuration);
            float flapSpeed = 1f / flapDuration * baseSpeed;

            if (rightFlapCoroutine != null)
                StopCoroutine(rightFlapCoroutine);

            rightFlapCoroutine = StartCoroutine(FlapOnce(rightWing, -flapAngle, flapSpeed));
        }

        // 左キーを押した瞬間に左羽ばたき（押しっぱなしは無効）
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            float now = Time.time;
            float interval = now - lastLeftInputTime;
            lastLeftInputTime = now;

            float flapDuration = Mathf.Max(interval, minFlapDuration);
            float flapSpeed = 1f / flapDuration * baseSpeed;

            if (leftFlapCoroutine != null)
                StopCoroutine(leftFlapCoroutine);

            leftFlapCoroutine = StartCoroutine(FlapOnce(leftWing, flapAngle, flapSpeed));
        }

        // 右キーを離したら右羽ばたきを即停止＆角度リセット
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (rightFlapCoroutine != null)
            {
                StopCoroutine(rightFlapCoroutine);
                rightFlapCoroutine = null;
                if (rightWing != null)
                    rightWing.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        // 左キーを離したら左羽ばたきを即停止＆角度リセット
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (leftFlapCoroutine != null)
            {
                StopCoroutine(leftFlapCoroutine);
                leftFlapCoroutine = null;
                if (leftWing != null)
                    leftWing.localRotation = Quaternion.Euler(0, 0, 0);
            }
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
