using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image healthFill; // phần màu đỏ của máu
    [SerializeField] private TextMeshProUGUI healthText; // text hiển thị máu (tuỳ chọn)

    /// <summary>
    /// Cập nhật thanh máu.
    /// </summary>
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthFill != null)
            healthFill.fillAmount = currentHealth / maxHealth;

        if (healthText != null)
            healthText.text = Mathf.CeilToInt(currentHealth).ToString(); // làm tròn nếu cần
    }
}
