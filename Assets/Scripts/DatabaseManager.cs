using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using UnityEngine;
using SQLite4Unity3d;

public class DatabaseManager : MonoBehaviour
{
    private SQLiteConnection connection;
    private string dbPath;

    void Awake()
    {
        // Указываем путь к базе данных внутри контейнера приложения
        dbPath = Path.Combine(Application.persistentDataPath, "GameDatabase.db");

        // Инициализируем соединение и создаём таблицу, если она не существует
        connection = new SQLiteConnection(dbPath);
        CreateDatabase();
    }

    private void OnDestroy()
    {
        connection?.Close();
    }

    // Метод для создания таблицы, если она не существует
    public void CreateDatabase()
    {
        try
        {
            connection.CreateTable<LevelState>();
            Debug.Log("Table 'level_state' created or already exists.");
            DebugLogObject.log("Table 'level_state' created or already exists.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to create or connect to database: " + ex.Message);
            DebugLogObject.log("Failed to create or connect to database: " + ex.Message);
        }
    }

    // Метод для получения состояния уровня
    public LevelState GetLevelState(int levelId)
    {
        try
        {
            return connection.Find<LevelState>(levelId);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to get level state: " + ex.Message);
            DebugLogObject.log("Failed to get level state: " + ex.Message);
            return null;
        }
    }

    // Метод для обновления состояния уровня
    public void UpdateLevelState(int levelId, bool isCompleted, int maxScore)
    {
        var levelState = GetLevelState(levelId);

        if (levelState == null)
        {
            // Создаём новую запись, если её нет
            levelState = new LevelState { LevelId = levelId, IsCompleted = isCompleted ? 1 : 0, MaxScore = maxScore };
            
            Debug.LogError("Insert result: " + connection.Insert(levelState));
        }
        else
        {
            // Обновляем существующую запись
            levelState.IsCompleted = isCompleted ? 1 : 0;
            levelState.MaxScore = maxScore;
            connection.Update(levelState);
        }
        
        Debug.Log("Level state updated for level: " + levelId);
        DebugLogObject.log("Level state updated for level: " + levelId);
    }
}

// Определяем класс LevelState для таблицы
[Table("level_state")]
public class LevelState
{
    public LevelState()
    {
        
    }

    public LevelState(bool isCompleted, int maxScore)
    {
        IsCompleted = isCompleted ? 1 : 0;
        MaxScore = maxScore;
    }

    [PrimaryKey, AutoIncrement]
    public int LevelId { get; set; }
    public int IsCompleted { get; set; } = 0;
    public int MaxScore { get; set; } = 0;
}
