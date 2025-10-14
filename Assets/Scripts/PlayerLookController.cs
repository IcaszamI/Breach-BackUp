using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookController : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public static PlayerLookController Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
        }
    }

    public IEnumerator TurnToFace(Vector3 targetPosition)
    {
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0;
        if (directionToTarget == Vector3.zero) yield break;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}

