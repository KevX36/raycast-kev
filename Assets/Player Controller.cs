using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using TMPro;

public class PlayerController : MonoBehaviour
{
    
    //player parts
    private Rigidbody rb;
    private Collider col;
    [SerializeField] private Camera cam;

    //movement

    [SerializeField] private Vector2 MoveDirection;
    public int speed = 5;
    private int baseSpeed;
    [SerializeField] private Vector3 move;
    //jump
    [SerializeField] Vector3 PlayerFall = new Vector3(0, 0, 0);
    [SerializeField] private int airJumps = 1;
    
    public float JumpHighet = 7;
    public float baseJumpHighet;
    private bool Jumping = false;
    [SerializeField] private bool isGrounded = true;

    
    public GameObject GrabSpot;
    public int TossPower = 5;
    public float InteractRange = 5;
    [SerializeField] private GameObject PickUpGO;
    [SerializeField] private Rigidbody PickUpRB;

    public int score;
    public TextMeshProUGUI scoretext;

    public float thossForce = 500;



    //makes sure no movement happens at start without imput
    private bool doNothingOnStart = true;

    public void scoreUp()
    {
        score++;
        scoretext.text = $"score: {score}";
    }


    public void Interact(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            RaycastHit hit;
            if (PickUpGO != null)
            {

                PickUpGO.GetComponent<BoxCollider>().enabled = true;
                PickUpRB.constraints = RigidbodyConstraints.None;
                PickUpRB.AddForce((cam.transform.forward * TossPower) + (cam.transform.up * (TossPower / 2)));
                PickUpGO = null;
                PickUpRB = null;

            }
            else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, InteractRange))
            {
                GameObject hitObject = hit.transform.gameObject;


                if (hitObject.CompareTag("PickUp"))
                {
                    PickUpGO = hit.transform.gameObject;
                    Debug.Log("Grabed pickup");
                    PickUpRB = PickUpGO.GetComponent<Rigidbody>();
                    PickUpGO.GetComponent<BoxCollider>().enabled = false;
                    PickUpRB.constraints = RigidbodyConstraints.FreezeAll;

                }
                else if (hitObject.CompareTag("interactable"))
                {
                    InteractionManager.instance.Interact(hitObject.GetComponent<IInteractable>());
                }


            }
        }
        
        
    }

    void Awake()
    {

        


        
        baseSpeed = speed;
        col = this.GetComponent<Collider>();
        rb = this.GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        MoveDirection = rb.position;
        baseJumpHighet = JumpHighet;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        InputAction input = context.action;
        Debug.Log("started jump");
        if (input.IsPressed())
        {
            if (isGrounded)
            {
                Jumping = true;
            }
            


        }



    }
    

    
    
    public void OnMove(InputAction.CallbackContext Context)
    {
        Debug.Log("moving");
        MoveDirection = Context.ReadValue<Vector2>();

    }
    public bool cheakIfGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, col.bounds.extents.y + 0.1f);


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("collectable"))
        {
            Destroy(other.gameObject);
        }
    }

    public GameObject currentInteractable;
    
    void Update()
    {
        RaycastHit interactableCheck;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out interactableCheck, InteractRange) && PickUpGO == null)
        {
            GameObject hitObject = interactableCheck.transform.gameObject;

            if (currentInteractable != null && hitObject != currentInteractable)
            {
                InteractionManager.instance.RemoveHighlightFromInteractable();
            }
            else if (hitObject.CompareTag("PickUp") || hitObject.CompareTag("interactable"))
            {
                currentInteractable = hitObject;
                InteractionManager.instance.HighlightInteractable(currentInteractable);
            }
            


        }
        

            Debug.DrawLine(cam.transform.position, (cam.transform.forward * InteractRange) + cam.transform.position, Color.red);
        isGrounded = cheakIfGrounded();
        
        if (PlayerFall.y > 0 && !Jumping)
        {

            PlayerFall.y += Physics.gravity.y * Time.deltaTime;

        }
        if (Jumping)
        {

            PlayerFall.y = JumpHighet;
            if (!isGrounded)
            {
                airJumps--;
            }

            Debug.Log("Jumped");
            Jumping = false;
        }


        if (PickUpGO != null)
        {

            PickUpGO.transform.position = GrabSpot.transform.position;
            PickUpGO.transform.rotation = GrabSpot.transform.rotation;
        }

        move = new Vector3(MoveDirection.x, PlayerFall.y, MoveDirection.y);
        move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * move;
        if (doNothingOnStart)
        {
            Debug.Log("stopped movement");
            MoveDirection.y = 0;
            move.z = 0;
            MoveDirection.x = 0;
            doNothingOnStart = false;
        }
        rb.MovePosition(rb.position + move * speed * Time.deltaTime);




    }

}
