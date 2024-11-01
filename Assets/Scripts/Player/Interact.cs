using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    void OnInteraction();
    string GetInteractPromptName();
    string GetInteractPromptDescription();
}

//Searching
public class Interact : MonoBehaviour
{
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TextMeshProUGUI itemNamePrompt;
    [SerializeField] private TextMeshProUGUI itemDescriptionPrompt;
    [SerializeField] private LayerMask interactableLayerMask;

    [SerializeField] float interactionDistance = 3;
    [SerializeField] float interactionRate = 0.1f;

    private Camera cam;
    private bool hasSearched = false;

    private ItemSO curItemData;
    private IInteractable curInteract;

    private void Start()
    {
        cam = Camera.main;
        promptPanel.SetActive(false);
        descriptionPanel.SetActive(false);
    }

    private void Update()
    {
        if (!hasSearched)
        {
            Searching();
            Invoke("SearchAgain", interactionRate);
        }
    }

    private void Searching()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, interactionDistance, interactableLayerMask))
        {
            if(hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                hasSearched = true;

                curItemData = hit.collider.GetComponent<ItemSO>();
                curInteract = interactable;

                SetNamePrompt(interactable.GetInteractPromptName());
                SetDescriptionPrompt(interactable.GetInteractPromptDescription());
            }
        }
        else
        {
            curItemData = null;
            curInteract = null;
            ClearPrompt();
        }
    }

    private void ClearPrompt()
    {
        promptPanel.SetActive(false);
        itemNamePrompt.text = string.Empty;
        descriptionPanel.SetActive(false);
        itemDescriptionPrompt.text = string.Empty;
    }

    private void SearchAgain()
    {
        hasSearched = false;
    }

    private void SetNamePrompt(string name)
    {
        promptPanel.SetActive(true);
        itemNamePrompt.text = name;
    }

    private void SetDescriptionPrompt(string description)
    {
        descriptionPanel.SetActive(true);
        itemDescriptionPrompt.text = description;
    }

    //e키를 누를때
    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            if(curItemData != null)
            {
                curInteract.OnInteraction();
            }
        }
    }

}