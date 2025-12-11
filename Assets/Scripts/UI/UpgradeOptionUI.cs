using UnityEngine;
using UnityEngine.UI;
using TMPro; // 引用 TextMeshPro

public class UpgradeOptionUI : MonoBehaviour {
    [Header("UI 组件引用")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button selectButton;

    // 设置卡片内容
    public void SetOption(UpgradeOption option) {
        // 1. 更新显示
        iconImage.sprite = option.upgradeIcon;
        nameText.text = option.upgradeName;
        descriptionText.text = option.upgradeDescription;

        // 2. 绑定点击事件
        // 先移除旧的监听，防止复用时点击一次触发多次
        selectButton.onClick.RemoveAllListeners();

        // 添加新的监听
        selectButton.onClick.AddListener(() => {
            // 执行 UpgradeManager 里定义的逻辑 (onSelect)
            option.onSelect?.Invoke();

            // 点击后按钮失效，防止快速连点
            selectButton.interactable = false;
        });

        // 确保按钮是可点的
        selectButton.interactable = true;
    }
}
