using System;
using System.Collections.Generic;
using System.Linq;
using UE.Events;
using UnityEngine.Events;

namespace Missions
{
    [Serializable]
    public class MissionProgressEvent: UnityEvent<KeyValuePair<SkillDefinition, float>> {}
    
    /// <summary>
    /// This class represents a finalized mission object.
    /// It differs from the <see cref="MissionDefinition"/> by including calculated Duration, RewardMoney
    /// aswell as the remaining days, the progress and placeholder replacements.
    /// This object will be serialized when saving the game.
    /// </summary>
    [Serializable]
    public class Mission
    {
        /// <summary>
        /// The mission definition
        /// </summary>
        public MissionDefinition Definition;
        /// <summary>
        /// Calculated duration for this mission.
        /// </summary>
        public int Duration;
        public int RemainingDays => RemainingTicks / GameTime.GameTime.Instance.ClockSteps;
        public int RemainingTicks;
        public int TotalTicks => Duration * GameTime.GameTime.Instance.ClockSteps;
        /// <summary>
        /// Calculated reward money
        /// </summary>
        public int RewardMoney;
        /// <summary>
        /// Calculated difficulty
        /// </summary>
        public int Difficulty;
        /// <summary>
        /// Calculated difficulties for the skills
        /// </summary>
        public Dictionary<SkillDefinition, int> SkillDifficulty;
        /// <summary>
        /// Placeholder replacements
        /// </summary>
        public Dictionary<string, string> Replacements;
        
        public Dictionary<SkillDefinition, float> Progress;

        [NonSerialized]
        public MissionProgressEvent ProgressChanged;
        
        public Mission(MissionDefinition definition)
        {
            Definition = definition;
            SkillDifficulty = new Dictionary<SkillDefinition, int>();
            Progress = new Dictionary<SkillDefinition, float>();
            definition.SkillsRequired.ForEach(s => Progress.Add(s, 0f));
            Replacements = new Dictionary<string, string>();
            ProgressChanged = new MissionProgressEvent();
        }

        /// <summary>
        /// Returns the name of the mission with replaced placeholders.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return Replacements.Aggregate(Definition.Title,
                (current, value) => current.Replace(value.Key, value.Value));
        }

        /// <summary>
        /// Returns the description of the mission with replaced placeholders.
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return Replacements.Aggregate(Definition.Description,
                (current, value) => current.Replace(value.Key, value.Value));
        }
    }
}