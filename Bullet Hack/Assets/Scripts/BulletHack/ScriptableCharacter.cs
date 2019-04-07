using System;
using BulletHack.Scripting.Entity.Ticking;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace BulletHack
{
    public class ScriptableCharacter : MonoBehaviour
    {
        public int X
        {
            get => pos.x;
            set
            {
                value = Mathf.Clamp(value, 0, gridSize.x - 1);
                int diff = value - pos.x;
                pos.x = value;

                Move(Vector3.right * diff * coordOffset);
            }
        }

        public int Y
        {
            get => pos.y;
            set
            {
                value = Mathf.Clamp(value, 0, gridSize.y - 1);
                int diff = pos.y - value;
                pos.y = value;

                Move(Vector3.forward * diff * coordOffset);
            }
        }

        public int Health
        {
            get => usePlayerHealth ? GameData.Instance.playerHealth : health;
            set
            {
                ref int health = ref this.health;

                if (usePlayerHealth)
                    health = ref GameData.Instance.playerHealth;

                int maxHealth = usePlayerHealth ? GameData.Instance.playerMaxHealth : this.maxHealth;

                if (!powerups.ShieldActive)
                {
                    health = Mathf.Clamp(value, 0, maxHealth);
                    Shake();
                }

                powerups.ShieldActive = false;

                if (health > 0) return;

                transform.DOKill();
                Destroy(gameObject);

                if (deathParticles)
                    Instantiate(deathParticles).transform.position = transform.position;

                if (deathSound)
                    SoundManager.PlayClip(deathSound, SoundManager.Channel.SoundEffect);
            }
        }

        public ref int MaxHealth => ref usePlayerHealth ? ref GameData.Instance.playerMaxHealth : ref maxHealth;

        public bool usePlayerHealth;

        [SerializeField]
        [ConditionalHide("usePlayerHealth", true, true)]
        private int health = 3;
        [SerializeField]
        [ConditionalHide("usePlayerHealth", true, true)]
        private int maxHealth = 3;

        public float rotateIntensity = 2F;

        [ColorUsage(false)]
        public Color gizmo;

        public float coordOffset = 1F;

        [HideInInspector]
        public float tweenSpeed;

        public Vector2Int gridSize = new Vector2Int(3, 3);
        [SerializeField]
        private Vector2Int pos = new Vector2Int(1, 1);

        [Header("Audio")]
        public AudioClip shootSound;
        public AudioClip deathSound;

        [Header("Templates")]
        public GameObject bullet;
        public GameObject deathParticles;

        [Space]
        public PowerupState powerups;

        private Quaternion neutral;

        private void Awake()
        {
            neutral = transform.rotation;
        }

        private void Move(Vector3 val)
        {
            if (Math.Abs(val.sqrMagnitude) < .025F)
            {
                Shake();
                return;
            }

            Vector3 direction = val / val.magnitude;
            direction.y = 0F;

            Sequence move = DOTween.Sequence();

            move.Append(transform.DOBlendableLocalRotateBy(-direction * rotateIntensity, tweenSpeed * .2F));
            move.AppendInterval(tweenSpeed * .6F);
            move.Append(transform.DORotateQuaternion(neutral, tweenSpeed * .2F));

            transform.DOMove(transform.position + val, tweenSpeed).SetEase(Ease.InOutSine);

            move.Play();
        }

        public void Shake()
        {
            transform.DOShakeRotation(tweenSpeed / 2F, rotateIntensity);
        }

        public void Shoot(int offset = 0)
        {
            GameObject b = Instantiate(bullet);
            b.transform.position = transform.position + Vector3.right * offset * coordOffset;
            b.transform.forward = transform.forward;

            Bullet bObj = b.GetComponent<Bullet>();
            if (bObj && CombatManager.Instance.Script.OtherAvatar)
                bObj.target = CombatManager.Instance.Script.OtherAvatar.transform;

            if (shootSound)
                SoundManager.PlayClip(shootSound, SoundManager.Channel.SoundEffect);
        }

        private void Update()
        {
            powerups.Update(tweenSpeed);
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
                return;

            Vector3 startPos = new Vector3(transform.position.x - coordOffset * pos.x, transform.position.y, transform.position.z - coordOffset * pos.y);

            gizmo.a = 1;
            Gizmos.color = gizmo;

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int z = 0; z < gridSize.y; z++)
                {
                    Gizmos.DrawWireCube(startPos + Vector3.right * x * coordOffset + Vector3.forward * z * coordOffset, Vector3.one);
                }
            }
        }

        [Serializable]
        public struct PowerupState
        {
            public int health, shield, spread;

            public bool ShieldActive
            {
                get => shieldActive;
                set
                {
                    if (shieldActive == value)
                        return;

                    shieldActive = value;

                    if (shieldOverlay)
                        shieldOverlay.material.DOFade(shieldActive ? shieldOpacity : 0F, tweenSpeed);
                }
            }
            [SerializeField]
            private bool shieldActive;

            [Header("Display")]
            public TextMeshProUGUI healthText;
            public TextMeshProUGUI shieldText;
            public TextMeshProUGUI spreadText;
            public Renderer shieldOverlay;
            public float shieldOpacity;

            private float tweenSpeed;

            public void Update(float tweenSpeed)
            {
                if (healthText)
                    healthText.text = health.ToString();
                if (shieldText)
                    shieldText.text = shield.ToString();
                if (spreadText)
                    spreadText.text = spread.ToString();

                this.tweenSpeed = tweenSpeed;
            }
        }
    }
}