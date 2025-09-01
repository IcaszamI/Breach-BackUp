using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPlayerPrefsKey : MonoBehaviour
{
    public string keyToClear;
    [ContextMenu("Execute Clear Key")]
    public void ClearKey()
    {
        if (string.IsNullOrEmpty(keyToClear))
        {
            Debug.LogError("Please enter a key to clear in the inspector");
            return;
        }
        if (PlayerPrefs.HasKey(keyToClear))
        {
            PlayerPrefs.DeleteKey(keyToClear);
            PlayerPrefs.Save();
            Debug.Log($"succesfully deleted Player Prefs key: {keyToClear}");
        }
        else
        {
            Debug.LogWarning($"PlayerPrefs key '{keyToClear}' not found. Nothing to delete");
        }
    }
}
