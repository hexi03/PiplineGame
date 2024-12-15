using System;
using UnityEngine;

namespace DefaultNamespace
{
    
    public class TilemapColliderHandlers : MonoBehaviour
    {
        public GridController gridController;
        public void OnMouseDown()
        {
            Debug.Log("Сработка коллайдера TilemapColliderHandlers");
            gridController.TileMapEvent();
            
        }
    }
}