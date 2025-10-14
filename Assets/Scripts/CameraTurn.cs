using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTurn : MonoBehaviour
{
    public NPCcontroller[] npc;
    public GameObject turnTarget;
    private Quaternion originalRotation;

    void Awake()
    {
        originalRotation = transform.rotation;
    }

    public IEnumerator FaceNPC()
    {
        Debug.Log("Trying to turn");
        float duration = 1f;
        float elapsedTime = 0f;

        Vector3 directionToTarget = turnTarget.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));

        Quaternion startRotation = transform.rotation;

        while (elapsedTime < duration)
        {
            Debug.Log("Trying to turn");
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    public IEnumerator FaceComputer()
    {
        Debug.Log("Turning...");
        float duration = 1f;
        float elapsedTime = 0f;

        Quaternion startRotation = transform.rotation;
        while (elapsedTime < duration)
        {
            Debug.Log("Turning...");
            transform.rotation = Quaternion.Slerp(startRotation, originalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = originalRotation;
    }
}
