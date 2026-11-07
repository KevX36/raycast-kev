using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Interact(IInteractable interactableHit)
    {
        interactableHit.OnInteract();
    }

    public void HighlightInteractable(GameObject interactable)
    {

    }
    public void RemoveHighlightFromInteractable(GameObject interactable)
    {

    }
}
