using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AiInteraction : MonoBehaviour
{
    public GameObject AiCharacter = null; // AI 캐릭터
    public List<Dialogue> dialogues; // 여러 대화 데이터를 담을 리스트
    public DialogueManager dialogueManager; // 대화 시스템 관리자
    private bool isDialogueActive = false; // 대화가 활성화되었는지 확인하는 플래그
    private int currentDialogueIndex = 0; // 현재 대화의 인덱스
    public float interactionRange = 5f; // 상호작용 가능 범위
    public Vector3 aiTargetPosition = new Vector3(0f, 0f, 0f); // AI 캐릭터가 이동할 목표 위치

    void Start()
    {
        if (dialogueManager != null)
        {
            dialogueManager.SetAiInteraction(this);        
        }
    }

    void Update()
    {
        // E 키를 누르고, AiCharacter가 null이 아니며, 상호작용 범위 안에 있을 때만 대화를 시작하거나 이어갈 수 있음
        if (Input.GetKeyDown(KeyCode.E) && AiCharacter != null && IsWithinInteractionRange())
        {
            if (!isDialogueActive)
            {
                if (dialogueManager != null && currentDialogueIndex < dialogues.Count)
                {
                    dialogueManager.StartDialogue(dialogues[currentDialogueIndex]);
                    isDialogueActive = true;
                }
            }
            else
            {
                dialogueManager.DisplayNextSentence();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // AI 캐릭터 태그를 감지하도록 변경
        if (other.gameObject.CompareTag("AI"))
        {
            AiCharacter = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 플레이어가 AI 캐릭터를 떠날 때
        if (other.gameObject.CompareTag("AI"))
        {
            AiCharacter = null;
            isDialogueActive = false; // 대화 비활성화
        }
    }

    public void OnDialogueEnd()
    {
        isDialogueActive = false;
        currentDialogueIndex = (currentDialogueIndex + 1) % dialogues.Count;
        MoveAiCharacterToTarget(dialogues[currentDialogueIndex].aiTargetPosition, dialogues[currentDialogueIndex].aiTargetRotation);
    }

        // private void MoveAiCharacterToTarget(Vector3 targetPosition)
        // {
        //     // AI 캐릭터의 위치를 목표 위치로 이동시킴
        //     AiCharacter.transform.position = targetPosition;
        // }

    private void MoveAiCharacterToTarget(Vector3 targetPosition, Quaternion targetRotation)
    {
        // AI 캐릭터의 위치와 회전을 목표 값으로 설정
        AiCharacter.transform.position = targetPosition;
        AiCharacter.transform.rotation = targetRotation;
    }


    private bool IsWithinInteractionRange()
    {
        // 플레이어와 AI 캐릭터 간의 거리가 상호작용 범위 내에 있는지 확인
        return Vector3.Distance(transform.position, AiCharacter.transform.position) <= interactionRange;
    }

    private void MoveAiCharacterToTarget()
    {
        // AI 캐릭터의 위치를 목표 위치로 이동시킴
        AiCharacter.transform.position = aiTargetPosition;
    }
}

// using UnityEngine;
// using System.Collections.Generic;
// using System.Collections;

// public class AiInteraction : MonoBehaviour
// {
//     public GameObject AiCharacter = null; // AI 캐릭터
//     public List<Dialogue> dialogues; // 여러 대화 데이터를 담을 리스트
//     public DialogueManager dialogueManager; // 대화 시스템 관리자
//     private bool isDialogueActive = false; // 대화가 활성화되었는지 확인하는 플래그
//     private int currentDialogueIndex = 0; // 현재 대화의 인덱스
//     public float interactionRange = 5f; // 상호작용 가능 범위

//     void Start()
//     {
//         if (dialogueManager != null)
//         {
//             dialogueManager.SetAiInteraction(this); 
//         }
//     }

//     void Update()
//     {
//         // E 키를 누르고, AiCharacter가 null이 아니며, 상호작용 범위 안에 있을 때만 대화를 시작하거나 이어갈 수 있음
//         if (Input.GetKeyDown(KeyCode.E) && AiCharacter != null && IsWithinInteractionRange())
//         {
//             if (!isDialogueActive)
//             {
//                 if (dialogueManager != null && currentDialogueIndex < dialogues.Count)
//                 {
//                     dialogueManager.StartDialogue(dialogues[currentDialogueIndex]);
//                     isDialogueActive = true;
//                 }
//             }
//             else
//             {
//                 dialogueManager.DisplayNextSentence();
//             }
//         }
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         // AI 캐릭터 태그를 감지하도록 변경
//         if (other.gameObject.CompareTag("AI"))
//         {
//             AiCharacter = other.gameObject;
//         }
//     }

//     void OnTriggerExit(Collider other)
//     {
//         // 플레이어가 AI 캐릭터를 떠날 때
//         if (other.gameObject.CompareTag("AI"))
//         {
//             AiCharacter = null;
//             isDialogueActive = false; // 대화 비활성화
//         }
//     }

//     public void OnDialogueEnd()
//     {
//         isDialogueActive = false;
//         currentDialogueIndex = (currentDialogueIndex + 1) % dialogues.Count; // 다음 대화로 넘어감
//     }

//     private bool IsWithinInteractionRange()
//     {
//         // 플레이어와 AI 캐릭터 간의 거리가 상호작용 범위 내에 있는지 확인
//         return Vector3.Distance(transform.position, AiCharacter.transform.position) <= interactionRange;
//     }
// }


// using UnityEngine;
// using System.Collections.Generic;
// using System.Collections;

// public class AiInteraction : MonoBehaviour
// {
//     public GameObject AiCharacter = null; // AI 캐릭터
//     public List<Dialogue> dialogues; // 여러 대화 데이터를 담을 리스트
//     public DialogueManager dialogueManager; // 대화 시스템 관리자
//     private bool isDialogueActive = false; // 대화가 활성화되었는지 확인하는 플래그
//     private int currentDialogueIndex = 0; // 현재 대화의 인덱스

//     void Start()
//     {
//         if (dialogueManager != null)
//         {
//             dialogueManager.SetAiInteraction(this); 
//         }
//     }

//     void Update()
//     {
//         // E 키를 누르고, AiCharacter가 null이 아닐 때만 대화를 시작하거나 이어갈 수 있음
//         if (Input.GetKeyDown(KeyCode.E) && AiCharacter != null)
//         {
//             if (!isDialogueActive)
//             {
//                 if (dialogueManager != null && currentDialogueIndex < dialogues.Count)
//                 {
//                     dialogueManager.StartDialogue(dialogues[currentDialogueIndex]);
//                     isDialogueActive = true;
//                 }
//             }
//             else
//             {
//                 dialogueManager.DisplayNextSentence();
//             }
//         }
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         // AI 캐릭터 태그를 감지하도록 변경
//         if (other.gameObject.CompareTag("AI"))
//         {
//             AiCharacter = other.gameObject;
//         }
//     }

//     void OnTriggerExit(Collider other)
//     {
//         // 플레이어가 AI 캐릭터를 떠날 때
//         if (other.gameObject.CompareTag("AI"))
//         {
//             AiCharacter = null;
//             isDialogueActive = false; // 대화 비활성화
//         }
//     }

//     public void OnDialogueEnd()
//     {
//         isDialogueActive = false;
//         currentDialogueIndex = (currentDialogueIndex + 1) % dialogues.Count; // 다음 대화로 넘어감
//     }
// }