using UnityEngine;

namespace BulletHack.World
{
    public class BattleEntryPoint : MonoBehaviour
    {
        public GameObject battle;
        
        public void EnterBattle()
        {
            WorldController.Disable();

            Instantiate(battle);
        }
    }
}
