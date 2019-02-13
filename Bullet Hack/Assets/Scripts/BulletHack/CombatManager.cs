using BulletHack.Scripting;
using UnityEngine;

namespace BulletHack
{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance { get; private set; }

        public ScriptController Script { get; private set; }

        private void Awake()
        {
            Instance = this;

            Script = GetComponent<ScriptController>();
            Debug.Assert(Script, "Script controller is not present on the game manager " + gameObject.name);
        }
    }
}