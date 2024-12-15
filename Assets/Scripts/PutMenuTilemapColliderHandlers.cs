using UnityEngine;

namespace DefaultNamespace
{
    public class PutMenuTilemapColliderHandlers : MonoBehaviour
    {
        public PutMenu putMenu;
        public void OnMouseDown()
        {
            Debug.Log("Сработка коллайдера TilemapColliderHandlers");
            putMenu.PutMenuTilemapEvent();
            
        }
    }
}