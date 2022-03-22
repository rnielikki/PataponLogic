using UnityEngine;

namespace PataRoad.Core.Map.Levels
{
    class Level0 : MonoBehaviour
    {
        private void Start()
        {
            var dropBehaviour = GetComponentInParent<Items.ItemDropBehaviour>();
            //Prevents retiring
            Global.GlobalData.Input.actions.FindAction("UI/Start").Disable();
            Character.Patapons.PataponsManager.AllowedToGoForward = false;
            (dropBehaviour.DropData[0] as Items.SongItemDropData).OnSongComplete
                .AddListener(() => Character.Patapons.PataponsManager.AllowedToGoForward = true);

            dropBehaviour.Drop();
        }
    }
}
