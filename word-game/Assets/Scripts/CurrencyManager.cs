using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public int coin;
    public Canvas canvas;
    public TMP_Text coinText;
    public TMP_Text nextLevelCoinText;
    public TMP_Text coinPlusText;
    public GameObject rows;
    public List<int> usedIndex;
    public int row;
    public int cell;
    public int number;
    public bool pressed;
    public AudioSource hintSound;
    public AudioSource clickSound;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        coinText.text = coin.ToString();
        nextLevelCoinText.text = coin.ToString();
    }

    public void Hint() 
    {        
        repeat1:
        row = 0;
        int count = rows.transform.childCount - 1;
        pressed = true;
        if (coin >= 50)
        {
            foreach (Transform child in rows.transform)
            {
                //ROW  
                if (pressed == true)
                {
                    if (cell >= child.GetComponent<GridsWord>().index.Count)
                    {
                        number = child.GetComponent<GridsWord>().index[0];
                    }
                    else
                    {
                        number = child.GetComponent<GridsWord>().index[cell];
                    }

                    if (usedIndex.Contains(number))
                    {
                        row += 1;
                        if (row > count)
                        {
                            row = 0;
                            cell += 1;
                            if (cell < child.GetComponent<GridsWord>().index.Count)
                            {
                                goto repeat1;
                            }
                        }
                    }
                    else if (!usedIndex.Contains(number))
                    {
                        CellsAdd(child);
                    }
                }
            }
        }
        else 
        {
            clickSound.Play();
        }
    }

    public void CellsAdd(Transform child) 
    {        
        foreach (Transform childschild in child.transform)
        {
            //CELL
            if (childschild.GetComponent<CellsLetter>().index == number)
            {
                coinPlusText.text = "-50";
                DOTween.Kill(coinPlusText.transform);
                DOTween.Kill(coinPlusText);
                coinPlusText.transform.position = coinText.transform.position;
                hintSound.Play();
                childschild.GetComponent<CellsLetter>().letterText.color = new Color32(0, 0, 0, 124);
                childschild.GetComponent<CellsLetter>().letterText.transform.position = childschild.GetComponent<CellsLetter>().size.position;
                childschild.GetComponent<CellsLetter>().letterText.transform.DOShakePosition(3f);
                usedIndex.Add(childschild.GetComponent<CellsLetter>().index);
                coinPlusText.color = Color.red;
                coinPlusText.DOFade(0f, 1f);
                coinPlusText.transform.DOMoveY(coinText.transform.position.y + 0.5f, 1f).SetEase(Ease.OutBounce); 
                coin -= 50;
            }
        }       
        pressed = false;
    }
}
