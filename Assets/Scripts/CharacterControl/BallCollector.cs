using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollector : MonoBehaviour
{
    public bool hasBall;
    // Start is called before the first frame update
    void Start()
    {
        hasBall = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void receiveBall()
    {
        hasBall = true;
    }
}
