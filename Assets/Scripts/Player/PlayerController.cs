using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    PlayerCombatManager playerCombatManager;
    public Animator animator;
    public SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    //public CharacterScriptableObject characterStats;
    PlayerStats player;
    
    [Header("Player Stats")]
    //public float movementSpeed;

    //dash handling
    /*
    public float dashForce; //force the player is pushed when dashing
    public float dashDuration; //how long the force is applied
    public float dashCooldown; //how often the player can dash

    public bool isDashing = false;
    public bool dashReady = true;
    private Vector2 dashDirection; //direction the player's dash force will be applied
    private float dashTimer = 0f; //timer used to quantify how long the player has been dashing
    private float dashCooldownTimer = 0f; //timer used to check when the player can dash again
    private float lastDashTime = -Mathf.Infinity;
    */

    //the layer we want our sprite to appear on
    public int desiredSortingOrder;
    //the layer we are on
    private int currentSortingOrder;

    [HideInInspector]
    public Vector2 movementDirection;
    [HideInInspector]
    public float mostRecentHorizontalVector;
    [HideInInspector]
    public float mostRecentVerticalVector;
    [HideInInspector]
    public Vector2 directionLastMoved;


    private Vector2 mousePosition;

    public float movementX;
    public float movementY;

    private bool runningLeft;
    
    void Awake()
    {
        playerCombatManager = GetComponent<PlayerCombatManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        directionLastMoved = new Vector2(1, 0f); //sets right as the default   
    }

    // Update is called once per frame
    void Update()
    {     
        PlayerAnimation();

        if (Input.GetMouseButtonDown(0))
        {
            //shooting behavior
        }

        /*
        // Check if the player can dash
        if (Time.time - lastDashTime >= dashCooldown)
        {
            dashReady = true;
            // Update dash cooldown timer
            dashCooldownTimer = 0f;
        }
        else
        {
            // Update dash cooldown timer
            dashCooldownTimer = dashCooldown - (Time.time - lastDashTime);
        }*/
    }

    void FixedUpdate()
    {
        //inputs
        playerMouse();
        PlayerMovement();      
    }

    void playerMouse()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void PlayerMovement()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");

        movementDirection = new Vector2(movementX, movementY).normalized;

        rb.velocity = new Vector2(movementDirection.x * player.currentMovementSpeed, movementDirection.y * player.currentMovementSpeed);

            //saves the last direction we moved so the projectile still has a direction if the player is still
           if(movementDirection.x != 0)
           {
                mostRecentHorizontalVector = movementDirection.x;
                directionLastMoved = new Vector2(mostRecentHorizontalVector, 0f);
           }

           if(movementDirection.y != 0)
           {
                mostRecentVerticalVector = movementDirection.y;
                directionLastMoved = new Vector2(0f, mostRecentVerticalVector);
           }

           if(movementDirection.x != 0 && movementDirection.y != 0)
           {
                directionLastMoved = new Vector2(mostRecentHorizontalVector, mostRecentVerticalVector);
           }
    }

    void PlayerAnimation()
    {
        //horizontal movement animation
        animator.SetFloat("sideSpeed", Mathf.Abs(movementX));

        //handles player facing the correct direction while running
        if (movementX > 0 && !runningLeft)
        {
            Flip();
        }
        if (movementX < 0 && runningLeft)
        {
            Flip();
        }

        //down movement animation
        animator.SetFloat("downSpeed", -movementY);

        //up movement animation
        animator.SetFloat("upSpeed", movementY);

        //gets the current layer our player is on
        currentSortingOrder = playerSpriteRenderer.sortingOrder;

        //if its not what we want, change it
        if (currentSortingOrder != desiredSortingOrder)
        {
            ChangeLayer(desiredSortingOrder);
        }
    }

    /*
    void StartDash(Vector2 direction)
    {
        // Set the dash direction
        dashDirection = direction.normalized;

        // Start the dash timer
        dashTimer = dashDuration;

        // Set the dashing flag to true
        isDashing = true;
        dashReady = false;
    }*/

    //flips the sprite
    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        runningLeft = !runningLeft;
    }

    void ChangeLayer(int desiredSortingOrder)
    {
        playerSpriteRenderer.sortingOrder = desiredSortingOrder;
    }
}
