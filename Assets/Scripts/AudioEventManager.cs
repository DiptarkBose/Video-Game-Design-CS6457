using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventManager : MonoBehaviour
{

    public AudioClip boxAudio;
    public AudioClip playerLandsAudio;


    private UnityAction<Vector3> boxCollisionEventListener;

    private UnityAction<Vector3> playerLandsEventListener;

   

    void Awake()
    {

        boxCollisionEventListener = new UnityAction<Vector3>(boxCollisionEventHandler);

        playerLandsEventListener = new UnityAction<Vector3>(playerLandsEventHandler);

    }


    // Use this for initialization
    void Start()
    {


        			
    }


    void OnEnable()
    {

        EventManager.StartListening<BoxCollisionEvent, Vector3>(boxCollisionEventListener);
        EventManager.StartListening<PlayerLandsEvent, Vector3>(playerLandsEventListener);
    }

    void OnDisable()
    {

        EventManager.StopListening<BoxCollisionEvent, Vector3>(boxCollisionEventListener);
        EventManager.StopListening<PlayerLandsEvent, Vector3>(playerLandsEventListener);
    }


	
    // Update is called once per frame
    void Update()
    {
    }


 

    void boxCollisionEventHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(this.boxAudio, worldPos);
    }

    void playerLandsEventHandler(Vector3 worldPos)
    {
        AudioSource.PlayClipAtPoint(this.playerLandsAudio, worldPos);
    }

}
