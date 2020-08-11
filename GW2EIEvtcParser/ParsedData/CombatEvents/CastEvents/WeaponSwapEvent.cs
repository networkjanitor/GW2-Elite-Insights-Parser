﻿namespace GW2EIEvtcParser.ParsedData
{
    public class WeaponSwapEvent : AbstractCastEvent
    {
        // Swaps
        public int SwappedTo { get; protected set; }

        internal WeaponSwapEvent(CombatItem evtcItem, AgentData agentData, SkillData skillData) : base(evtcItem, agentData, skillData)
        {
            SwappedTo = (int)evtcItem.DstAgent;
            Skill = skillData.Get(SkillItem.WeaponSwapId);
            ExpectedDuration = 50;
            ActualDuration = 50;
        }
    }
}