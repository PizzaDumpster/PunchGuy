using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] string currentState;
    [SerializeField] Vector2 movement;
    [SerializeField] float movementSpeed;
    [SerializeField] bool isPunching;
    [SerializeField] Transform punchPoint;
    [SerializeField] float punchRadius;
    Collider2D[] myHits;
    // Start is called before the first frame update
    void Start()
    {
        anim.GetComponent<Animator>();
        isPunching = false;
    }

    // Update is called once per frame
    void Update()
    {


        movement.x = Input.GetAxis("Horizontal");

        if (!isPunching)
        {
            rb.velocity = new Vector2(movement.x * movementSpeed, rb.velocity.y);
        }


        if (movement.x > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
            if (!isPunching)
            {

                ChangeAnimationState("Walk");
            }
        }
        else if (movement.x < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            if (!isPunching)
            {
                ChangeAnimationState("Walk");
            }
        }
        else
        {
            if (!isPunching)
            {
                ChangeAnimationState("Idle");
            }
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isPunching)
            {
                StartCoroutine(PunchClock());
                myHits = Physics2D.OverlapCircleAll(punchPoint.position, punchRadius);
                StartCoroutine(HitDelay());
            }
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
        isPunching = true;
        ChangeAnimationState("Punch");
        yield return null;
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log(anim.GetCurrentAnimatorStateInfo(0).length);
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
}
