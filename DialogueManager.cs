using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public List<string> sentences;
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public Vector3 aiTargetPosition;
    public Quaternion aiTargetRotation;
}

public class DialogueManager : MonoBehaviour
{
    public RawImage backgroundImage;
    public GameObject player;
    public GameObject playerCamera;
    public Text dialogueText;
    private Queue<string> sentences;

    private AiInteraction aiInteraction;

    public Transform cameraTransform;
    public Quaternion fixedCameraRotation;

    public Image fadePanel; // 페이드 패널

    void Start()
    {
        backgroundImage.enabled = false;
        sentences = new Queue<string>();
        fadePanel.gameObject.SetActive(false); // 페이드 패널을 처음에는 비활성화
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        fadePanel.gameObject.SetActive(true); // 페이드 패널 활성화

        Color color = fadePanel.color;
        float startAlpha = color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadePanel.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadePanel.color = color;

        if (targetAlpha == 0)
        {
            fadePanel.gameObject.SetActive(false); // 페이드 아웃 완료 후 패널 비활성화
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //플레이어 움직임 즉시 정지
        player.GetComponent<FirstPersonMovement>().enabled = false;
        player.GetComponent<Jump>().enabled = false;
        player.GetComponent<Crouch>().enabled = false;
        playerCamera.GetComponent<FirstPersonLook>().enabled = false;

        // 물리적 제약 추가
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;


        StartCoroutine(FadeAndStartDialogue(dialogue));

        //backgroundImage.enabled = true;
    }

    IEnumerator FadeAndStartDialogue(Dialogue dialogue)
    {
        yield return StartCoroutine(FadeTo(1, 0.3f)); // 페이드 아웃
        yield return new WaitForSeconds(0.5f); // 페이드 아웃 유지 시간

        // 플레이어 위치와 회전 즉시 설정
        player.transform.position = dialogue.playerPosition;
        player.transform.rotation = dialogue.playerRotation;
        cameraTransform.localRotation = fixedCameraRotation;
        
        backgroundImage.enabled = true;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        yield return StartCoroutine(FadeTo(0, 0.3f)); // 페이드 인

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        StartCoroutine(FadeAndEndDialogue());
    }

    IEnumerator FadeAndEndDialogue()
    {
        backgroundImage.enabled = false;
        dialogueText.text = "";

        yield return StartCoroutine(FadeTo(1, 0.3f)); // 페이드 아웃
        yield return new WaitForSeconds(0.5f); // 페이드 아웃 유지 시간

        player.GetComponent<FirstPersonMovement>().enabled = true;
        player.GetComponent<Jump>().enabled = true;
        player.GetComponent<Crouch>().enabled = true;
        playerCamera.GetComponent<FirstPersonLook>().enabled = true;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Y축 회전만 풀어줌

        if (aiInteraction != null)
        {
            aiInteraction.OnDialogueEnd();
        }

        yield return StartCoroutine(FadeTo(0, 0.3f)); // 페이드 인
    }

    public void SetAiInteraction(AiInteraction interaction)
    {
        aiInteraction = interaction;
    }
}
