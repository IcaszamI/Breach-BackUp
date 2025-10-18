using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public bool currentlyInteracting;

    void Start()
    {
        currentlyInteracting = false;
    }

    public void CurrentlyInteractingTrue(){
        currentlyInteracting = true;
    }

    public void CurrentlyInteractingFalse(){
        currentlyInteracting = false;
    }
}
