using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Cabnet : MonoBehaviour, IInteractable
{
    public Animator animator;
    
    public void OnInteract()
    {
        animator.SetTrigger("playAnimation");
        
        
    }
}
