using UnityEngine;

public class AssetFlip : MonoBehaviour
{
    [Header("Asset References")]
    public SpriteRenderer spriteRenderer; // สำหรับการกลับด้าน Sprite
    public AudioSource audioSource;       // สำหรับการกลับด้านเสียง (หากต้องการ)
    public GameObject targetObject;       // สำหรับการกลับด้าน GameObject ทั้งหมด

    private bool isFlipped = false;

    void Update()
    {
        // ตรวจสอบการกดปุ่ม Shift
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleFlip();
        }
    }

    void ToggleFlip()
    {
        isFlipped = !isFlipped;

        // กลับด้าน Sprite
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = isFlipped;
        }

        // กลับด้านเสียง (หากต้องการ)
        if (audioSource != null)
        {
            audioSource.pitch = isFlipped ? -1f : 1f; // เปลี่ยน pitch เพื่อกลับด้านเสียง
        }

        // กลับด้าน GameObject ทั้งหมด
        if (targetObject != null)
        {
            Vector3 scale = targetObject.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (isFlipped ? -1 : 1);
            targetObject.transform.localScale = scale;
        }
    }
}
