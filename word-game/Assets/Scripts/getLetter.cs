using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class getLetter : MonoBehaviour
{
    public static getLetter Instance;

    //DATA
    public List<PuzzleData> puzzleData = new List<PuzzleData>();
    public List<string> keyToWords = new List<string>();
    int theWordLength;
    public List<string> letters;
    public List<int> indexs;
    public string lettersToWord;
    public int indexLength;
    public float duration = 1f;
    public bool SoundToggle = true;

    //OBJECT
    public LineRenderer circleRender;
    public LineRenderer line;

    public TMP_Text letterTMP;
    public Letter prefab;
    public GameObject lettersGO;
    public Camera mainCamera;
    public RectTransform backgroundS;
    public RectTransform canvas;
    public Vector3 screenPosition;
    public CellsLetter gridletterPrefab;
    public GridsWord cellsPref;
    public GameObject rows;
    public RawImage nextLevel;
    public GameObject nextLevelMenu;
    public GameObject optionsMenu;
    public AudioSource bgMusic;
    public AudioSource shuffleSound;
    public AudioSource correctWordSound;
    public AudioSource clickSound;
    public Button toggleSound;
    public Slider soundSlider;
    public TMP_Text toggleOrNot;
    public GameObject LanguageChanged;
    public string currentLanguage;

    //RANDOM NUMBER WITHOUT REPEAT
    List<int> list = new List<int>();
    public int uniqRandom(int min, int max)
    {
        int number = Random.Range(min, max);
        while (list.Contains(number))
        {
            if (list.Contains(number))
            {
                number = Random.Range(min, max);
            }
            else
            {
                list.Add(number);
                return number;
            }
        }
        list.Add(number); return number;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentLanguage = LocalizationSettings.SelectedLocale.ToString();
        bgMusic.DOFade(1f,5f);
        puzzleData = getJSON.puzzleData;
        theWordLength = puzzleData[0].theWord.ToString().Length;
        DrawCircle(theWordLength, 1);
        AddWordToGrid();
        Values.Instance.currentRoundValue = getJSON.CurrentRound + 1;
    }


    private void Update()
    {
        DrawLine();
        string oldlanguage = currentLanguage;
        if (oldlanguage != LocalizationSettings.SelectedLocale.ToString()) 
        {
            LanguageChanged.SetActive(true);
            getJSON.Instance.GetJsonByLanguage();
            currentLanguage = LocalizationSettings.SelectedLocale.ToString();
            SceneManager.LoadScene("mainmenu");
        }        
    }

    public void OnSliderChange()
    {
        bgMusic.DOKill();
        bgMusic.volume = soundSlider.value;
    }

    //WORD - GRID
    void AddWordToGrid() 
    {
        CellsLetter cells;
        int index = 0;
        float sizef;
        Vector3 v3;

        backgroundS.sizeDelta = new Vector2(canvas.sizeDelta.x - 34f, backgroundS.sizeDelta.y);

        if (getJSON.MaxRowLength() < getJSON.MaxLength())
        {
            float size = backgroundS.sizeDelta.x / getJSON.MaxLength();
            sizef = size;
        }
        else if (getJSON.MaxRowLength() >= getJSON.MaxLength())
        {
            float size = backgroundS.sizeDelta.y / getJSON.MaxRowLength();
            sizef = size;
        }
        else
        {
            float size = 1f;
            sizef = size;
        }
        Debug.Log(getJSON.MaxLength());
        Debug.Log(sizef);
        v3 = new Vector3(sizef-3f, sizef-3f);

        gridletterPrefab.size.sizeDelta = v3;

        for (int i = 0; i < puzzleData[0].Words.Count; i++) 
        {          
            GridsWord grids = Instantiate(cellsPref, new Vector3(0f, 0f, 0f), Quaternion.identity);
            grids.transform.SetParent(rows.transform, false);
            grids.GetComponent<RectTransform>().sizeDelta = v3;
            grids.word = puzzleData[0].Words[i];
            for (int j = 0; j < puzzleData[0].Words[i].Length; j++) 
            {
                cells = Instantiate(gridletterPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                cells.transform.SetParent(grids.transform, false);
                cells.letterText.text = puzzleData[0].Words[i].Substring(j,1);
                cells.index = index;                
                grids.index.Add(index);
                
                index +=1;
                indexLength += 1;
            }
        }              
    }

    //MOUSE POSITION - LINE RENDERER
    void DrawLine() 
    {
        screenPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) * 50f;
        if (Letter.isPressed == true)
        {
            line.SetPosition(line.positionCount - 1, new Vector3 (screenPosition.x / 50f, screenPosition.y /50f, 90f));
        }
    }

    //CIRCLE LINE RENDERER
    void DrawCircle(int steps, float radius)
    {
        circleRender.positionCount = steps + 1;
        for (int currentSteps = 0; currentSteps < steps; currentSteps++)
        {
            float circumferenceProgress = (float)currentSteps / steps;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y-3f, 0);

            circleRender.SetPosition(currentSteps, currentPosition);
        }
        circleRender.SetPosition(circleRender.positionCount - 1, circleRender.GetPosition(0));
        AddLetter();
    }

    //ADD LETTER TO CIRCLE LINE RENDERER'S CORNERS
    void AddLetter()
    {        
        puzzleData = getJSON.puzzleData;
        string theWord = puzzleData[0].theWord.ToString();
        string[] letter = new string[theWordLength];
        for (int i = 0; i < theWordLength; i++)
        {
            letter[i] = theWord.Substring(i, 1);
        }
        for (int i = 0; i < theWordLength; i++)
        {
            Letter pref = Instantiate(prefab, (circleRender.GetPosition(i) * 50f), Quaternion.identity);
            pref.letterText.text = letter[uniqRandom(0, theWordLength)];
            pref.index = i;
            pref.transform.SetParent(lettersGO.transform, false);
            pref.transform.GetChild(0).DOShakePosition(0.5f);
            pref.transform.GetChild(0).DOShakeRotation(0.5f);
            pref.transform.DOPunchScale(pref.transform.localScale*0.2f,0.5f);
        }
    }

    public void Shuffle() 
    {
        shuffleSound.Play(); 
        list.Clear();
        foreach (Transform child in lettersGO.transform)
        {
            Destroy(child.gameObject);
        }
        AddLetter();
    }

    //CHECK
    public IEnumerator WordCheck()
    {        
        if (puzzleData[0].Words.Contains(lettersToWord) && Letter.isPressed == false)
        {
            if (!keyToWords.Contains(lettersToWord))
            {
                //CORRECT WORD
                keyToWords.Add(lettersToWord);
                letterTMP.color = Color.green;
                letterTMP.transform.DOShakePosition(1f,2f);
                correctWordSound.Play();
                foreach (Transform child in rows.transform) 
                {
                    if (child.GetComponent<GridsWord>().word == lettersToWord) 
                    {
                        float animDuration = duration / (child.childCount+3);
                        foreach (Transform childschild in child.transform)
                        {
                            if (!CurrencyManager.Instance.usedIndex.Contains(childschild.GetComponent<CellsLetter>().index))
                            {
                                CurrencyManager.Instance.usedIndex.Add(childschild.GetComponent<CellsLetter>().index);
                            }
                        }
                        foreach (Transform childschild in child.transform) 
                        {                            
                            childschild.GetComponent<CellsLetter>().letterText.color = new Color32(12, 137, 65, 1);
                            DOTween.Kill(childschild.GetComponent<CellsLetter>().letterText.transform);

                            childschild.GetComponent<CellsLetter>().letterText.CrossFadeAlpha(255f,1f,false);
                            childschild.GetComponent<CellsLetter>().letterText.transform.DOScale(1.15f, animDuration).SetEase(Ease.OutBounce);
                            childschild.GetComponent<CellsLetter>().bg.DOColor(new Color32(12, 137, 65, 255), animDuration).SetEase(Ease.InCirc);
                            childschild.GetComponent<CellsLetter>().letterText.DOColor(Color.white, animDuration).SetEase(Ease.InCirc);
                            yield return new WaitForSeconds(0.10f);                                                       
                        }
                    }
                }
                //ADD COIN
                DOTween.Kill(CurrencyManager.Instance.coinPlusText.transform);
                DOTween.Kill(CurrencyManager.Instance.coinPlusText);
                CurrencyManager.Instance.coinPlusText.transform.position = CurrencyManager.Instance.coinText.transform.position;
                CurrencyManager.Instance.coinPlusText.text = "+50";
                CurrencyManager.Instance.coinPlusText.color = Color.green;
                CurrencyManager.Instance.coinPlusText.DOFade(0f, 1f);
                CurrencyManager.Instance.coinPlusText.transform.DOMoveY(CurrencyManager.Instance.coinText.transform.position.y + 0.5f, 1f).SetEase(Ease.OutBounce);
                CurrencyManager.Instance.coin += 50;
                if (Enumerable.SequenceEqual(puzzleData[0].Words.OrderBy(e => e), keyToWords.OrderBy(e => e)))
                {
                    //NEXT
                    yield return new WaitForSeconds(1f);
                    nextLevel.CrossFadeAlpha(255f,0.50f,false);
                    yield return new WaitForSeconds(0.50f);
                    nextLevelMenu.SetActive(true);
                    NextWordFunct();
                }
            }
        }
        else 
        {
            letterTMP.text = "";
        }        
    }

    //NEXT WORD
    public void NextWord()
    {
        clickSound.Play();
        nextLevelMenu.SetActive(false);        
        nextLevel.CrossFadeAlpha(0f, 0.75f, false);
    }

    void NextWordFunct()
    {
        letterTMP.text = "";
        list.Clear();
        foreach (Transform child in lettersGO.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in rows.transform)
        {
            Destroy(child.gameObject);
        }
        if (getJSON.CurrentRound > getJSON.levelName.Count - 1)
        {
            getJSON.CurrentRound = 0;
        }
        if (!getJSON.passedLevels.Contains(getJSON.CurrentRound+1))
        {
            //Bir sonraki bölümün kilidini açmak için listeye atar
            getJSON.passedLevels.Add(getJSON.CurrentRound+1);
        }
        getJSON.CurrentRound++;
        getJSON.GetData();
        puzzleData = getJSON.puzzleData;
        Values.Instance.currentRoundValue = getJSON.CurrentRound + 1;
        theWordLength = puzzleData[0].theWord.ToString().Length;
        DrawCircle(theWordLength, 1);
        keyToWords.Clear();
        CurrencyManager.Instance.usedIndex.Clear();
        CurrencyManager.Instance.row = 0;
        CurrencyManager.Instance.cell = 0;
        indexLength = 0;
        AddWordToGrid();
    }

    //BACK TO MAIN MENU
    public void BackToMenu() 
    {
        clickSound.Play();
        SceneManager.LoadScene("mainmenu");
    }

    public void OptionsOpen()
    {
        clickSound.Play();
        optionsMenu.SetActive(true);
    }

    public void OptionsClose() 
    {
        clickSound.Play();
        optionsMenu.SetActive(false);
    }

    public void ToggleSound() 
    {
        bgMusic.DOKill();
        if (SoundToggle == true) 
        {
            toggleOrNot.enabled = true;
            //OFF
            bgMusic.mute = true;
            SoundToggle = false;
        }
        else 
        {
            toggleOrNot.enabled = false;
            //ON
            bgMusic.mute = false;
            SoundToggle = true;
        }
    }   
}
