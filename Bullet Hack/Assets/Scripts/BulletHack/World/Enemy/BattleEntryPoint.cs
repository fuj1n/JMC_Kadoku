using UnityEngine;

namespace BulletHack.World.Enemy
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