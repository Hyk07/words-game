using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour, IPointerDownHandler
{
    public string levelName;
    public int index;
    public bool locked;

    public GameObject lockedImg; 
    public TMP_Text levelNameTxt;   

    private void Start()
    {
        levelNameTxt.text = levelName;
    }

    private void Update()
    {
        if (locked)
        {
            lockedImg.SetActive(true);
        }
        else 
        { 
            lockedImg.SetActive(false);
        }
        if (getJSON.passedLevels.Contains(index)) 
        {
            locked = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!locked)
        {
            getJSON.CurrentRound = index;
            getJSON.GetData();
            SceneManager.LoadScene("game");
        }        
    }
}
