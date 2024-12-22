using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using DefaultNamespace;
using Unity.VisualScripting;

public class StatisticsMenu : MonoBehaviour
{
    public GameObject levelPrefab; // Префаб для уровня
    public GameObject attemptPrefab; // Префаб для попытки
    public Transform statsContainer; // Контейнер для всех уровней

    public DatabaseManager databaseManager;
    public LevelManager levelManager;
    void Start()
    {
        PopulateStatsMenu();
    }

    public void Reload()
    {
        PopulateStatsMenu();
    }

    public void PopulateStatsMenu()
{
    Debug.Log("Starting PopulateStatsMenu...");

    // Очищаем контейнер перед заполнением
    foreach (Transform child in statsContainer)
    {
        if (!child.IsDestroyed()){
            Debug.Log($"Destroying child: {child.name}");
            Destroy(child.gameObject);
        }
    }

    Debug.Log("All children destroyed. Fetching levels from levelManager...");

    // Получаем список уровней из базы данных
    List<LevelData> levels = levelManager.levels;

    Debug.Log($"Levels fetched. Count: {levels.Count}");

    foreach (LevelData level in levels)
    {
        Debug.Log($"Processing Level ID: {level.id}");

        // Создаём элемент уровня
        GameObject levelGO = Instantiate(levelPrefab, statsContainer);
        Debug.Log($"111Processing Level ID: {level.id}");
        LevelStatsItem levelStats = levelGO.GetComponent<LevelStatsItem>();
        Debug.Log($"222Processing Level ID: {level.id}");

        // Проверяем наличие компонента
        if (levelStats == null)
        {
            Debug.LogError($"LevelStatsItem component missing on level prefab for Level ID: {level.id}");
            continue;
        }

        // Заполняем данные для уровня
        levelStats.levelNameText.text = $"Level {level.id}";
        levelStats.maxScoreText.text = $"{level.maxScore}";
        levelStats.difficultyText.text = $"{level.difficulty}";
        levelStats.isCompletedText.text = level.isCompleted ? "✅" : "⭕";
        Debug.Log($"Level {level.id} - Max Score set: {level.maxScore}");

        // Добавляем попытки для текущего уровня
        var attempts = databaseManager.GetAttemptsForLevel(level.id);

        Debug.Log($"Fetched {attempts.Count} attempts for Level ID: {level.id}");

        foreach (var attempt in attempts)
        {
            Debug.Log($"Processing Attempt Score: {attempt.Score} for Level ID: {level.id}");

            // Создаём элемент попытки
            GameObject attemptGO = Instantiate(attemptPrefab, levelStats.attemptsContainer);
            AttemptStatsItem attemptStats = attemptGO.GetComponent<AttemptStatsItem>();

            // Проверяем наличие компонента
            if (attemptStats == null)
            {
                Debug.LogError($"AttemptStatsItem component missing on attempt prefab for Attempt Score: {attempt.Score}");
                continue;
            }

            // Заполняем данные для попытки
            attemptStats.attemptNumText.text = $"{attempt.AttemptNumber}";
            attemptStats.attemptScore.text = $"{attempt.Score}";
            Debug.Log($"Attempt Score set: {attempt.Score} for Level ID: {level.id}");
        }
    }

    Debug.Log("PopulateStatsMenu completed.");
}
}

