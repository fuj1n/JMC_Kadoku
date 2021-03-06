﻿using BulletHack.UI;

namespace BulletHack.Scripting.Action.BlockAction
{
    [BlockLoader.Block("repeat")]
    public class RepeatTimesBlock : ActionBlockBase
    {
        [InputVar(Reverse = true)]
        private int times;

        private int currentTimes;

        public override string GetName() => "Repeat";

        public override bool ShouldExecute()
        {
            return currentTimes < times;
        }

        public override void Next()
        {
            currentTimes++;
        }

        public override void ResetState()
        {
            base.ResetState();

            currentTimes = 0;
        }
    }
}