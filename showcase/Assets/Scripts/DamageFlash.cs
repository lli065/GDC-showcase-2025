using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashTime = 0.25f;
    [SerializeField] private AnimationCurve flashSpeedCurve;

    private SpriteRenderer spriteRenderer;
    private Material material;

    private Coroutine damageFlashCoroutine;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Init();
    }

    private void Init() {
        material = spriteRenderer.material;
    }

    public void CallDamageFlash() {
        if (damageFlashCoroutine != null) {
            StopCoroutine(damageFlashCoroutine);
        }
        damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher() {
        material.SetColor("_FlashColor", flashColor);

        float currentFlashAmount = 0f;
        float elapsedTime = 0f; 
        while (elapsedTime < flashTime) {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, flashSpeedCurve.Evaluate(elapsedTime), elapsedTime / flashTime);

            material.SetFloat("_FlashAmount", currentFlashAmount);

            yield return null;
        }
        material.SetFloat("_FlashAmount", 0f);
    }
}
