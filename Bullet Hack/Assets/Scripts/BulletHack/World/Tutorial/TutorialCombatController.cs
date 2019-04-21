using System.Linq;
using UnityEngine;

namespace BulletHack.World.Tutorial
{
    public class TutorialCombatController : MonoBehaviour
    {
        public Transform popupsRoot;
        
        private GameObject[] tutorials;
        private int currentTutorial = -1;
        
        private void Awake()
        {
            tutorials = popupsRoot.Cast<Transform>().Select(x => x.gameObject).ToArray();
            CycleNext();
        }

        public void CycleNext()
        {
            currentTutorial++;
            for (int i = 0; i < tutorials.Length; i++)
            {
                if(!tutorials[i])
                    continue;
                tutorials[i].SetActive(i == currentTutorial);
            }
        }
    }
}
