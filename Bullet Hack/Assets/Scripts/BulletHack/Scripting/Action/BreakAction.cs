﻿using BulletHack.Scripting.Action.BlockAction;
using BulletHack.UI;
using BulletHack.UI.BlockManager;

namespace BulletHack.Scripting.Action
{
    [BlockLoader.Block]
    public class BreakAction : ActionBase
    {
        public override void Execute()
        {
            BlockManagerBase manager = this.manager;

            while (manager && manager is BlockManager)
            {
                if (manager is BracketBlockManager)
                {
                    if ((bool) manager.GetComponent<ActionBlockBase>()?.Break())
                        break;
                }

                manager = ((BlockManager) manager).inConnector;
            }
        }

        public override string GetName() => "Escape";
    }
}