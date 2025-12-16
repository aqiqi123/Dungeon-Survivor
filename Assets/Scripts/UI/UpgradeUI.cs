using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour {
    [Header("容器引用")]
    [SerializeField] private GameObject panelContainer;

    [Header("选项卡槽位")]
    [SerializeField] private List<UpgradeOptionUI> optionSlots;

    private void Awake() {
        panelContainer.SetActive(false);
    }

    // 由 UpgradeManager 调用
    public void OpenPanel(List<UpgradeOption> options) {
        panelContainer.SetActive(true);

        // 遍历 3 个槽位
        for (int i = 0; i < optionSlots.Count; i++) {
            if (i < options.Count) {
                // 如果有数据，显示按钮并填充内容
                optionSlots[i].gameObject.SetActive(true);
                optionSlots[i].SetOption(options[i]);
            } else {
                // 如果选项不足 3 个，隐藏多余的按钮
                optionSlots[i].gameObject.SetActive(false);
            }
        }
    }

    // 由 UpgradeManager 调用
    public void ClosePanel() {
        panelContainer.SetActive(false);
    }
}
