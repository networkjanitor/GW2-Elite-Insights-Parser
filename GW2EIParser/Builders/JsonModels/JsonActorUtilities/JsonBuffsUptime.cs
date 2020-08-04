﻿using System.Collections.Generic;
using System.Linq;
using GW2EIParser.EIData;
using GW2EIParser.Parser.ParsedData;

namespace GW2EIParser.Builders.JsonModels
{
    /// <summary>
    /// Class representing buff on targets
    /// </summary>
    public class JsonBuffsUptime
    {
        /// <summary>
        /// Target buff item
        /// </summary>
        public class JsonBuffsUptimeData
        {
            /// <summary>
            /// Uptime of the buff
            /// </summary>
            public double Uptime { get; }
            /// <summary>
            /// Presence of the buff (intensity only)
            /// </summary>
            public double Presence { get; }
            /// <summary>
            /// Buff generated by
            /// </summary>
            public Dictionary<string, double> Generated { get; }
            /// <summary>
            /// Buff overstacked by
            /// </summary>
            public Dictionary<string, double> Overstacked { get; }
            /// <summary>
            /// Buff wasted by
            /// </summary>
            public Dictionary<string, double> Wasted { get; }
            /// <summary>
            /// Buff extended by unknown for
            /// </summary>
            public Dictionary<string, double> UnknownExtended { get; }
            /// <summary>
            /// Buff by extension
            /// </summary>
            public Dictionary<string, double> ByExtension { get; }
            /// <summary>
            /// Buff extended for
            /// </summary>
            public Dictionary<string, double> Extended { get; }


            private static Dictionary<string, double> ConvertKeys(Dictionary<AbstractSingleActor, double> toConvert)
            {
                var res = new Dictionary<string, double>();
                foreach (KeyValuePair<AbstractSingleActor, double> pair in toConvert)
                {
                    res[pair.Key.Character] = pair.Value;
                }
                return res;
            }

            public JsonBuffsUptimeData(FinalBuffs buffs, FinalBuffsDictionary buffsDictionary)
            {
                Uptime = buffs.Uptime;
                Presence = buffs.Presence;
                Generated = ConvertKeys(buffsDictionary.Generated);
                Overstacked = ConvertKeys(buffsDictionary.Overstacked);
                Wasted = ConvertKeys(buffsDictionary.Wasted);
                UnknownExtended = ConvertKeys(buffsDictionary.UnknownExtension);
                ByExtension = ConvertKeys(buffsDictionary.Extension);
                Extended = ConvertKeys(buffsDictionary.Extended);
            }
        }

        /// <summary>
        /// ID of the buff
        /// </summary>
        /// <seealso cref="JsonLog.BuffMap"/>
        public long Id { get; }
        /// <summary>
        /// Array of buff data \n
        /// Length == # of phases
        /// </summary>
        /// <seealso cref="JsonBuffsUptimeData"/>
        public List<JsonBuffsUptimeData> BuffData { get; }
        /// <summary>
        /// Array of int[2] that represents the number of buff \n
        /// Array[i][0] will be the time, Array[i][1] will be the number of buff present from Array[i][0] to Array[i+1][0] \n
        /// If i corresponds to the last element that means the status did not change for the remainder of the fight
        /// </summary>
        public List<int[]> States { get; }

        public JsonBuffsUptime(AbstractSingleActor actor, long buffID, ParsedLog log, RawFormatSettings settings, List<JsonBuffsUptimeData> buffData, Dictionary<string, JsonLog.BuffDesc> buffDesc)
        {
            Id = buffID;
            BuffData = buffData;
            if (!buffDesc.ContainsKey("b" + buffID))
            {
                buffDesc["b" + buffID] = new JsonLog.BuffDesc(log.Buffs.BuffsByIds[buffID], log);
            }
            if (settings.RawFormatTimelineArrays)
            {
                States = GetBuffStates(actor.GetBuffGraphs(log)[buffID]);
            }
        }


        public static List<int[]> GetBuffStates(BuffsGraphModel bgm)
        {
            if (bgm == null || bgm.BuffChart.Count == 0)
            {
                return null;
            }
            var res = bgm.BuffChart.Select(x => new int[2] { (int)x.Start, (int)x.Value }).ToList();
            return res.Count > 0 ? res : null;
        }
    }

}
