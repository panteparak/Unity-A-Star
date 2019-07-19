using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chase : MonoBehaviour
{

    public Transform player;

    static Animator anim;

    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsSkill = Animator.StringToHash("isSkill");
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 direction = player.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);
        
        if (Vector3.Distance(player.position, this.transform.position) < 10 && angle < 30)
        {
            
            direction.y = 0;
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            
            anim.SetBool(IsIdle, false);

            if (direction.magnitude > 5)
            {
                this.transform.Translate(0, 0, 0.01f);
                anim.SetBool(IsWalking, true);
                anim.SetBool(IsAttacking, false);
            }
            else
            {
                anim.SetBool(IsAttacking, true);
                anim.SetBool(IsWalking, false);
            }
        }
        else
        {
            anim.SetBool(IsIdle, true);
            anim.SetBool(IsWalking, false);
            anim.SetBool(IsAttacking, false);
        }
    }
}
