using UnityEngine;
using UnityEngine.UI;

public class ImageShow : MonoBehaviour
{
    public RawImage imageObject;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;

    private CanvasGroup canvasGroup;
    private bool isFadingIn = false;
    private bool isFadingOut = false;

    private void Start()
    {
        canvasGroup = imageObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            // RawImage에 CanvasGroup 컴포넌트가 없는 경우, 새로 추가
            canvasGroup = imageObject.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0f; // 초기에 이미지를 완전히 투명하게 설정
    }

    private void Update()
    {
        if (isFadingIn)
        {
            FadeIn(Time.deltaTime);
        }
        else if (isFadingOut)
        {
            FadeOut(Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartFadeIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartFadeOut();
        }
    }

    private void StartFadeIn()
    {
        isFadingIn = true;
        isFadingOut = false;
        imageObject.gameObject.SetActive(true);
    }

    private void StartFadeOut()
    {
        isFadingIn = false;
        isFadingOut = true;
    }

    private void FadeIn(float deltaTime)
    {
        canvasGroup.alpha += deltaTime / fadeInDuration;
        if (canvasGroup.alpha >= 1f)
        {
            canvasGroup.alpha = 1f;
            isFadingIn = false;
        }
    }

    private void FadeOut(float deltaTime)
    {
        canvasGroup.alpha -= deltaTime / fadeOutDuration;
        if (canvasGroup.alpha <= 0f)
        {
            canvasGroup.alpha = 0f;
            isFadingOut = false;
            imageObject.gameObject.SetActive(false);
        }
    }
}