using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//require some things the bot control needs
[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
public class YBotSimpleControlScript : MonoBehaviour
{
    private Animator anim;	
    private Rigidbody rbody;

    private Transform leftFoot;
    private Transform rightFoot;

    public int groundContacts = 0;
   
    private float filteredForwardInput = 0f;
    private float filteredTurnInput = 0f;

    public float forwardInputFilter = 5f;
    public float turnInputFilter = 5f;

    private float forwardSpeedLimit = 1f;

    public bool IsGrounded
    {
        get { return groundContacts > 0; }
    }


    void Awake()
    {

        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

    }


    // Use this for initialization
    void Start()
    {
		//example of how to get access to certain limbs
        leftFoot = this.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot");
        rightFoot = this.transform.Find("mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg/mixamorig:RightFoot");

        if (leftFoot == null || rightFoot == null)
            Debug.Log("One of the feet could not be found");

    }





    //Update whenever physics updates with FixedUpdate()
    //Updating the animator here should coincide with "Animate Physics"
    //setting in Animator component under the Inspector
    void FixedUpdate()
    {
	
        //GetAxisRaw() so we can do filtering here instead of the InputManager
        float h = Input.GetAxisRaw("Horizontal");// setup h variable as our horizontal input axis
        float v = Input.GetAxisRaw("Vertical");	// setup v variables as our vertical input axis


        //enforce circular joystick mapping which should coincide with circular blendtree positions
        Vector2 vec = Vector2.ClampMagnitude(new Vector2(h, v), 1.0f);

        h = vec.x;
        v = vec.y;


        //BEGIN ANALOG ON KEYBOARD DEMO CODE
        if (Input.GetKey(KeyCode.Q))
            h = -0.5f;
        else if (Input.GetKey(KeyCode.E))
            h = 0.5f;

        if (Input.GetKeyUp(KeyCode.Alpha1))
            forwardSpeedLimit = 0.1f;
        else if (Input.GetKeyUp(KeyCode.Alpha2))
            forwardSpeedLimit = 0.2f;
        else if (Input.GetKeyUp(KeyCode.Alpha3))
            forwardSpeedLimit = 0.3f;
        else if (Input.GetKeyUp(KeyCode.Alpha4))
            forwardSpeedLimit = 0.4f;
        else if (Input.GetKeyUp(KeyCode.Alpha5))
            forwardSpeedLimit = 0.5f;
        else if (Input.GetKeyUp(KeyCode.Alpha6))
            forwardSpeedLimit = 0.6f;
        else if (Input.GetKeyUp(KeyCode.Alpha7))
            forwardSpeedLimit = 0.7f;
        else if (Input.GetKeyUp(KeyCode.Alpha8))
            forwardSpeedLimit = 0.8f;
        else if (Input.GetKeyUp(KeyCode.Alpha9))
            forwardSpeedLimit = 0.9f;
        else if (Input.GetKeyUp(KeyCode.Alpha0))
            forwardSpeedLimit = 1.0f;
        //END ANALOG ON KEYBOARD DEMO CODE  


        //do some filtering of our input as well as clamp to a speed limit
        filteredForwardInput = Mathf.Clamp(Mathf.Lerp(filteredForwardInput, v, 
                Time.deltaTime * forwardInputFilter), -forwardSpeedLimit, forwardSpeedLimit);
        
        filteredTurnInput = Mathf.Lerp(filteredTurnInput, h, 
            Time.deltaTime * turnInputFilter);
                                                    
        //finally pass the processed input values to the animator
        anim.SetFloat("velx", filteredTurnInput);	// set our animator's float parameter 'Speed' equal to the vertical input axis				
        anim.SetFloat("vely", filteredForwardInput); // set our animator's float parameter 'Direction' equal to the horizontal input axis		


        //if (Input.GetButtonDown("Fire1")) //normally left-ctrl on keyboard
        //    anim.SetTrigger("throw"); 
    }


    //This is a physics callback
    void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
        {
            ++groundContacts;

            //Debug.Log("Player hit the ground at: " + collision.impulse.magnitude);

            if (collision.impulse.magnitude > 100f)
            {               
                EventManager.TriggerEvent<PlayerLandsEvent, Vector3>(collision.contacts[0].point);
            }
        }
						
    }

    //This is a physics callback
    void OnCollisionExit(Collision collision)
    {

        if (collision.transform.gameObject.tag == "ground")
            --groundContacts;
    }



    void OnAnimatorMove()
    {
        if (IsGrounded)
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
        				
    }

}
