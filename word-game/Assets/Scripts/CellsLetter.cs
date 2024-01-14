using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellsLetter : MonoBehaviour
{
    public string letter;
    public int index;
    public TMP_Text letterText;
    public RectTransform size;
    public RawImage bg;
    void Start()
    {
        letter = letterText.text;
        letterText.rectTransform.sizeDelta = size.sizeDelta;        
        letterText.color = new Color32(0, 0, 0, 0);
    }
}
