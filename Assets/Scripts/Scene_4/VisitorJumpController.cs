using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class VisitorJumpController : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite idleSprite; // Ảnh bình thường (Visitor)
    [SerializeField] private Sprite jumpSprite; // Ảnh duỗi thẳng hướng thẳng đứng (+Y)

    [Header("Waypoints")]
    [SerializeField] private List<Transform> waypoints; // 3 điểm di chuyển

    [Header("Settings")]
    [SerializeField] private float squishDuration = 0.15f; // Thời gian nhún
    [SerializeField] private float moveDurationPerSegment = 0.5f; // Thời gian bay qua mỗi điểm
    [SerializeField] private KeyCode triggerKey = KeyCode.S; // Phím kích hoạt

    // Scale gốc của bạn: X và Y âm để lật ảnh ban đầu
    private readonly Vector3 originalScale = new Vector3(-0.37f, -0.3f, 1f);

    private SpriteRenderer spriteRenderer;
    private bool isJumping = false;

    private void Awake()
    {
        // Lấy SpriteRenderer ngay trên GameObject này
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Thiết lập trạng thái ban đầu: Vị trí tại điểm 1, góc xoay phẳng
        transform.localScale = originalScale;
        transform.rotation = Quaternion.identity;
        if (idleSprite != null)
        {
            spriteRenderer.sprite = idleSprite;
        }

        if (waypoints != null && waypoints.Count > 0)
        {
            transform.position = waypoints[0].position;
        }
    }

    private void Update()
    {
        // Kiểm tra ấn phím S và chưa trong trạng thái nhảy
        if (Input.GetKeyDown(triggerKey) && !isJumping)
        {
            StartJumpSequence();
        }
    }

    [ContextMenu("Play Jump")]
    public void StartJumpSequence()
    {
        if (waypoints == null || waypoints.Count < 3)
        {
            Debug.LogWarning("Cần đủ 3 điểm waypoint!");
            return;
        }

        isJumping = true;

        // Đặt vị trí về waypoint[0] trước khi nhảy
        transform.position = waypoints[0].position;
        transform.rotation = Quaternion.identity; // Đảm bảo góc xoay phẳng ban đầu

        Sequence jumpSeq = DOTween.Sequence();

        // 1. Nhún xuống (Anticipation)
        Vector3 squishScale = new Vector3(originalScale.x * 1.2f, originalScale.y * 0.7f, originalScale.z);
        jumpSeq.Append(transform.DOScale(squishScale, squishDuration).SetEase(Ease.OutQuad));

        // 2. Nảy lên & Đổi ảnh nhảy
        Vector3 stretchScale = new Vector3(originalScale.x * 0.8f, originalScale.y * 1.2f, originalScale.z);
        jumpSeq.Append(transform.DOScale(stretchScale, squishDuration).SetEase(Ease.InQuad));
        jumpSeq.AppendCallback(() =>
        {
            if (jumpSprite != null)
            {
                spriteRenderer.sprite = jumpSprite;
            }
        });

        // 3. Trả về Scale chuẩn khi bắt đầu bay
        jumpSeq.Append(transform.DOScale(originalScale, 0.1f));

        // 4. Di chuyển mượt theo đường cong CatmullRom qua 3 điểm
        Vector3[] pathPositions = new Vector3[waypoints.Count];
        for (int i = 0; i < waypoints.Count; i++)
        {
            pathPositions[i] = waypoints[i].position;
        }

        float totalDuration = moveDurationPerSegment * (waypoints.Count - 1);

        // -- SỬA LỖI MẤT HÌNH: Dùng hàm Update thủ công để xoay chỉ trục Z --
        jumpSeq.Append(
            transform.DOPath(pathPositions, totalDuration, PathType.CatmullRom)
                .SetEase(Ease.InOutSine)
                .OnUpdate(LookAtMovingDirection) // Gọi hàm xoay mỗi frame
        );

        // 5. Tiếp đất: Đổi về Idle Sprite & Trả về góc xoay phẳng & Nhún nhẹ
        jumpSeq.OnComplete(() =>
        {
            if (idleSprite != null)
            {
                spriteRenderer.sprite = idleSprite;
            }
            transform.rotation = Quaternion.identity; // Reset góc xoay Z về 0

            Sequence landSeq = DOTween.Sequence();
            landSeq.Append(transform.DOScale(squishScale, 0.1f).SetEase(Ease.OutQuad));
            landSeq.Append(transform.DOScale(originalScale, 0.1f).SetEase(Ease.InQuad));
            landSeq.OnComplete(() =>
            {
                isJumping = false;
            });
        });
    }

    /// <summary>
    /// Hàm tính toán xoay chỉ trục Z dựa trên hướng di chuyển trong không gian 2D.
    /// Giúp sprite không bao giờ quay mặt về phía sau (lỗi culling) hoặc quay cạnh mỏng về phía camera.
    /// </summary>
    private void LookAtMovingDirection()
    {
        if (transform.position == waypoints[0].position) return; // Không xoay khi chưa di chuyển

        // 1. Tính hướng di chuyển tức thời (Current Position - Previous Position ở frame trước)
        // DOTween tự xử lý việc thay đổi vị trí, chúng ta tính hướng từ vị trí đó.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 velocity;
        if (rb != null)
        {
            velocity = rb.velocity;
        }
        else
        {
            // Fallback nếu không dùng Rigidbody, tính thủ công bằng Vector2.MoveTowards không chính xác bằng,
            // nhưng ta có thể dùng `transform.DOPath().SetLookAt(0.01f)` bị lỗi cũ, nên đây là cách thay thế:
            // Chúng ta lấy vị trí dự kiến tiếp theo của Path (hơi phức tạp nếu tính thủ công).
            // Cách đơn giản nhất là dùng `.SetLookAt()` nhưng với hướng cố định.
            // Vì bạn không dùng Rigidbody, ta buộc phải dùng một trick là tính hướng tới điểm tiếp theo:
            // Nhưng DOPath CatmullRom không đi thẳng.

            // GIẢI PHÁP ĐƠN GIẢN NHẤT KHI KHÔNG DÙNG RIGIDBODY2D:
            // Quay trở lại `.SetLookAt()` nhưng thay vì để nó tự xoay toàn bộ, ta chỉ lấy góc xoay Z.
            // Tuy nhiên DOPath SetLookAt xoay thẳng Transform.
            // Vậy cách tốt nhất là chúng ta tự tính hướng dựa trên path:
            return; // Ta sẽ xử lý ở hàm OnUpdatePath thay thế
        }
    }

    // Do bạn dùng DOPath và có thể không có Rigidbody2D để lấy velocity chính xác tức thời, 
    // chúng ta sẽ dùng trick là tính vector hướng từ vị trí frame này so với vị trí frame trước:
    private Vector3 lastPosition;
    private void FixedUpdate()
    {
        if (isJumping)
        {
            Vector3 currentPos = transform.position;
            if (currentPos != lastPosition)
            {
                Vector3 moveDirection = (currentPos - lastPosition).normalized;

                // Nếu moveDirection rất nhỏ (gần như đứng yên), không xoay
                if (moveDirection.sqrMagnitude > 0.001f)
                {
                    // TÍNH GÓC XOAY Z (Chỉ 2D)
                    // Ảnh của bạn hướng thẳng đứng lên (+Y), nên angleOffset = -90 để 
                    // đỉnh đầu (+Y) hướng theo direction.
                    float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

                    // Do Scale âm của bạn (X=-0.37, Y=-0.3), ảnh có thể bị ngược.
                    // Với scale Y âm, "đỉnh đầu" thực sự quay xuống dưới.
                    // Ta cần offset lại. Thử offset -90.
                    transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
                }
            }
            lastPosition = currentPos;
        }
    }
}