using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ToastAlarm : MonoBehaviour
{
    private class MesssagePair
    {
        public string message;
        public bool positive;

        public MesssagePair(string _message, bool _positive)
        {
            message = _message;
            positive = _positive;
        }
    }

    [SerializeField] private GameObject toastPrefabPositive; // 토스트 프리팹 연결
    [SerializeField] private GameObject toastPrefabNegative; // 토스트 프리팹 연결
    [SerializeField] private float moveUpDistance = 50f; // 밀어낼 거리
    [SerializeField] private float moveDuration = 0.3f;  // 밀어내는 애니메이션 지속 시간
    [SerializeField] private float showDuration = 5.0f;  // 밀어내는 애니메이션 지속 시간
    [SerializeField] private float fadeDuration = 3.0f;  // 밀어내는 애니메이션 지속 시간

    private bool isMoving;
    private Queue<MesssagePair> messageQueue = new();
    private List<GameObject> activeToasts = new List<GameObject>(); // 현재 활성화된 토스트 메시지 리스트

    private void Update()
    {
        if (messageQueue.Count == 0 || isMoving) return;

        MesssagePair queue = messageQueue.Dequeue();
        
        // 새로운 토스트 메시지 생성
        GameObject newToast = Instantiate(queue.positive ? toastPrefabPositive : toastPrefabNegative, transform.position - new Vector3(0, moveUpDistance),  Quaternion.identity, transform);
        TextMeshProUGUI toastText = newToast.GetComponentInChildren<TextMeshProUGUI>();
        toastText.text = queue.message;

        // 리스트에 추가
        activeToasts.Add(newToast);

        // 기존의 모든 토스트를 위로 이동하는 코루틴 시작
        StartCoroutine(MoveToastsUp());

        // 페이드아웃 시작
        StartCoroutine(FadeOutToast(newToast, showDuration));
    }

    public void ShowToast(string message, bool positive)
    {
        messageQueue.Enqueue(new MesssagePair(message, positive));
    }

    // 모든 토스트를 동시에 위로 이동시키는 함수
    private IEnumerator MoveToastsUp()
    {
        isMoving = true;

        // 기존의 모든 토스트를 위로 이동
        float elapsedTime = 0;

        // 이동 애니메이션
        while (elapsedTime < moveDuration)
        {
            for (int i = 0; i < activeToasts.Count; i++)
            {
                RectTransform rectTransform = activeToasts[i].GetComponent<RectTransform>();
                rectTransform.localPosition = new Vector3(0, moveUpDistance * (activeToasts.Count - i - 2 + elapsedTime / moveDuration), 0);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 위치를 정확하게 설정
        for (int i = 0; i < activeToasts.Count; i++)
        {
            RectTransform rectTransform = activeToasts[i].GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, moveUpDistance * (activeToasts.Count - i - 1), 0);
        }

        isMoving = false;
    }

    // 메시지 페이드아웃 효과 코루틴
    private IEnumerator FadeOutToast(GameObject toast, float duration)
    {
        CanvasGroup canvasGroup = toast.GetComponent<CanvasGroup>();

        // 지정한 시간이 지난 후 페이드아웃 시작
        yield return new WaitForSeconds(duration);

        float startAlpha = canvasGroup.alpha;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        // 완전히 투명해지면 리스트에서 제거하고 삭제
        activeToasts.Remove(toast);
        Destroy(toast);
    }
}
