using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class getJSON : MonoBehaviour
{
    public static getJSON Instance;

    public static int CurrentRound = 0;
    public static List<int> passedLevels = new List<int>();
    public static List<ReadData> levelName = new List<ReadData>();
    public  List<ReadData> LevelName = new List<ReadData>();
    public static List<PuzzleData> puzzleData = new List<PuzzleData>();
    public GameObject container;
    public GameObject selectLevel;
    public Level LevelPref;
    public string[] language;
    public string currentLanguage;
    public List<Sprite> flags;
    public Image languagebutton;
    public GameObject loadingScreen;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {        
        language = LocalizationSettings.SelectedLocale.ToString().ToLower().Split(char.Parse(" "));
        if (language[0] == "turkish"){languagebutton.sprite = flags[1];} else { languagebutton.sprite = flags[0]; }
        currentLanguage = LocalizationSettings.SelectedLocale.ToString();
        passedLevels.Add(0);

        GetJsonByLanguage();
    }

    private void Update()
    {
        string oldlanguage = currentLanguage;
        if (oldlanguage != LocalizationSettings.SelectedLocale.ToString())
        {
            loadingScreen.SetActive(true);
            foreach (Transform itemtodel in container.transform)
            {
                Destroy(itemtodel.gameObject);
            }            
            GetJsonByLanguage();            
        }
        if (language[0] == "turkish") { languagebutton.sprite = flags[1]; } else { languagebutton.sprite = flags[0]; }
    }

    public void languageChangeButton() 
    {
        if (language[0] == "turkish") { LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0]; } else { LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1]; }        
    }

    public void GetJsonByLanguage() 
    {        
            levelName.Clear();
            LevelName.Clear();
            puzzleData.Clear();
            language = LocalizationSettings.SelectedLocale.ToString().ToLower().Split(char.Parse(" "));
            Debug.Log(language[0]);
            int index = 0;

            string data = Resources.Load<TextAsset>(string.Format("Puzzles/{0}", "json_" + language[0])).text;
            Dictionary<string, object> root = (Dictionary<string, object>)Json.Deserialize(data);
            List<object> levels = (List<object>)root["Levels"];

            if (SceneManager.GetActiveScene().name == "mainmenu") 
            {
              foreach (var level in levels)
              {
                ReadData _item = new ReadData((Dictionary<string, object>)level);
                levelName.Add(_item);
                LevelName.Add(_item);
                Debug.Log(LevelName[index].levelName.ToString());
                Level levelPref = Instantiate(LevelPref, new Vector3(0f, 0f, 0f), Quaternion.identity);
                levelPref.transform.SetParent(container.transform, false);
                levelPref.levelName = LevelName[index].levelName;
                levelPref.index = index;
                if (!passedLevels.Contains(index))
                { levelPref.locked = true; }
                else
                { levelPref.locked = false; }
                index++;                
              }
            loadingScreen.SetActive(false);
            }           
            currentLanguage = LocalizationSettings.SelectedLocale.ToString();
            
    }

    public static void GetData()
    {
        if (CurrentRound > levelName.Count - 1) { CurrentRound = 0; }
        List<string> words = new List<string>();
        puzzleData.Clear();
        for (int i = 0; i < levelName[CurrentRound].words.Count; i++)
        {           
            words.Add(levelName[CurrentRound].words[i].words.ToString());            
        }
        PuzzleData _item = new PuzzleData(words, levelName[CurrentRound].levelName.ToString(), levelName[CurrentRound].theWord.ToString());
        puzzleData.Add(_item);
    }

    public void StartGame()
    {
        GetData();
        selectLevel.SetActive(true);
    }

    public void BackToMenu() 
    {
        selectLevel.SetActive(false);
    }
    public static float MaxLength() 
    {
        int i = 0;
        foreach (ReadData item in levelName) 
        {            
            if (item.theWord.Length >= i) 
            {
                i = item.theWord.Length;
            }
        }
        return i;
    }

    public static float MaxRowLength()
    {
        int i = 0;
        foreach (ReadData item in levelName)
        {
            if (item.words.Count >= i)
            {
                i = item.words.Count;
            }
        }
        return i;
    }
}

[System.Serializable]
public class ReadData
{
    public string levelName;
    public string theWord;
    public List<ReadWordData> words = new List<ReadWordData>();

    public ReadData(Dictionary<string, object> root)
    {
        levelName = root["levelName"].ToString();
        theWord = root["the_word"].ToString();

        List<object> wordslevels = (List<object>)root["words"];
        foreach (var wordlevel in wordslevels)
        {
            ReadWordData _item = new ReadWordData((Dictionary<string, object>)wordlevel);
            words.Add(_item);
        }
    }
}


[System.Serializable]
public class ReadWordData
{
    public string words;
    public ReadWordData(Dictionary<string, object> root)
    {
        words = root["word"].ToString();
    }
}

[System.Serializable]
public class PuzzleData 
{
    public string levelName;
    public string theWord;
    public List<string> Words;

    public PuzzleData(List<string> word, string LevelName,string TheWord) 
    {
        levelName = LevelName;
        theWord = TheWord;
        Words = word;
    }
}

