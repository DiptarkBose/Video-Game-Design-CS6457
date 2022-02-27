using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMagicAnimation : MonoBehaviour
{
    [SerializeField] private Animator magicObjectAnimator;
    [SerializeField] private string magic = "Magic";
    [SerializeField] private string idle = "Idle";
    [SerializeField] private string inflate = "Inflate";

    void OnTriggerEnter(Collider other)
    {
        magicObjectAnimator.Play(magic, 0, 30f);   
    }
    void OnTriggerExit(Collider other)
    {
        magicObjectAnimator.Play(idle, 0, 0.0f);
    }
}
