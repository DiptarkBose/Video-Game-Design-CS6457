using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSphereCollisionReporter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision c)
    {
        EventManager.TriggerEvent<BombBounceEvent, Vector3>(c.contacts[0].point);
    }
}
