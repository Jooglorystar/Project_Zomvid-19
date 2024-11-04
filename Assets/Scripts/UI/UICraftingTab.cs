using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICraftingTab : MonoBehaviour
{
    public CraftingSlot[] slots;

    public Transform Page1;
    public Transform Page2;
    public UIInventoryTab inventoryTab;

    [Header("Select Item")]
    public TextMeshProUGUI craftingItemNameText;
    public TextMeshProUGUI craftingItemDescText;
    public TextMeshProUGUI materialNameText;
    public TextMeshProUGUI materialCountText;

    public GameObject craftButton;

    private CraftingSlot selectedItem;
    private int selectedItemIndex;

    [Header("Craft Quantity")]
    public TextMeshProUGUI craftQuantityText;
    private int craftQuantity;

    public GameObject QuantityUpButton;
    public GameObject QuantityDownButton;

    [Header("Not Enough Item")]
    public TextMeshProUGUI notEnoughItemMessageText;
    private Coroutine notEnoughItemMessageCoroutine;


    // 소지한 재료의 갯수 파악용 배열
    private int[] hasMaterialCounts;

    private void Start()
    {
        slots = new CraftingSlot[Page1.childCount + Page2.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Page1.childCount)
            {
                slots[i] = Page1.GetChild(i).GetComponent<CraftingSlot>();
            }
            else if (i >= Page1.childCount)
            {
                slots[i] = Page2.GetChild(i - Page1.childCount).GetComponent<CraftingSlot>();
            }

            slots[i].index = i;
            slots[i].craftingTab = this;
        }
        ClearCraftingItemWindow();

        CharacterManager.Instance.player.uICraftingTab = this;
        CharacterManager.Instance.player.uiInventoryTab = inventoryTab;
    }

    // 창 초기화
    private void ClearCraftingItemWindow()
    {
        craftingItemNameText.text = string.Empty;
        craftingItemDescText.text = string.Empty;
        materialNameText.text = string.Empty;
        materialCountText.text = string.Empty;
        craftQuantityText.text = string.Empty;

        notEnoughItemMessageText.gameObject.SetActive(false);

        craftButton.SetActive(false);
        QuantityUpButton.SetActive(false);
        QuantityDownButton.SetActive(false);
    }

    // 선택 아이템 표시
    public void SelectItem(int index)
    {
        if (slots[index].itemData == null)
        {
            ClearCraftingItemWindow();
            return;
        }

        selectedItem = slots[index];
        selectedItemIndex = index;

        // 제작할 아이템 이름과 설명
        craftingItemNameText.text = selectedItem.itemData.itemName;
        craftingItemDescText.text = selectedItem.itemData.itemDesc;

        craftButton.SetActive(true);

        // 제작 수 조정 버튼
        craftQuantity = 1;
        craftQuantityText.text = craftQuantity.ToString();
        QuantityUpButton.SetActive(true);
        QuantityDownButton.SetActive(true);

        // 제작에 필요한 재료 체크
        CheckMaterials();
    }

    // 제작 버튼
    public void OnCraftButton()
    {
        if (HasAllMaterial())
        {
            Debug.Log("제작 완료");
            CraftItem();
        }
        else
        {
            if (notEnoughItemMessageCoroutine == null)
            {
                notEnoughItemMessageCoroutine = StartCoroutine(DisplayNotEnoughItemMessage());
            }
        }
    }

    // 제작 갯수 체크 메서드
    private void CheckMaterials()
    {
        materialNameText.text = string.Empty;
        materialCountText.text = string.Empty;

        hasMaterialCounts = new int[selectedItem.itemData.itemMaterials.Count];

        // 재료 표시
        for (int i = 0; i < selectedItem.itemData.itemMaterials.Count; i++)
        {
            // 이름 표기
            materialNameText.text += selectedItem.itemData.itemMaterials[i].item.itemName.ToString() + "\n";

            // needCount = 필요한 재료 수, hasCount = 보유한 재료 수
            int needCount = selectedItem.itemData.itemMaterials[i].itemCount * craftQuantity;
            int hasCount = 0;

            // 레시피와 같은 재료 찾기
            for (int j = 0; j < inventoryTab.slots.Length; j++)
            {
                if (selectedItem.itemData.itemMaterials[i].item == inventoryTab.slots[j].itemData)
                {
                    hasCount = inventoryTab.slots[j].itemCount;
                    break;
                }
            }
            // 제작시 이용할 배열에 보유숫자 저장
            hasMaterialCounts[i] = hasCount;

            // 보유/필요 형태의 문자열화
            string hasCountAndNeedCount = $"{hasCount} / {needCount}";

            // 미달된 아이템은 붉은 표시
            if (hasCount < needCount)
            {
                hasCountAndNeedCount = $"<color=#FF0000>{hasCountAndNeedCount}</color>";
            }
            // 충족된 아이템은 녹색 표시
            else
            {
                hasCountAndNeedCount = $"<color=#00FF00>{hasCountAndNeedCount}</color>";
            }

            // 재료 수 표기
            materialCountText.text += hasCountAndNeedCount + "\n";
        }
    }

    // 선택된 아이템 재료 수 체크 메서드
    private bool HasAllMaterial()
    {
        for (int i = 0; i < hasMaterialCounts.Length; i++)
        {
            // hasMaterialCounts에 저장한 값과 itemMaterials[i].itemCount값 비교
            if (hasMaterialCounts[i] < selectedItem.itemData.itemMaterials[i].itemCount * craftQuantity)
            {
                // 하나라도 충족시키지 못하면 false 반환
                return false;
            }
            else
            {
                // 충족하면 다음 아이템 체크
                continue;
            }
        }
        // 모두 충족시 true 반환
        return true;
    }

    // 아이템 제작 메서드
    private void CraftItem()
    {
        // TODO 아이템 제작하는 메서드
        // 재료 아이템을 필요한 양 만큼 깎고
        for (int j = 0; j < inventoryTab.slots.Length; j++)
        {
            for (int i = 0; i < selectedItem.itemData.itemMaterials.Count; i++)
            {
                if (inventoryTab.slots[j].itemData == selectedItem.itemData.itemMaterials[i].item)
                {
                    inventoryTab.slots[j].itemCount--;
                }
            }
        }
        // 만든 만큼 인벤토리 추가하기
        CharacterManager.Instance.player.itemData = selectedItem.itemData;
        for(int i = 0; i < craftQuantity; i++)
        {
            inventoryTab.AddItem();
        }
        CharacterManager.Instance.player.itemData = null;
        SelectItem(selectedItemIndex);
    }

    // 재료 부족시 경고 메세지 띄우는 메서드
    private IEnumerator DisplayNotEnoughItemMessage()
    {
        notEnoughItemMessageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        notEnoughItemMessageText.gameObject.SetActive(false);

        notEnoughItemMessageCoroutine = null;
    }

    public void OnQuantityUpButton()
    {
        // craft범위는 1 이상 9 이하
        craftQuantity = Mathf.Clamp(craftQuantity + 1, 1, 9);
        craftQuantityText.text = craftQuantity.ToString();
        CheckMaterials();
    }

    public void OnQuantityDownButton()
    {
        craftQuantity = Mathf.Clamp(craftQuantity - 1, 1, 9);
        craftQuantityText.text = craftQuantity.ToString();
        CheckMaterials();
    }
}