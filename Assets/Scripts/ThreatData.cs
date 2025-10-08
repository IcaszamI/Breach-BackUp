using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewThreat", menuName = "Threat System/Threat Data")]
public class ThreatData : ScriptableObject
{
    public string threatName;
    [TextArea(3, 10)]
    public string threatDescription;
    public Sprite icon;
    [Range(1, 4)]
    public int dayAppears;
}
