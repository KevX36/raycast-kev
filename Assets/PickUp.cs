using UnityEngine;

public class PickUp : MonoBehaviour,IInteractable
{
    public void OnInteract()
    {

        FindAnyObjectByType<PlayerController>().scoreUp();
        Destroy(this.gameObject);


    }
}
