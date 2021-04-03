using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Andrew Greer
 *  - this script is responsible for handling all the animations of the revamped character model 
 *  
 *  IMPORTANT NOTE: When using the Player V2 asset, remember to rename the prefab to "Player" so scripts like movement.cs work correctly
 */
public class dummyAnimations : MonoBehaviour
{
    public RuntimeAnimatorController sprint;
    public RuntimeAnimatorController run;
    public RuntimeAnimatorController walk;
    public RuntimeAnimatorController jump;
    public RuntimeAnimatorController idle;

    private Animator dummy;
    private Rigidbody2D player;
    private bool facingRight = true;

    private void Start()
    {
        dummy = GameObject.FindGameObjectWithTag("dummy_mesh").GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    

    /* Andrew Greer
     *  - gets the root transform and it's rotation struct
     *  - flips the animation depending on whether the player is running to the right or the left
     *  - uses the character's speed to determine which animation to play
     *      - uses a coroutine to handle jumping since it needs to interrupt the other animations and play for a set period of time
     */
    private void LateUpdate()
    {
        Transform rootBone = transform.root;
        Vector3 rootBoneRotation = rootBone.rotation.eulerAngles;


        //Andrew Greer: flips the character transform depending on direction of travel
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!facingRight)
            {
                rootBone.Rotate(new Vector3(0, rootBoneRotation.y, 0), Space.World);
                facingRight = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (facingRight)
            {
                rootBone.Rotate(new Vector3(0, rootBoneRotation.y - 180, 0), Space.World);
                facingRight = false;
            }
        }


        // Andrew Greer: large block for setting aniamtion based on speed
        if (Mathf.Abs(player.velocity.x) < 0.5f && dummy.runtimeAnimatorController != jump)
        {
            dummy.runtimeAnimatorController = idle;
        }
        else if (Mathf.Abs(player.velocity.x) < 6f && dummy.runtimeAnimatorController != jump)
        {
            dummy.runtimeAnimatorController = walk;
        }
        else if (Mathf.Abs(player.velocity.x) < 10f && dummy.runtimeAnimatorController != jump)
        {
            dummy.runtimeAnimatorController = run;
        }
        else if (Mathf.Abs(player.velocity.x) > 10f && dummy.runtimeAnimatorController != jump)
        {
            dummy.runtimeAnimatorController = sprint;
        }
    }

    //Andrew Greer: Exposed method for jumping animation that can be called from movement.cs
    public void PlayJumpingAnimation()
    {
        StartCoroutine("JumpAnimation");
    }


    //Andrew Greer: couroutine for playing jump animation before transitioning back to running
    IEnumerator JumpAnimation()
    {
        dummy.runtimeAnimatorController = jump;
        yield return new WaitForSeconds(0.75f);
        dummy.runtimeAnimatorController = run;
    }
}
