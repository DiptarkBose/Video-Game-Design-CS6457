using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackJumpingBean : MonoBehaviour
{
    public Rigidbody rb;
    public bool isGrounded;
    public Vector3 jump;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 1.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isGrounded && transform.position.y <=1)
        {
            int random = UnityEngine.Random.Range(1, 1001);

            if (random <= 15)
            {
                int randomForce = UnityEngine.Random.Range(1, 11);
                rb.AddForce(randomForce * jump, ForceMode.Impulse);
                int randomTorqueX = UnityEngine.Random.Range(1, 21);
                int randomTorqueY = UnityEngine.Random.Range(1, 21);
                int randomTorqueZ = UnityEngine.Random.Range(1, 21);
                rb.AddTorque(randomTorqueX, randomTorqueY, randomTorqueZ, ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }

    //Change isGrounded status to true when on ground
    void OnCollisionStay()
    {
        isGrounded = true;
    }

}
