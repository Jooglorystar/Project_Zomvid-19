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

    [SerializeField] private GameObject toastPrefabPositive; // �佺Ʈ ������ ����
    [SerializeField] private GameObject toastPrefabNegative; // �佺Ʈ ������ ����
    [SerializeField] private float moveUpDistance = 50f; // �о �Ÿ�
    [SerializeField] private float moveDuration = 0.3f;  // �о�� �ִϸ��̼� ���� �ð�
    [SerializeField] private float showDuration = 5.0f;  // �о�� �ִϸ��̼� ���� �ð�
    [SerializeField] private float fadeDuration = 3.0f;  // �о�� �ִϸ��̼� ���� �ð�

    private bool isMoving;
    private Queue<MesssagePair> messageQueue = new();
    private List<GameObject> activeToasts = new List<GameObject>(); // ���� Ȱ��ȭ�� �佺Ʈ �޽��� ����Ʈ

    private void Update()
    {
        if (messageQueue.Count == 0 || isMoving) return;

        MesssagePair queue = messageQueue.Dequeue();
        
        // ���ο� �佺Ʈ �޽��� ����
        GameObject newToast = Instantiate(queue.positive ? toastPrefabPositive : toastPrefabNegative, transform.position - new Vector3(0, moveUpDistance),  Quaternion.identity, transform);
        TextMeshProUGUI toastText = newToast.GetComponentInChildren<TextMeshProUGUI>();
        toastText.text = queue.message;

        // ����Ʈ�� �߰�
        activeToasts.Add(newToast);

        // ������ ��� �佺Ʈ�� ���� �̵��ϴ� �ڷ�ƾ ����
        StartCoroutine(MoveToastsUp());

        // ���̵�ƿ� ����
        StartCoroutine(FadeOutToast(newToast, showDuration));
    }

    public void ShowToast(string message, bool positive)
    {
        messageQueue.Enqueue(new MesssagePair(message, positive));
    }

    // ��� �佺Ʈ�� ���ÿ� ���� �̵���Ű�� �Լ�
    private IEnumerator MoveToastsUp()
    {
        isMoving = true;

        // ������ ��� �佺Ʈ�� ���� �̵�
        float elapsedTime = 0;

        // �̵� �ִϸ��̼�
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

        // ���� ��ġ�� ��Ȯ�ϰ� ����
        for (int i = 0; i < activeToasts.Count; i++)
        {
            RectTransform rectTransform = activeToasts[i].GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(0, moveUpDistance * (activeToasts.Count - i - 1), 0);
        }

        isMoving = false;
    }

    // �޽��� ���̵�ƿ� ȿ�� �ڷ�ƾ
    private IEnumerator FadeOutToast(GameObject toast, float duration)
    {
        CanvasGroup canvasGroup = toast.GetComponent<CanvasGroup>();

        // ������ �ð��� ���� �� ���̵�ƿ� ����
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

        // ������ ���������� ����Ʈ���� �����ϰ� ����
        activeToasts.Remove(toast);
        Destroy(toast);
    }
}
