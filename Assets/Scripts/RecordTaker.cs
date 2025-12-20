using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class RecordTaker : MonoBehaviour
{

    public TMP_Text recordText;
    private int record;

    private void OnEnable()
    {
        record = PlayerPrefs.GetInt("Record", 0);
        UpdateUI();
    }
    void UpdateUI()
    {
      
        recordText.text = "Record: "+record.ToString();
    }
}
