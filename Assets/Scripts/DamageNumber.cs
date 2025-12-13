using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(TextMeshPro))]
public class DamageNumber : PoolableObject {
    [Header("动画设置")]
    [SerializeField] private float floatDistance = 1.5f;
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private float fadeStartTime = 0.5f;

    [Header("缩放动画")]
    [SerializeField] private bool enableScaleAnimation = true;
    [SerializeField] private float startScale = 0.5f;
    [SerializeField] private float targetScale = 1.0f;
    [SerializeField] private float scaleDuration = 0.3f;

    private TextMeshPro textComponent;
    private Sequence animationSequence;

    private void Awake() {
        textComponent = GetComponent<TextMeshPro>();
    }

    public void SetDamageText(float damageAmount) {
        if (textComponent != null) {
            textComponent.text = Mathf.RoundToInt(damageAmount).ToString();
        }
    }

    public override void OnSpawn() {
        if (textComponent == null) return;

        // 杀死之前的动画
        animationSequence?.Kill();

        // 设置初始状态
        Color initialColor = textComponent.color;
        initialColor.a = 1f;
        textComponent.color = initialColor;

        if (enableScaleAnimation) {
            transform.localScale = Vector3.one * startScale;
        }

        // 创建动画序列
        animationSequence = DOTween.Sequence();

        // 向上浮动动画
        animationSequence.Append(
            transform.DOMoveY(transform.position.y + floatDistance, duration)
                .SetEase(Ease.OutQuad)
        );

        // 缩放动画（从小到大）
        if (enableScaleAnimation) {
            animationSequence.Join(//join表示和上一个动画同时开始
                transform.DOScale(targetScale, scaleDuration)
                    .SetEase(Ease.OutBack)
            );
        }

        // 淡出动画（延迟开始）
        animationSequence.Insert(fadeStartTime,
            textComponent.DOFade(0f, duration - fadeStartTime)
                .SetEase(Ease.InQuad)
        );

        // 动画完成后回收到对象池
        animationSequence.OnComplete(() => ReturnToPool());
    }

    public override void OnDespawn() {
        // 停止并清理动画
        animationSequence?.Kill();
        animationSequence = null;
    }
}
