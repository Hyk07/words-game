using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using RTLTMPro;
public class Values : MonoBehaviour
{
    public static Values Instance;

    public float currentRoundValue = 1;
    public LocalizedString myString;
    public RTLTextMeshPro localizedText;

    private void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        myString.Arguments = new[] { this };
        myString.StringChanged += UpdateString;
    }

    void OnDisable()
    {
        myString.StringChanged -= UpdateString;
    }

    void UpdateString(string s)
    {
        localizedText.text = s;
    }

    void OnGUI()
    {
        myString.RefreshString();
        GUILayout.Label(localizedText.text);
    }
}
