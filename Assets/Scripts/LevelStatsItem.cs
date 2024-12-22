using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelStatsItem : MonoBehaviour
{
    public TextMeshProUGUI levelNameText; // Ссылка на компонент Text
    public TextMeshProUGUI difficultyText; // Ссылка на компонент Text
    public TextMeshProUGUI maxScoreText; // Ссылка на компонент Text
    public TextMeshProUGUI isCompletedText; // Ссылка на компонент Text
    public Transform attemptsContainer; // Контейнер для попыток
}