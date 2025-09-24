using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnScript : MonoBehaviour
{
    public Transform player;
    public Quaternion originalRotation;
    public Vector3 origianlPosition;

    public void SaveOriginalRotationPosition()
    {
        originalRotation = transform.rotation;
        origianlPosition = transform.position;  
    }

    public void LoadOriginalRotationPositon()
    {
        transform.rotation = originalRotation;
        transform.position = origianlPosition;
    }

    public IEnumerator TurnToPlayer()
    {
        float duration = 1.0f;
        float elapsedTime = 0f;

        Vector3 directionToPlayer = player.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        targetRotation *= Quaternion.Euler(0, 180, 0);

        Quaternion startRotation = transform.rotation;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    public IEnumerator ReturnToOriginalRotation()
    {
        float duration = 1.0f;
        float elapsedTime = 0f;

        Quaternion startRotation = transform.rotation;
        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, originalRotation, elapsedTime / duration);
            transform.position = Vector3.Lerp(startPosition, origianlPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.rotation = originalRotation;
        transform.position = origianlPosition;
    }
}


