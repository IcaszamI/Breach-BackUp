using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CriteriaHandler : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI criteria;
    void Update()
    {
        int currentCriteria = GameManager.Instance.currentDay;

        switch (currentCriteria)
        {
            case 1:
                criteria.text = "--Make sure to only reply to proper domain emails ( breachnetworks.com.ph )";
                break;
            case 2:
                criteria.text = "--Make sure files received are safe by right-clicking and scanning them";
                break;
            case 3:
                criteria.text = "--See if applications requested by colleagues are not blacklisted";
                break;
            case 4:
                criteria.text = "--Make sure to only reply to proper domain emails ( breachnetworks.com.ph )\n--Make sure files received are safe by right-clicking and scanning them\n--See if applications requested by colleagues are not blacklisted";
                break;
            default:
                criteria.text = "--No Criteria Today";
                break;
        }
    }
}
