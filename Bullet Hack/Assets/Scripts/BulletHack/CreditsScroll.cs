using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletHack
{
    public class CreditsScroll : MonoBehaviour
    {
        public float speed;

        private TextMeshProUGUI tmp;

        private void Awake()
        {
            tmp = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Update()
        {
            RectTransform rect = transform as RectTransform;

            if (!rect)
                return;

            rect.anchoredPosition += Vector2.up * speed * Time.deltaTime;

            if (rect.anchoredPosition.y > tmp.preferredHeight + 900F)
                SceneManager.LoadScene("0MainMenu");
        }
    }
}
