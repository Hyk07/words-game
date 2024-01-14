using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Letter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    //DATA
    public string letter;
    public int index;
    public static bool isPressed;
    public static bool isReverse;

    //OBJECT
    public TMP_Text letterText;    
    public RectTransform position;
    public Vector3 lineCord;
    public RawImage bg;

    void Start()
    {
        isPressed = false;
        letter = letterText.text;
    }

    private void Update() 
    {
        if (isPressed == false) 
        {
            bg.color = new Color32(255, 255, 255, 255);
            letterText.color = new Color32(0, 0, 0, 255);
        }
        else if (isPressed == true) 
        {            
            if (!getLetter.Instance.indexs.Contains(index) && isReverse == true) 
            {
                bg.color = new Color32(255, 255, 255, 255);
                letterText.color = new Color32(0, 0, 0, 255);
            }           
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isReverse = false;
        getLetter.Instance.line.positionCount = 0;
        getLetter.Instance.line.positionCount += 1;
        getLetter.Instance.line.SetPosition(getLetter.Instance.line.positionCount - 1,(position.position));
        getLetter.Instance.line.positionCount += 1; 
        isPressed = true;
        getLetter.Instance.lettersToWord = "";
        getLetter.Instance.letterTMP.text = "";
        getLetter.Instance.letters.Clear();
        getLetter.Instance.indexs.Clear();
        getLetter.Instance.letters.Add(letter);
        getLetter.Instance.letterTMP.text += letter;
        getLetter.Instance.letterTMP.color = Color.white;
        getLetter.Instance.indexs.Add(index);
        bg.color = new Color32(26, 78, 24, 255);
        letterText.color = new Color32(255, 255, 255, 255);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isReverse = false;
        isPressed = false;
        letterText.color = new Color32(0, 0, 0, 255);
        getLetter.Instance.line.positionCount = 0;
        foreach (string letter in getLetter.Instance.letters) 
        {
            getLetter.Instance.lettersToWord += letter;
        }
        StartCoroutine(getLetter.Instance.WordCheck());
        Debug.Log(getLetter.Instance.lettersToWord);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {        
        if (isPressed == true && !getLetter.Instance.indexs.Contains(index))
        {
            bg.color = new Color32(26, 78, 24, 255);
            getLetter.Instance.line.SetPosition(getLetter.Instance.line.positionCount - 1, (position.position));
            getLetter.Instance.line.positionCount += 1;               
            getLetter.Instance.letters.Add(letter);
            getLetter.Instance.letterTMP.text += letter;
            letterText.color = new Color32(255, 255, 255, 255);
            getLetter.Instance.indexs.Add(index);
        }
        if (isPressed == true && getLetter.Instance.letters.Contains(letter))
        {                
            if (getLetter.Instance.letters.Count > 1 && getLetter.Instance.letters[getLetter.Instance.letters.Count - 2].ToString() == letter && getLetter.Instance.indexs[getLetter.Instance.indexs.Count - 2] == index) 
            {
                isReverse = true;
                getLetter.Instance.letters.RemoveAt(getLetter.Instance.letters.Count - 1);
                getLetter.Instance.indexs.RemoveAt(getLetter.Instance.indexs.Count - 1);
                getLetter.Instance.letterTMP.text = getLetter.Instance.letterTMP.text.Substring(0,getLetter.Instance.letterTMP.text.Length - 1);
                getLetter.Instance.line.positionCount -= 1;                
            }            
        }
    }
}
