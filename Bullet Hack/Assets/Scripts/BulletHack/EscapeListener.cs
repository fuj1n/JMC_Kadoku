using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletHack
{
    [AddComponentMenu("")]
    public class EscapeListener : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Scene current = SceneManager.GetActiveScene();

                if (current.buildIndex == 0)
                {
                    Application.Quit();

#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                }
                else
                {
                    Time.timeScale = 1F;
                    SceneManager.LoadScene(0);
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Init()
        {
            GameObject ob = new GameObject("Escape Listener");
            ob.AddComponent<EscapeListener>();
            DontDestroyOnLoad(ob);
        }
    }
}