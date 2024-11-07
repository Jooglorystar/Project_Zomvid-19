using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;
    public float flashSpeed;

    private Color indicatorColor = new Color(1f, 50/255f, 50/255f);

    private Coroutine coroutine;

    void Start()
    {
        CharacterManager.Instance.player.condition.OnTakeDamage += Flash;
    }

    private void Flash()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        image.enabled = true;
        image.color = indicatorColor;
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.35f;
        float a = startAlpha;

        while(a>0.0f)
        {
            a -= (startAlpha/flashSpeed) * Time.deltaTime;
            image.color = new Color(indicatorColor.r, indicatorColor.g, indicatorColor.b, a);
            yield return null;
        }

        image.enabled = false;
    }
}
