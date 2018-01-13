using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterInputController))]
public class MinionRootMotionControlScript : MonoBehaviour
{
    private Animator anim;	
    private Rigidbody rbody;
    private CharacterInputController cinput;
  
    public bool isGrounded;

    //Useful if you implement jump in the future...
    public float jumpableGroundNormalMaxAngle = 45f;
    public bool closeToJumpableGround;


    void Awake()
    {

        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        cinput = GetComponent<CharacterInputController>();

        if (cinput == null)
            Debug.Log("CharacterInputController could not be found");

        anim.applyRootMotion = true;
    }


    void Start()
    {

        isGrounded = false;

        //never sleep so that OnCollisionStay() always reports for ground check
        rbody.sleepThreshold = 0f;
    }



    void FixedUpdate()
    {
	
        //onCollisionStay() doesn't always work for checking if the character is grounded from a playability perspective
        //Uneven terrain can cause the player to become technically airborne, but so close the player thinks they're touching ground.
        //Therefore, an additional raycast approach is used to check for close ground
        if (CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.85f, 0f, out closeToJumpableGround))
            isGrounded = true;


        anim.SetFloat("velx", cinput.Turn); 
        anim.SetFloat("vely", cinput.Forward);
        anim.SetBool("isFalling", !isGrounded);


    }



    //This is a physics callback
    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;

    }

    //This is a physics callback
    void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {
                         
            EventManager.TriggerEvent<MinionLandsEvent, Vector3, float>(collision.contacts[0].point, collision.impulse.magnitude);

        }
						
    }



    void OnAnimatorMove()
    {
        if (isGrounded)
        {
         	//use root motion as is if on the ground		
            this.transform.position = anim.rootPosition;

        }
        else
        {
            //Simple trick to keep model from climbing other rigidbodies that aren't the ground
            this.transform.position = new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        }

        //use rotational root motion as is
        this.transform.rotation = anim.rootRotation;

        //clear IsGrounded
        isGrounded = false;
    }

}
