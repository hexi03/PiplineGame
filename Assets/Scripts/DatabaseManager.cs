using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using SQLite4Unity3d;

public class DatabaseManager : MonoBehaviour
{
    private SQLiteConnection connection;
    private string dbPath;

    void Awake()
    {
        dbPath = Path.Combine(Application.persistentDataPath, "GameDatabase.db");
        connection = new SQLiteConnection(dbPath);
        CreateDatabase();
    }

    private void OnDestroy()
    {
        connection?.Close();
    }

    // Создание таблиц
    public void CreateDatabase()
    {
        try
        {
            connection.CreateTable<LevelState>();
            connection.CreateTable<Attempt>();
            Debug.Log("Tables 'level_state' and 'score' created or already exist.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to create database: " + ex.Message);
        }
    }

    // Получение состояния уровня
    public LevelState GetLevelState(int levelId)
    {
        try
        {
            return connection.Find<LevelState>(levelId);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to get level state: " + ex.Message);
            return null;
        }
    }
    
    // Получение состояния уровня
    public List<LevelState> GetAllLevelStates()
    {
        try
        {
            return connection.Table<LevelState>().ToList();
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to get level states: " + ex.Message);
            return null;
        }
    }

    // Добавление нового счёта
    public void AddScore(int levelId, int value, int attemptNumber)
    {
        try
        {
            var score = new Attempt
            {
                LevelId = levelId,
                Score = value,
                AttemptNumber = attemptNumber,
                CreatedAt = DateTime.UtcNow
            };
            connection.Insert(score);
            Debug.Log("Score added for level: " + levelId);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to add score: " + ex.Message);
        }
    }

    // Получение всех счётов для уровня
    public List<Attempt> GetAttemptsForLevel(int levelId)
    {
        try
        {
            return connection.Table<Attempt>().Where(s => s.LevelId == levelId).ToList();
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to get scores: " + ex.Message);
            return new List<Attempt>();
        }
    }

    // Обновление состояния уровня
    public void UpdateLevelState(int levelId, bool isCompleted, int maxScore)
    {
        var levelState = GetLevelState(levelId);

        if (levelState == null)
        {
            levelState = new LevelState { LevelId = levelId, IsCompleted = isCompleted ? 1 : 0, MaxScore = maxScore };
            connection.Insert(levelState);
        }
        else
        {
            levelState.IsCompleted = isCompleted ? 1 : 0;
            levelState.MaxScore = maxScore;
            connection.Update(levelState);
        }

        Debug.Log("Level state updated for level: " + levelId);
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

[Table("score")]
public class Attempt
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed] // Создаёт индекс для ускорения поиска
    public int LevelId { get; set; } // Связь с LevelState

    public int AttemptNumber { get; set; } // Номер попытки
    public int Score { get; set; } // Значение счёта

    public DateTime CreatedAt { get; set; } // Время записи
}

