using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private ItemSO itemData;

    [SerializeField] private Image slotIcon;
    [SerializeField] private TextMeshProUGUI itemCountText;

    private Outline outline;

    public int index;
    public int itemCount;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void Start()
    {
        // юс╫ц
        if (itemData != null) Set();
        else Clear();
    }
    public void Set()
    {
        slotIcon.gameObject.SetActive(true);
        slotIcon.sprite = itemData.icon;
        itemCountText.text = itemCount > 1? itemCount.ToString() : string.Empty;
    }

    public void Clear()
    {
        itemData = null;
        slotIcon.gameObject.SetActive(false);
        itemCountText.text = string.Empty;
    }
}
