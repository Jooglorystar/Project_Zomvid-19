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

public class Interact : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NamePrompt;
    [SerializeField] private TextMeshProUGUI DescriptionPrompt;
    [SerializeField] private LayerMask InteractableLayerMask;
    [SerializeField] private float inspectDistance = 5;
    [SerializeField] private float checkRate = 0.05f;
    private float lastCheckTime;

    private Camera cam;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;


    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            Search();
        }
    }

    private void Search()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, inspectDistance, InteractableLayerMask))
        {
            if(hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                curInteractable = interactable;
                curInteractGameObject = hit.collider.gameObject;
                SetNamePrompt(interactable.GetInteractPromptName());
                SetDescriptionPrompt(interactable.GetInteractPromptDescription());
            }
        }
        else
        {
            curInteractGameObject = null;
            curInteractable = null;
            ClearPrompt();
        }
    }

    private void SetNamePrompt(string name)
    {
        NamePrompt.text = name;
    }

    private void SetDescriptionPrompt(string description)
    {
        DescriptionPrompt.text = description;
    }

    private void ClearPrompt()
    {
        NamePrompt.text = string.Empty;
        DescriptionPrompt.text = string.Empty;
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curInteractable?.OnInteraction();

            curInteractGameObject = null;
            curInteractable = null;
            ClearPrompt();
        }
    }
}