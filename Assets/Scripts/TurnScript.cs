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
        originalRotation = base.transform.rotation;
        origianlPosition = base.transform.position;

    }

    public void LoadOriginalRotationPositon()
    {
        base.transform.rotation = originalRotation;
        base.transform.position = origianlPosition;
    }


    public IEnumerator TurnToPlayer()
    {
        float duration = 1.0f;
        float elapsedTime = 0f;

        Vector3 directionToPlayer = player.position - base.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
        targetRotation *= Quaternion.Euler(0, 180, 0);

        Quaternion startRotation = base.transform.rotation;

        while (elapsedTime < duration)
        {
            base.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        base.transform.rotation = targetRotation;
    }

    public IEnumerator ReturnToOriginalRotation()
    {
        float duration = 1.0f;
        float elapsedTime = 0f;

        Quaternion startRotation = base.transform.rotation;
        Vector3 startPosition = base.transform.position;

        while (elapsedTime < duration)
        {
            base.transform.rotation = Quaternion.Slerp(startRotation, originalRotation, elapsedTime / duration);
            base.transform.position = Vector3.Lerp(startPosition, origianlPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        base.transform.rotation = originalRotation;
        base.transform.position = origianlPosition;
    }

}


