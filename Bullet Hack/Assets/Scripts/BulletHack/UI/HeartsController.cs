using System;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHack.UI
{
    public class HeartsController : MonoBehaviour
    {
        public ScriptableCharacter character;
    
        private const int SPRITES_LENGTH = 2;

        private RectTransform transform2D;

        public float spriteSize = 32F;
        public Sprite[] sprites = { };
        public bool reverse;
        public Color colorMultiplier = Color.white;
    
        private Image[] hearts;

        private int cachedHealth;

        private void Start()
        {
            transform2D = GetComponent<RectTransform>();
        
            SetupHearts();
        }

        private void Update()
        {
            if (character.MaxHealth != hearts.Length)
                SetupHearts();

            int health = character.Health;
            if (health == cachedHealth)
                return;
            cachedHealth = health;

            int partialHealth = health;

            foreach (Image t in hearts)
            {
                if (partialHealth >= 1)
                {
                    partialHealth -= 1;
                    t.sprite = sprites[1];
                }
                else
                {
                    t.sprite = sprites[0];
                }
            }
        }

        private void SetupHearts()
        {
            if (hearts != null)
                foreach (Image heart in hearts)
                    Destroy(heart.gameObject);

            cachedHealth = int.MinValue;

            transform2D.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, spriteSize);
            transform2D.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, spriteSize * character.MaxHealth);

            hearts = new Image[character.MaxHealth];

            for (int i = 0; i < hearts.Length; i++)
            {
                GameObject go = new GameObject("Heart " + i);

                RectTransform rect = go.AddComponent<RectTransform>();
                rect.SetParent(transform2D, false);
                rect.sizeDelta = new Vector2(spriteSize, spriteSize);

                hearts[i] = go.AddComponent<Image>();
                hearts[i].sprite = sprites[1];
                if (reverse)
                    hearts[i].transform.localScale = new Vector3(-1F, 1F, 1F);
                hearts[i].color = colorMultiplier;
            }

            if (reverse)
                Array.Reverse(hearts);
        }

        private void OnValidate()
        {
            // Ensure sprite array is x elements long during edit-time
            if (sprites.Length != SPRITES_LENGTH)
                Array.Resize(ref sprites, SPRITES_LENGTH);
        }
    }
}
