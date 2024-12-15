using DefaultNamespace;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public GameObject  _gameManager;
    public GameObject menu;
    public TextMeshProUGUI levelNameText; // поле для отображения названия уровня
    public TextMeshProUGUI difficultyText; // поле для отображения сложности
    public TextMeshProUGUI statusText; // поле для отображения статуса (пройден или нет)

    
    private LevelData levelData; // данные об уровне
    
    private Button button;

    // Метод для инициализации кнопки
    public void Setup(LevelData level)
    {
        levelData = level;
        levelNameText.text = level.name;
        difficultyText.text = level.difficulty;
        statusText.text = level.isCompleted ? "✅" : "⭕";
        
    } 
    
    public void Start()
    {
        /*Image image = GetComponent<Image>();
        if (image != null)
        {
            image.raycastTarget = false;  // Отключаем и снова включаем для перезапуска
            image.raycastTarget = true;
        }
        
        button = GetComponent<Button>();
        Debug.Log("Попытка регистрации лиснера для: " + button);
        button.onClick.AddListener(() =>
        {
            Debug.Log("Вызов лямбды: ");
            LevelButton a = this;
            a.OnClick();
        }); */
        //Debug.Log("Успешная регистрация: " + button.onClick);
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse clicked at position: " + Input.mousePosition);
            Debug.Log("Button on: " + transform.position);
        }
    }

    // Метод для обработки клика по кнопке
    public void OnClick()
    {
        Debug.Log("Loading level: " + levelData.name);

        // Создаем новый объект GameManager из префаба
        GameObject newGameManagerObject = Instantiate(_gameManager);
        GameManager newGameManager = newGameManagerObject.GetComponent<GameManager>();

        MapLoader mapLoader = new MapLoader();
        StartCoroutine(mapLoader.LoadMapFromJson(levelData.mapPath, () =>
        {
            Debug.Log("Загружен уровень: " + mapLoader.level);
            DebugLogObject.log("OnClick Loaded: " + mapLoader.level.ToString());
            menu.SetActive(false);
            mapLoader.level.setId(levelData.id);
            // Используем новый экземпляр GameManager для запуска игры
            newGameManager.StartGame(mapLoader.level);
        }));
    }
    
    // Этот метод будет срабатывать при клике на объект с 2D коллайдером
    private void OnMouseDown()
    {
        // Логика, которая должна происходить при клике
        Debug.Log("2D Object Clicked: " + levelNameText.text);

        // Например, можно вызвать здесь свой метод, который ты бы вызывал через кнопку
        OnClick();
    }


}