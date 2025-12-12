using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true,true)]
    [SerializeField]private Color flashColor = Color.white;
    [SerializeField] private float flashTime = 0.2f;

    [SerializeField] private SpriteRenderer sp;

    private Coroutine damageFlashCoroutine;
    private Material materialInstance;
    private HealthSystem healthSystem;

    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();
        materialInstance = sp.material;
    }

    private void Start() {
        if (healthSystem != null) {
            healthSystem.OnTakeDamage += CallDamageFlash;
        }
    }

    private void OnEnable() {
        ResetFlash();
    }

    private void OnDisable() {
        if (damageFlashCoroutine != null) {
            StopCoroutine(damageFlashCoroutine);
        }

        ResetFlash();
    }

    private void OnDestroy() {
        if (healthSystem != null) {
            healthSystem.OnTakeDamage -= CallDamageFlash;
        }
    }

    private void CallDamageFlash(float amount) {
        if (!gameObject.activeInHierarchy) return;

        if (damageFlashCoroutine != null) StopCoroutine(damageFlashCoroutine);

        damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher() {
        materialInstance.SetColor("_FlashColor", flashColor);

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < flashTime) {
            elapsedTime+= Time.deltaTime;

            currentFlashAmount=Mathf.Lerp(1f,0f,elapsedTime/flashTime);

            materialInstance.SetFloat("_FlashAmount",currentFlashAmount);

            yield return null;
        }
    }

    private void ResetFlash() {
        if (materialInstance != null) {
            materialInstance.SetFloat("_FlashAmount", 0f);
        }
    }
}
