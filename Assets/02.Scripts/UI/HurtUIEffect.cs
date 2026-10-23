using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HurtUIEffect : MonoBehaviour
{
    [SerializeField] private Image _hurtImage;
    [SerializeField, Min(0f)] private float _fadeInDuration = 0.1f;
    [SerializeField, Min(0f)] private float _holdDuration = 0.15f;
    [SerializeField, Min(0f)] private float _fadeOutDuration = 0.35f;

    private Coroutine _fadeCoroutine;
    private float _targetAlpha = 0.35f;

    private void Awake()
    {
        if (_hurtImage == null)
            _hurtImage = GetComponent<Image>();

        if (_hurtImage == null)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("HurtUIEffect needs an Image or an active Canvas in the scene.", this);
                enabled = false;
                return;
            }

            var overlay = new GameObject("HurtScreen", typeof(RectTransform), typeof(Image));
            overlay.transform.SetParent(canvas.transform, false);
            var rectTransform = (RectTransform)overlay.transform;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            _hurtImage = overlay.GetComponent<Image>();
            _hurtImage.color = new Color(1f, 0f, 0f, _targetAlpha);
            _hurtImage.raycastTarget = false;
        }

        if (_hurtImage.color.a > 0f)
            _targetAlpha = _hurtImage.color.a;

        SetAlpha(0f);
    }

    public void Play()
    {
        if (!enabled || _hurtImage == null)
            return;

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        yield return FadeTo(_targetAlpha, _fadeInDuration);
        yield return new WaitForSecondsRealtime(_holdDuration);
        yield return FadeTo(0f, _fadeOutDuration);
        _fadeCoroutine = null;
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = _hurtImage.color.a;
        if (duration <= 0f)
        {
            SetAlpha(targetAlpha);
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration));
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color color = _hurtImage.color;
        color.a = alpha;
        _hurtImage.color = color;
    }
}
