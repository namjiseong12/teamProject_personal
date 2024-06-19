using UnityEngine;
using System.Collections;

public class KeyPlayer : MonoBehaviour
{
    public bool hasKey = false; // 플레이어가 키를 가지고 있는지 여부

    // private GameObject KeyButton = null;

    // public Material glowMaterial; // 빛나는 효과를 줄 머티리얼
    // private Material defaultMaterial; // 버튼의 기본 머티리얼

    private GameObject currentInteractableButton = null;
    private Renderer currentButtonRenderer = null;
    private Material[] defaultMaterials; // 버튼의 기본 메테리얼들
    private Material[] glowMaterials; // 빛나는 효과를 줄 메테리얼들

    public Material glowMaterial; // 빛나는 효과를 줄 머티리얼을 인스펙터에서 할당할 수 있도록 public으로 선언

    public GameObject door; // 이동시킬 문 오브젝트
    public Vector3 moveDirection; // 문의 이동 방향
    public float moveDistance; // 문의 이동 거리
    public float moveSpeed; // 문의 이동 속도

    private bool isMoving = false; // 문이 현재 이동 중인지 여부
    private bool doorOpened = false; // 문이 이미 열렸는지 여부

    private void OnTriggerEnter(Collider other)
    {
        // 키와 버튼 상호작용 처리
        if (other.CompareTag("Key"))
        {
            hasKey = true; // 키를 획득
            Destroy(other.gameObject); // 키 오브젝트 제거
        }
        else if (other.CompareTag("KeyButton") && !doorOpened && hasKey ) // 버튼의 태그를 확인하고, 문이 아직 열리지 않았다면
        {
            currentInteractableButton = other.gameObject; // 상호작용 가능한 버튼으로 설정.
            currentButtonRenderer = currentInteractableButton.GetComponent<Renderer>();
            defaultMaterials = currentButtonRenderer.materials; // 버튼의 기본 메테리얼들 저장
            glowMaterials = new Material[defaultMaterials.Length]; // 빛나는 효과를 줄 메테리얼들 생성
            for (int i = 0; i < defaultMaterials.Length; i++)
            {
                glowMaterials[i] = glowMaterial; // 빛나는 효과를 줄 메테리얼 설정
            }
            currentButtonRenderer.materials = glowMaterials; // 빛나는 효과 적용
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentInteractableButton) // 상호작용 중이던 버튼에서 벗어난 경우
        {
            // 기본 메테리얼로 복원
            currentButtonRenderer.materials = defaultMaterials;
            currentInteractableButton = null; // 상호작용 가능한 버튼을 초기화.
            currentButtonRenderer = null;
        }
    }

    void Update()
    {
        // 플레이어가 버튼 근처에 있고, "E" 키를 누르면 버튼을 활성화
        if (currentInteractableButton != null && Input.GetKeyDown(KeyCode.E) && hasKey && !isMoving && !doorOpened)
        {
            StartCoroutine(OpenDoor());
        }
    }

    private IEnumerator OpenDoor()
    {
        isMoving = true;
        Vector3 startPosition = door.transform.position;
        Vector3 endPosition = startPosition + moveDirection.normalized * moveDistance;
        float elapsedTime = 0f;

        while (elapsedTime < moveDistance / moveSpeed)
        {
            door.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime * moveSpeed) / moveDistance);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.transform.position = endPosition;
        isMoving = false;
        doorOpened = true; // 문이 열렸으므로 더 이상 버튼을 누를 수 없음
    }
}

