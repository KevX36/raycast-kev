using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;

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







    //makes sure no movement happens at start without imput
    private bool doNothingOnStart = true;




    public void Interact(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            RaycastHit hit;
            if (PickUpGO != null)
            {

                PickUpGO.GetComponent<BoxCollider>().enabled = true;
                PickUpRB.constraints = RigidbodyConstraints.None;
                PickUpRB.AddForce(cam.transform.forward);
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
                    hitObject.GetComponent<IInteractable>().OnInteract();
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
    void Update()
    {
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
