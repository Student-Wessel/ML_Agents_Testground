using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WanderingDeer : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotSpeed = 100f;

    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
    

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(Wander());
    }

    void Update()
    {
        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }
        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
        }
        if (isWalking == true)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    
    
    IEnumerator Wander()
    {
        while (true)
        {
            int rotTime = Random.Range(1, 3);
            int rotateWait = Random.Range(1, 4);
            int rotateLorR = Random.Range(1, 2);
            int walkWait = Random.Range(1, 5);
            int walkTime = Random.Range(1, 6);

            yield return new WaitForSeconds(walkWait);
            isWalking = true;
            SetAnimationState();
            yield return new WaitForSeconds(walkTime);
            isWalking = false;
            SetAnimationState();
            yield return new WaitForSeconds(rotateWait);
            if (rotateLorR == 1)
            {
                isRotatingRight = true;
                isWalking = true;
                SetAnimationState();
                yield return new WaitForSeconds(rotTime);
                isRotatingRight = false;
                isWalking = false;
                SetAnimationState();
            }
            if (rotateLorR == 2)
            {
                isRotatingLeft = true;
                SetAnimationState();
                yield return new WaitForSeconds(rotTime);
                isRotatingLeft = false;
                SetAnimationState();
            }
        }
    }

    private void SetAnimationState()
    {

        animator.SetBool("IsWalking",isWalking);
        animator.SetBool("IsWandering",!isWalking);
        animator.SetBool("WalkTurnR",isRotatingRight);
        animator.SetBool("WalkTurnL",isRotatingLeft);
    }
}