using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [Header("Idle Stuff")]
    [SerializeField] private float minIdleTime = 0.1f;
    [SerializeField] private float maxIdleTime = 1.5f;
    
    [Header("Move Stuff")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minMoveTime = 0.5f;
    [SerializeField] private float maxMoveTime = 1.5f;

    [Header("Ground Check Stuff")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private RaycastHit2D hit;

    private float idleTime;
    private float moveTime;
    private int facingDirection; // 1 for right, -1 for left

    private int randomFlip;

    private Rigidbody2D rb2D;
    private Animator anim;
    private CurrentAction currentAction;

    private bool idleIsSet;
    private bool moveIsSet;

    private float tempYPosForGroundCheck;

    void Awake()
    {
        facingDirection = 1;

        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        idleIsSet = false;
        moveIsSet = false;
    }
    
    void Start()
    {
        SetIdle();
    }


    void Update()
    {
        if(!GameManager.Instance.isGameOver)
        {
            switch (currentAction)
            {
                case CurrentAction.IDLE:
                    if(!idleIsSet)
                    {
                        SetIdle();
                    }
                    CheckIdle();
                    break;
                case CurrentAction.MOVE:
                    if(!moveIsSet)
                    {
                        SetMove();
                    }
                    CheckMove();
                    break;
                default:
                    Debug.LogError("Bilinmeyen bir eylem durumu.");
                    break;
            }
        }
        
    }

    private void GroundCheck()
    {
        hit = Physics2D.Raycast(groundCheck.position, Vector2.down,  20f, groundLayer);

        if(!hit)
        {
            Flip();
            SetMove();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3 (transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
        facingDirection *= -1;
    }

    private void CheckIdle()
    {
        if(idleTime > 0f)
        {
            idleTime -= Time.deltaTime;
        }
        else
        {
            SetRandomFlip();
            SetMove();
        }
    }

    private void CheckMove()
    {
        if(moveTime > 0f)
        {
            moveTime -= Time.deltaTime;
            GroundCheck();
        }
        else
        {
            SetIdle();
        }
    }

    private void SetIdle()
    {
        ResetFlags();

        idleTime = Random.Range(minIdleTime, maxIdleTime);
        rb2D.velocity = new Vector2(0f, rb2D.velocity.y);
        anim.Play("Idle");
        currentAction = CurrentAction.IDLE;

        idleIsSet = true;
    }

    private void SetMove()
    {
        ResetFlags();

        moveTime = Random.Range(minMoveTime, maxMoveTime);
        rb2D.velocity = new Vector2(moveSpeed * facingDirection, 0f);
        anim.Play("Move");
        currentAction = CurrentAction.MOVE;

        moveIsSet = true;
    }

    public void MultiplySpeed(bool isIncreasing, float multiplyAmount)
    {
        if(isIncreasing)
        {
            moveSpeed *= multiplyAmount;
            anim.speed *= multiplyAmount;
            
        }
        else
        {
            moveSpeed /= multiplyAmount;
            anim.speed /= multiplyAmount;
        }

        SetMove();
    }

    public void MultiplySize(bool isIncreasing, float multiplyAmount)
    {
        tempYPosForGroundCheck = groundCheck.position.y;
        if(isIncreasing)
        {
            transform.localScale *= multiplyAmount;
            transform.position = new Vector3(transform.position.x, transform.position.y + (tempYPosForGroundCheck - groundCheck.position.y), transform.position.z);
        }
        else
        {
            transform.localScale /= multiplyAmount;
            transform.position = new Vector3(transform.position.x, transform.position.y - (groundCheck.position.y - tempYPosForGroundCheck), transform.position.z);
        }
    }

    private void SetRandomFlip()
    {
        randomFlip = Random.Range(0, 2);

        switch (randomFlip)
        {
            case 0:
                Flip();
                break;
            case 1:
                // Do nothing
                break;
        }
    }

    private void ResetFlags()
    {
        idleIsSet = false;
        moveIsSet = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();

        if(collectable != null)
        {
            collectable.Collect(true);
        }
    }

    enum CurrentAction
    {
        IDLE,
        MOVE
    }
}
