using System;
using TMPro;
using UnityEngine;

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
    [SerializeField] float interactionRate = 0.5f;

    private Camera cam;
    private bool hasSearched = false;

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

                SetNamePrompt(interactable.GetInteractPromptName());
                SetDescriptionPrompt(interactable.GetInteractPromptDescription());
            }
        }
        else
        {
            promptPanel.SetActive(false);
            descriptionPanel.SetActive(false);
        }
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


}