using System;
using UnityEngine;
using UnityEngine.Serialization;


public class Player : MonoBehaviour
{
    #region Configuration Parameters
    
    [FormerlySerializedAs("playerSpeed")]
    [SerializeField]
    private float speedModifier = 2;

    [SerializeField]
    private Animator animator;
    
    #endregion
    
    #region State
    
    #endregion

    #region Cached Component Reference

    
    private static readonly int AnimatorHorizontal = Animator.StringToHash("Horizontal");

    private static readonly int AnimatorVertical = Animator.StringToHash("Vertical");
    
    private static readonly int AnimatorMagnitude = Animator.StringToHash("Magnitude");
    
    private static readonly int LastUp = Animator.StringToHash("LastUp");
    
    private static readonly int LastDown = Animator.StringToHash("LastDown");
    
    private static readonly int LastRight = Animator.StringToHash("LastRight");
    
    private static readonly int LastLeft = Animator.StringToHash("LastLeft");

    public char[,] mazeSolution;
    
    private Vector2 _moveVelocity;

    private Rigidbody2D body;
    private Vector3 currentTarget;

    public bool notAutosolving;
    private float autoHorizontalMovement;
    private float autoVerticalMovement;

    #endregion

    #region Unity Methods
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        notAutosolving = true;
        autoHorizontalMovement = 0;
        autoVerticalMovement = 0;
    }

    // Update is called once per frame
    void Update()
    {
        DetectKeypress();
        if (notAutosolving)
        {
            DetectMovement();
        }
        else
        {
            AutoMove();
        }

        AnimateCharacter();

    }

    private void AnimateCharacter()
    {
        var movement = new Vector3(autoHorizontalMovement, autoVerticalMovement, 0.0f);
        animator.SetFloat(AnimatorHorizontal, autoHorizontalMovement);
        animator.SetFloat(AnimatorVertical, autoVerticalMovement);
        animator.SetFloat(AnimatorMagnitude, movement.magnitude);
    }


    private void AutoMove()
    {
        float step = speedModifier * Time.deltaTime;

        if (transform.position == currentTarget)
        {
            var row = (int)transform.position.x;
            var column = (int)transform.position.y;
            var desired_move = mazeSolution[row, column];
            if (desired_move == '>')
            {
                var x_position = transform.position.x;
                var y_position = transform.position.y + 1;
                currentTarget = new Vector3(x_position,y_position,0f);
                autoHorizontalMovement = 0;
                autoVerticalMovement = 1;
            }
            if (desired_move == '^')
            {
                var x_position = transform.position.x-1;
                var y_position = transform.position.y;
                currentTarget = new Vector3(x_position,y_position,0f);
                autoHorizontalMovement = -1;
                autoVerticalMovement = 0;
            }
            if (desired_move == '<')
            {
                var x_position = transform.position.x;
                var y_position = transform.position.y - 1;
                currentTarget = new Vector3(x_position,y_position,0f);
                autoHorizontalMovement = 0;
                autoVerticalMovement = -1;
            }
            if (desired_move == 'v')
            {
                var x_position = transform.position.x+1;
                var y_position = transform.position.y;
                currentTarget = new Vector3(x_position,y_position,0f);
                autoHorizontalMovement = 1;
                autoVerticalMovement = 0;
            }

            
        }
        

        

        
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, step);
    }

    #endregion

    #region Private Methods

    private void DetectMovement()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 | verticalInput != 0)
        {
            autoHorizontalMovement = horizontalInput;
            autoVerticalMovement = verticalInput;
        }
        
        var movement = new Vector3(horizontalInput, verticalInput, 0.0f);
        
        var movementvector = new Vector2(horizontalInput,verticalInput);
        movementvector.Normalize();
        body.velocity = movementvector * speedModifier;

    }
    
    private void DetectKeypress()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            
            notAutosolving = !notAutosolving;
            
            if (!notAutosolving)
            {
                CenterCharacter();
            }
        }


    }

    private void CenterCharacter()
    {
        var x_position = (int)transform.position.x+0.5f;
        var y_position = (int)transform.position.y+0.5f;
        currentTarget = new Vector3(x_position,y_position);
    }
    
    #endregion
}
