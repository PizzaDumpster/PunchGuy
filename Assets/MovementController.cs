using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] string currentState;
    [SerializeField] Vector2 movement;
    [SerializeField] float movementSpeed, jumpForce;
    [SerializeField] bool isPunching, isJumping, isIdle, isWalking, isGrounded;
    [SerializeField] Transform punchPoint, groundCheck;
    [SerializeField] float punchRadius, groundCheckRadius;
    Collider2D[] myHits;
    enum State
    {
        isPunching,
        isWalking,
        isIdle,
        isJumping
    }
    State myState;
    // Start is called before the first frame update
    void Start()
    {
        anim.GetComponent<Animator>();
        isPunching = false;
        isJumping = false;
        isIdle = true;
        isWalking = false;
        myState = State.isIdle;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.CircleCast(groundCheck.position, groundCheckRadius, Vector2.down);
        movement.x = Input.GetAxis("Horizontal");
        if (isJumping || isPunching)
        {
            movement.x = 0;
        }

         rb.velocity = new Vector2(movement.x * movementSpeed, rb.velocity.y);
        


        if (movement.x > 0.1f && !isJumping && !isPunching)
        {
            transform.localScale = new Vector3(1, 1, 1);
            ChangeAnimationState("Walk");
            isWalking = true;
            isIdle = false;
            isJumping = false;
            isPunching = false;
            myState = State.isWalking;

        }
        else if (movement.x < -0.01f && !isJumping && !isPunching)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            ChangeAnimationState("Walk");
            isWalking = true;
            isIdle = false;
            isJumping = false;
            isPunching = false;
            myState = State.isWalking;

        }
        else if (movement == new Vector2(0, 0) && isGrounded && !isJumping && !isPunching)
        {

            ChangeAnimationState("Idle");
            isIdle = true;
            isWalking = false;
            isJumping = false;
            isPunching = false;
            myState = State.isIdle;

        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isPunching || !isJumping)
            {
                isIdle = false;
                isWalking = false;
                isJumping = false;
                isPunching = true;
                StartCoroutine(PunchClock());
                myHits = Physics2D.OverlapCircleAll(punchPoint.position, punchRadius);
                StartCoroutine(HitDelay());
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            isIdle = false;
            isWalking = false;
            isJumping = true;
            isPunching = false;
            if (!isPunching || !isJumping)
                StartCoroutine(MyJump());
            Debug.Log("1");
        }
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        anim.Play(newState);
        currentState = newState;
    }
    IEnumerator PunchClock()
    {
        myState = State.isPunching;
        isPunching = true;
        ChangeAnimationState("Punch");
        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        isPunching = false;
    }
    IEnumerator HitDelay()
    {
        foreach (Collider2D hit in myHits)
        {
            Debug.Log(hit.name);
            yield return null;
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length * 0.5f);
            hit.transform.localPosition += new Vector3(1, 0, 0);
        }
    }

    IEnumerator MyJump()
    {
        myState = State.isJumping;
        isJumping = true;
        ChangeAnimationState("Jump");
        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        yield return null;
        isJumping = false;
        Debug.Log("2");
    }
}
