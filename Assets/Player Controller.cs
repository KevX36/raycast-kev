using UnityEngine;
using UnityEngine.InputSystem;

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



    public float GrabRange = 5;




    



    //makes sure no movement happens at start without imput
    private bool doNothingOnStart = true;




    

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
    public GameObject basicBox;

    
    
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
        Debug.DrawLine(cam.transform.position, (cam.transform.forward * GrabRange) + cam.transform.position, Color.red);
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
