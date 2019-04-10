﻿using System;
using UnityEngine;

namespace BulletHack.World.Enemy.BattleEntry
{
    public abstract class BattleEntryBase : MonoBehaviour
    {
        public static Action onBattleFinish;

        public BattleEntryPoint EntryPoint { get; private set; }

        private void Awake()
        {
            EntryPoint = GetComponentInParent<BattleEntryPoint>();
        }

        protected virtual void OnEntry()
        {
            onBattleFinish = OnBattleFinished;
            EntryPoint.EnterBattle();
        }

        private void OnBattleFinished()
        {
            if (GameData.Instance.playerHealth > 0)
                SendMessageUpwards("OnEnemyDefeated", SendMessageOptions.DontRequireReceiver);
            
            Destroy(EntryPoint.gameObject);
        }
    }
}