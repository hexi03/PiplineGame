using System;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using System.Collections.Generic;
using DefaultNamespace;

[System.Serializable]
public class LevelData
{
    public string name;
    public string difficulty;
    public string mapPath;
    public int id;

    public int maxScore;
    public bool isCompleted;
}

[System.Serializable]
public class LevelList
{
    public List<LevelData> levels;
}

public class LevelManager : MonoBehaviour
{

    public Transform levelButtonParent; // Родительский объект для кнопок
    public GameObject levelButtonPrefab; // Префаб кнопки уровня

    private DatabaseManager dbManager;

    public string levelsJsonPath = "levels.json"; // путь до json файла с уровнями 
    public List<LevelData> levels;
    public List<GameObject> buttons;

    void Start()
    {
        dbManager = FindObjectOfType<DatabaseManager>();
        DebugLogObject.log("Level loading...");
        
        
    }

    public void Reload()
    {
        StartCoroutine(LoadLevels(() =>
        {
            buttons.ForEach(o => Destroy(o));
            DebugLogObject.log("DB level data fetching...");
            LoadLevelStatesFromDatabase();
            DebugLogObject.log("Creating buttons...");
            CreateLevelButtons();
        })); 
    }

    IEnumerator LoadLevels(System.Action callback)
    {
        JSONLoader loader = new JSONLoader();
        yield return loader.LoadJSON(levelsJsonPath, () =>
        {
            if (!string.IsNullOrEmpty(loader.jsonString))
            {
                DebugLogObject.log("Trying to parse");
                // Десериализуем JSON в список уровней
                LevelList levelList = JsonUtility.FromJson<LevelList>(loader.jsonString);
                DebugLogObject.log("Success! Level count: " + levelList.levels.Count);
                levels = new List<LevelData>(levelList.levels);

                // Теперь у тебя есть список уровней в переменной `levels`
                foreach (LevelData level in levels)
                {
                    Debug.Log($"Уровень: {level.name}, Сложность: {level.difficulty}, Файл карты: {level.mapPath}");
                    DebugLogObject.log($"Level: {level.name}, Difficulty: {level.difficulty}, Map file: {level.mapPath}");
                }

                callback();
            }
        });
        
    }




    void LoadLevelStatesFromDatabase()
    {
        foreach (var level in levels)
        {
            LevelState state = dbManager.GetLevelState(level.id);
            if (state == null) {
                dbManager.UpdateLevelState(level.id, false, 0);
                state = new LevelState(false, 0);
            }
            level.isCompleted = state.IsCompleted == 1;  // Нужно добавить в LevelData флаг isCompleted
            level.maxScore = state.MaxScore;
        }
    }

    void CreateLevelButtons()
    {
        foreach (var level in levels)
        {
            DebugLogObject.log("Button created for " + level.name);
            GameObject buttonObj = Instantiate(levelButtonPrefab, levelButtonParent);
            LevelButton button = buttonObj.GetComponent<LevelButton>();
            buttonObj.SetActive(true);
            button.Setup(level);
            buttons.Add(buttonObj);
        }
    }
}
