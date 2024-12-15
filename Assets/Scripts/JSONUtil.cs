using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace
{
    
    public class JSONLoader
    {
        public string jsonString = "";
        
        
        
        public IEnumerator LoadJSON(string jsonPath, System.Action callback)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, jsonPath);
            DebugLogObject.log("LoadJSON Trying to get: " + filePath);
            

            // Для Android использовать UnityWebRequest
#if UNITY_ANDROID
    UnityWebRequest www = UnityWebRequest.Get(filePath);
    yield return www.SendWebRequest();

    if (www.result == UnityWebRequest.Result.Success)
    {
        DebugLogObject.log("JSON файл загружен: ");
        jsonString = www.downloadHandler.text;
    }
    else
    {
        Debug.LogError("Ошибка загрузки JSON файла: " + www.error);
        DebugLogObject.log("Ошибка загрузки JSON файла: " + www.error);
        yield break;  // Здесь выходим из метода, если ошибка
    }
#else
            // Для остальных платформ
            if (File.Exists(filePath))
            {
                jsonString = File.ReadAllText(filePath);
            }
            else
            {
                Debug.LogError("JSON файл не найден: " + filePath);
                yield break;  // Здесь тоже выходим из метода, если файл не найден
            }
#endif

            callback();

            yield break;  // Добавляем этот вызов, чтобы гарантировать выход из метода
        }
    }
    
}