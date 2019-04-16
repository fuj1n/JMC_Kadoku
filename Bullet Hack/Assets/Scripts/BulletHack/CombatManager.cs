using BulletHack.Scripting;
using BulletHack.World;
using BulletHack.World.Enemy.BattleEntry;
using UnityEngine;

namespace BulletHack
{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance { get; private set; }
        public static CombatProperties properties;

        public ScriptController Script { get; private set; }

        public GameObject combatWorld;
        public Animator combatAnimator;

        private void Awake()
        {
            Instance = this;

            Script = GetComponent<ScriptController>();
            Debug.Assert(Script, "Script controller is not present on the game manager " + gameObject.name);
        }

        public void OnCombatFinish()
        {
            Destroy(combatWorld);
            WorldController.Enable();

            if(BattleEntryBase.onBattleFinish != null)
                BattleEntryBase.onBattleFinish();
        }
    }
}