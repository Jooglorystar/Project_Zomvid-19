using UnityEngine;

public interface IInteractable
{
    void OnInteraction();
    string GetInteractPromptName();
    string GetInteractPromptDescription();
}

public class Interact : MonoBehaviour
{
    
}