using System;
using System.Collections.Generic;
using System.Linq;

namespace Missions
{
    [Serializable]
    public class Mission
    {
        public MissionDefinition Definition;
        public int Duration;
        public int RemainingDays;
        public int RewardMoney;
        public int Difficulty;
        public Dictionary<SkillDefinition, int> SkillDifficulty;
        public Dictionary<string, string> Replacements;
        
        public Dictionary<SkillDefinition, float> Progress;
        public Mission(MissionDefinition definition)
        {
            Definition = definition;
            SkillDifficulty = new Dictionary<SkillDefinition, int>();
            Progress = new Dictionary<SkillDefinition, float>();
            Replacements = new Dictionary<string, string>();
        }

        public string GetName()
        {
            return Replacements.Aggregate(Definition.Title,
                (current, value) => current.Replace(value.Key, value.Value));
        }

        public string GetDescription()
        {
            return Replacements.Aggregate(Definition.Description,
                (current, value) => current.Replace(value.Key, value.Value));
        }
    }
}