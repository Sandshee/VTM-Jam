using System.Collections;
using UnityEngine.Events;
using UnityEngine;

//[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    public CameraAnims camAn;

    [Header("Movement")]
    public float maxSpeed = 5f;
    private float speed = 0f;
    public float accelleration = 0.1f;
    public float airMod = 0.5f;
    private float slopeLimit;
    private float stepOffset;
    public float landingMod = 0.25f;
    private Vector2 playerInput;

    private Vector3 horizontalVel;
    public float drag;

    private Vector3 previousPosition;

    private bool frozen;

    [Header("Grounded Check")]

    public Transform groundCheck;
    public Transform headCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded = false;
    public float zeroGravity = -2f;

    [Header("Jumping & Landing")]

    private bool canJump = true;
    private bool canBump = false;
    public float landingLag = 1f;
    private int coroutineCount = 0;
    public float jumpHeight;
    private float jumpForce;
    private Vector3 startingJump;
    public float maxLandingTime;
    private int jumpCoroutineCount = 0;


    [Header("Crouching")]

    public float crouchMod;
    private bool crouching = false;
    private bool queueUncrouch = false;
    public float crouchingHeight;
    private float standingHeight;
    public Transform crouchingHeadCheck;

    [Header("Physics")]

    private Vector3 velocity;
    private float gravity;
    public float mass;
    public float interactDistance = 0.5f;
    public HandsUI hands;
    //private PhysicsProp heldItem;

    private Ladder ladder;
    private float ladderPCT;
    private bool isClimbing = false;
    public float climbSpeed;

    private bool interacting;
    private bool canStopInteracting;
    private bool canInteract;
    private Interactible interactingWith;

    [Header("Slopes")]
    public float slipAccel;
    public bool sliding;

    [Header("Inventory")]
    public InventoryUI inventory;
    private bool canUseInventory;

    // Start is called before the first frame update
    void Awake()
    {
        previousPosition = transform.position;
        cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        startingJump = transform.position;
        gravity = Physics.gravity.y;
        jumpForce = Mathf.Sqrt(jumpHeight * -2 * gravity * mass);
        slopeLimit = cc.slopeLimit;
        stepOffset = cc.stepOffset;
        standingHeight = cc.height;
    }

    private void FixedUpdate()
    {
        Grounded();

    }

    private Vector2 GetPlayerMovement()
    {
        return playerInput;
    }

    private void UpdatePlayerMovement()
    {
        if (!GetFrozen())
        {
            playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        } else
        {
            playerInput = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool cancel = Input.GetAxisRaw("Cancel") > 0;
        //Inventory
        if(Input.GetAxisRaw("Inventory") > 0  || cancel)
        {
            if (canUseInventory || cancel)
            {
                if (inventory.GetDisplaying())
                {
                    inventory.Hide();
                    UnFreeze();
                }
                else if(!cancel)
                {
                    inventory.Display();
                    Freeze();
                }
                canUseInventory = false;
            }

        } else
        {
            canUseInventory = true;
        }

        UpdatePlayerMovement();
        CrouchCheck();

        if (isClimbing)
        {
            Climbing();
        }

        CheckInteract();

        Slide();

        if (!sliding)
        {
            MovePlayer(GetPlayerMovement());
        }

        if (Input.GetAxisRaw("Jump") > 0 && isGrounded)
        {
            Jump();
        }

        if (!isClimbing)
        {
            //The player falls down, affected by Gravity.
            velocity.y += gravity * mass * Time.deltaTime;
            cc.Move(velocity * Time.deltaTime);
        }

        if(velocity.y >= 0)
        {
            startingJump = transform.position;
        }
    }

    /* Grounded Check methods
     * For the grounded methods, and jumping.
     * 
     */
    private void Grounded()
    {
        //Was I grounded last frame -> Used to detect if I just landed.
        bool wasGrounded = isGrounded;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y <= 0)
        {
            velocity.y = zeroGravity;
        }

        //Just hit my head on the ceiling, owie, prevents the stuck head functionality.
        if(crouching)
        {
            if (!isGrounded && canBump && Physics.CheckSphere(crouchingHeadCheck.position, groundDistance, groundMask))
            {
                velocity.y = -0.1f;
                canBump = false;
            }
        } else if (!isGrounded && canBump && Physics.CheckSphere(headCheck.position, groundDistance, groundMask))
        {
            velocity.y = -0.1f;
            Debug.Log("Ouch");
            canBump = false;
        }

        if(!wasGrounded && isGrounded)
        {
            //I've just landed, implement Landing Lag.
            camAn.LandingShake(Vector3.Distance(transform.position, startingJump));
            StartCoroutine(WaitForNextJump(Vector3.Distance(transform.position, startingJump)));
            cc.slopeLimit = slopeLimit;
            cc.stepOffset = stepOffset;
        }

        if(wasGrounded && !isGrounded)
        {
            startingJump = transform.position;
        }

    }

    public bool GetFrozen()
    {
        return interacting || frozen;
    }

    public void Freeze()
    {
        frozen = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void UnFreeze()
    {
        frozen = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public bool GetInteracting()
    {
        return interacting;
    }

    void Jump()
    {
        //If I'm unable to jump, why bother?
        if (!canJump || (crouching && Physics.CheckSphere(crouchingHeadCheck.position, groundDistance, groundMask)))
        {
            return;
        }

        canBump = true;
        //Removes any additional vertical component to the velocity.
        velocity = Vector3.up * jumpForce;
        cc.slopeLimit = 0;
        cc.stepOffset = 0;

        canJump = false;
        //Debug.Break();
    }

    IEnumerator WaitForNextJump(float modifier)
    {
        modifier = Mathf.Clamp(modifier * landingLag, 0, maxLandingTime * landingLag)/landingLag;
        coroutineCount++;
        yield return new WaitForSeconds(landingLag * modifier);
        coroutineCount--;

        if(coroutineCount == 0)
        {
            canJump = isGrounded;
        }

        if(coroutineCount < 0)
        {
            Debug.LogError("Negative number of coroutines finished");
            Debug.Break();
        }
    }

    /* Movement methods
     *
     * for moving the player character, and converting vectors.
     */

    void MovePlayer(Vector2 inputs)
    {
        if (!isClimbing)
        {
            previousPosition = transform.position;

            //If the player is on the ground, they can stop moving without input.
            if (isGrounded && inputs == Vector2.zero)
            {
                Drag();
            }

            //If the player is in the air, their accelleration is modified;

            Vector3 relativeInputs = GetRelativeInputs(inputs);

            if (!isGrounded)
            {
                horizontalVel += relativeInputs * accelleration * airMod;
            }
            else
            {
                horizontalVel += relativeInputs * accelleration;
            }

            //Deal with the different max speeds, depending on the player's current state.
            float currentMaxSpeed = maxSpeed;

            if (crouching)
            {
                currentMaxSpeed *= crouchMod;
            }

            if (isGrounded && !canJump)
            {
                currentMaxSpeed *= landingMod;
            }

            //Clamp the velocity to a maximum value.
            if (horizontalVel.sqrMagnitude > currentMaxSpeed * currentMaxSpeed)
            {
                horizontalVel = horizontalVel.normalized * currentMaxSpeed;
            }

            cc.Move(horizontalVel * Time.deltaTime);
        } else
        {
            horizontalVel = Vector3.zero;
        }
    }

    void Drag()
    {
        if (!sliding)
        {
            horizontalVel = horizontalVel / drag;
        }
    }

    private Vector3 GetHorizontalComponents(Vector2 inVector)
    {
        return new Vector3(inVector.x, 0f, inVector.y);
    }

    private Vector3 GetHorizontalComponents(Vector3 inVector)
    {
        return new Vector3(inVector.x, 0f, inVector.z);
    }

    private Vector3 GetRelativeInputs(Vector2 inVector)
    {
        return transform.right * inVector.x + transform.forward * inVector.y;
    }

    public float getSpeed()
    {
        if (isGrounded)
        {
            float t = GetPlayerMovement().SqrMagnitude();

            t = Mathf.Clamp(t, 0, 1);

            if (crouching)
            {
                t *= 0.8f;
            }

            return t;
        } else
        {
            //Can't be walking in the air?
            return 0;
        }
    }

    /* The crouching methods
     * 
     */

    void CrouchCheck()
    {
        if (Input.GetAxisRaw("Crouch") > 0)
        {
            crouching = true;
            queueUncrouch = false;
            cc.height = crouchingHeight;
            cc.center = new Vector3(0, crouchingHeight / 2f, 0);
            camAn.StartCrouch();

            //transform.localScale = crouchingScale;
            //transform.position = Vector3.Scale(prevPos, crouchingScale);
            //Debug.Break();
            Debug.Log("My knees!");
        } else if (Input.GetAxisRaw("Crouch") == 0)
        {
            //I want to get up.
            queueUncrouch = true;
        }

        //See if there's space above my head first.
        if (queueUncrouch && !Physics.CheckSphere(headCheck.position, groundDistance, groundMask) && !Physics.CheckSphere(crouchingHeadCheck.position, groundDistance, groundMask))
        {
            crouching = false;
            queueUncrouch = false;
            camAn.EndCrouch();
            cc.height = standingHeight;
            cc.center = new Vector3(0, standingHeight / 2, 0);

        }
    }

    public void Uninteract()
    {
        interactingWith.UnInteract();
        interactingWith = null;
        canInteract = false;
        interacting = false;
        canStopInteracting = false;
        UnFreeze();
    }

    /* The interactables, buttons etc.
     * 
     * 
     */

    void CheckInteract()
    {
        /*
        if (heldItem)
        {
            heldItem.PutDown();
            heldItem = null;
        }
        */

        bool interacted = Input.GetAxisRaw("Interact") > 0;

        if (!interacted)
        {
            canStopInteracting = true;
            canInteract = true;
        }

        RaycastHit objectHit;

        bool hit = Physics.Raycast(camAn.gameObject.transform.position, camAn.gameObject.transform.forward, out objectHit, interactDistance);

        if(interacting && canStopInteracting && (interacted || Input.GetAxisRaw("Cancel") > 0))
        {
            Uninteract();
        }

        //Update the UI to tell the player what they're looking at.
        if (hit)
        {
            Interactible inter = null;
            switch (objectHit.collider.tag)
            {
                case "Interact":
                    hands.SetTouch();
                    inter = objectHit.collider.GetComponent<Interactible>();
                    break;
                case "Look":
                    hands.SetLook();
                    inter = objectHit.collider.GetComponent<Interactible>();
                    break;
                case "Pickup":
                    hands.SetGrab();
                    if (interacted && canInteract)
                    {
                        objectHit.collider.GetComponent<Interactible>().Interact();
                        canInteract = false;
                        Debug.Log("I pickeded it up!");
                    }
                    //inter = objectHit.collider.GetComponent<Interactible>();
                    break;
                default:
                    hands.ResetIcon();
                    break;
            }

            if (interacted && inter && canInteract)
            {
                Debug.Log("I'm interacting!");
                inter.Interact();
                if (inter.DoesFreezePlayer())
                {
                    interacting = true;
                    interactingWith = inter;
                    canStopInteracting = false;
                    canInteract = false;
                    Freeze();
                } else
                {
                    Debug.Log("No need to freeze");
                }
            }

        } else
        {
            hands.ResetIcon();
        }


        if (Input.GetAxisRaw("Interact") > 0)
        {
            //I need to establish what I'm looking at.
            if (isClimbing)
            {
                StopClimbing();
            }
        }
    }

    /* The sliding code
     * 
     * 
     */

    void Slide()
    {
        RaycastHit rayHit;

        Physics.Raycast(groundCheck.position, Vector3.down, out rayHit, 1f, groundMask);

        Vector3 normal = rayHit.normal;
        if (normal.y <= 0.5 && normal.y > 0)
        {
            sliding = true;
            //cc.Move(GetHorizontalComponents(normal));
            //MovePlayer(GetHorizontalComponents(normal));
            horizontalVel += GetHorizontalComponents(normal * slipAccel);
            cc.Move(horizontalVel * Time.deltaTime);
        } else
        {
            sliding = false;
        }
    }

    /* Climbing code is mostly left up to the ladder to calculate where I should be, I just trust the ladder.
     * 
     * 
     */

    void Climbing()
    {
        float up = GetPlayerMovement().y;

        Vector3 desiredPosition = ladder.Climb(up, climbSpeed * Time.deltaTime);
        if (desiredPosition == Vector3.zero)
        {
            StopClimbing();
        }
        else
        {
            transform.position = desiredPosition;
        }
    }

    void StopClimbing()
    {
        ladder = null;
        isClimbing = false;
    }
}
