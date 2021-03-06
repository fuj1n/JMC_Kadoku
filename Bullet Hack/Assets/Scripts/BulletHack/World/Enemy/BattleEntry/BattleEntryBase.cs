﻿using System;
using System.Linq;
using BulletHack.World.Messaging;
using UnityEngine;

namespace BulletHack.World.Enemy.BattleEntry
{
    public class BattleEntryBase : MonoBehaviour
    {
        public static Action onBattleFinish;

        public BattleEntryPoint EntryPoint { get; private set; }

        private void Awake()
        {
            EntryPoint = GetComponentInParent<BattleEntryPoint>();
        }

        public virtual void OnEntry()
        {
            ICombatEntryEvent[] receivers = GetComponentsInParent<ICombatEntryEvent>();

            bool cancel = receivers.Aggregate(false, (current, receiver) => current || receiver.OnPreCombatEnter());
            if (cancel)
                return;
            
            CombatManager.properties = GetComponentInParent<CombatProperties>();
            
            onBattleFinish = OnBattleFinished;
            EntryPoint.EnterBattle();
        }

        private void OnBattleFinished()
        {
            if (!GameData.Instance.isDead)
            {
                foreach(IEnemyDefeatedHandler handler in GetComponentsInParent<IEnemyDefeatedHandler>(true))
                    handler.OnEnemyDefeated();
                Destroy(EntryPoint.gameObject);
            }
        }

        public interface IEnemyDefeatedHandler
        {
            void OnEnemyDefeated();
        }
    }
}