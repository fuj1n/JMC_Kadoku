using TMPro;
using UnityEngine;

namespace BulletHack.Scripting.Entity.Ticking
{
    public class PowerupTimer : TickingEntity
    {
        public TextMeshPro timeDisplay;
        public GameObject template;

        private int appearTime;

        private void Start()
        {
            ScriptController controller = CombatManager.Instance.Script;

            appearTime = Random.Range(2, controller.maxTurns - 3);
            UpdateDisplay(appearTime - 1);
        }

        public override void Tick()
        {
            UpdateDisplay(--appearTime);

            if (appearTime <= 0F)
            {
                Destroy(gameObject);
                if (template)
                    Instantiate(template, transform.parent).transform.SetPositionAndRotation(transform.position, transform.rotation);
            }
        }

        private void UpdateDisplay(int turnsRemain)
        {
            if (timeDisplay)
                timeDisplay.text = turnsRemain.ToString();
        }

        public override bool BoundsAware() => false;
    }
}