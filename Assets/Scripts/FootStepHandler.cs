using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepHandler : MonoBehaviour
{
    private AudioSource footstepAudio;



    public void OnFootstep()
    {
        if (footstepAudio != null)
            footstepAudio.Play();
    }
}
