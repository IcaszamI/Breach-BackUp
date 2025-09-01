using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    Animator anim;
    FirstPersonController controller;
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponentInParent<FirstPersonController>();

    }

    void Update()
    {
        anim.SetBool("IsGrounded",controller.Grounded);
    }
}
