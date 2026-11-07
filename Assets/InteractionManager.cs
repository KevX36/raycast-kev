using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance;
    public GameObject highlightFX;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            highlightFX.SetActive(false);
        }
        else
        {
            if (highlightFX != null)
            {
                Destroy(highlightFX);
            }
            Destroy(this.gameObject);
        }
        
    }

    public void Interact(IInteractable interactableHit)
    {
        interactableHit.OnInteract();
    }

    public void HighlightInteractable(GameObject interactable)
    {
        highlightFX.SetActive(true);
        highlightFX.transform.position = interactable.transform.position;
    }
    public void RemoveHighlightFromInteractable()
    {
        highlightFX.SetActive(false);
    }
}
