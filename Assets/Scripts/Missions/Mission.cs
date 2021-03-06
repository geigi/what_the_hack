using System;
using System.Collections.Generic;
using System.Linq;
using GameTime;
using UnityEngine.Events;

namespace Missions
{
    [Serializable]
    public class MissionProgressEvent: UnityEvent<KeyValuePair<SkillDefinition, float>> {}
    
    public class MissionFinishedEvent : UnityEvent<Mission> {}
    
    public class MissionHookSpawnEvent : UnityEvent<MissionHook> {}
    
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
        public float Difficulty;

        public float AverageSkillDifficulty
        {
            get
            {
                float result = 0f;
                foreach (var s in SkillDifficulty.Values)
                {
                    result += s;
                }

                return result / SkillDifficulty.Count;
            }
        }
        /// <summary>
        /// Calculated difficulties for the skills
        /// </summary>
        public Dictionary<SkillDefinition, int> SkillDifficulty;
        /// <summary>
        /// Placeholder replacements
        /// </summary>
        public Dictionary<string, string> Replacements;
        
        /// <summary>
        /// This dictionary is responsible for the mission progress.
        /// A skill part of a mission is at 0f when it's freshly started and 1f when its finished.
        /// </summary>
        public Dictionary<SkillDefinition, float> Progress;

        /// <summary>
        /// This list contains a entry for every hook of this mission.
        /// The bool indicates, whether the interaction was handled or not.
        /// The list is sorted by appearance time.
        /// </summary>
        public Dictionary<MissionHook, bool> HookStatus;

        /// <summary>
        /// This boolean will be set to true when work has started on this mission.
        /// </summary>
        public bool WorkStarted = false;

        /// <summary>
        /// The date when this mission was accepted.
        /// </summary>
        public GameTimeData AcceptDate;

        /// <summary>
        /// Is this mission paused?
        /// This happens only when a interaction is pending.
        /// </summary>
        public bool Paused;
        
        /// <summary>
        /// This property is necessary because no constructor is being called on deserialization.
        /// </summary>
        [NonSerialized]
        private MissionProgressEvent progressChanged;
        
        /// <summary>
        /// This event gets fired when the progress of the mission changed.
        /// </summary>
        public MissionProgressEvent ProgressChanged
        {
            get
            {
                if (progressChanged == null)
                {
                    progressChanged = new MissionProgressEvent();
                }

                return progressChanged;
            }
            private set => progressChanged = value;
        }

        /// <summary>
        /// This property is necessary because no constructor is being called on deserialization.
        /// </summary>
        [NonSerialized]
        private MissionFinishedEvent finished;
        /// <summary>
        /// This event gets fired when the mission has finished (successful or not).
        /// </summary>
        public MissionFinishedEvent Finished
        {
            get
            {
                if (finished == null)
                {
                    finished = new MissionFinishedEvent();
                }

                return finished;
            }
            private set => finished = value;
        }
        
        /// <summary>
        /// This property is necessary because no constructor is being called on deserialization.
        /// </summary>
        [NonSerialized]
        private MissionHookSpawnEvent missionHookSpawn;
        /// <summary>
        /// This event gets fired when a mission hook is spawned.
        /// </summary>
        public MissionHookSpawnEvent MissionHookSpawn
        {
            get
            {
                if (missionHookSpawn == null)
                {
                    missionHookSpawn = new MissionHookSpawnEvent();
                }

                return missionHookSpawn;
            }
            private set => missionHookSpawn = value;
        }
        
        /// <summary>
        /// This property is necessary because no constructor is being called on deserialization.
        /// </summary>
        [NonSerialized]
        private MissionHook.MissionHookCompletedEvent missionHookCompleted;
        /// <summary>
        /// This event gets fired when a mission hook completes.
        /// </summary>
        public MissionHook.MissionHookCompletedEvent MissionHookCompleted
        {
            get
            {
                if (missionHookCompleted == null)
                {
                    missionHookCompleted = new MissionHook.MissionHookCompletedEvent();
                }

                return missionHookCompleted;
            }
            private set => missionHookCompleted = value;
        }

        public Mission()
        {
            ProgressChanged = new MissionProgressEvent();
            Finished = new MissionFinishedEvent();
            MissionHookSpawn = new MissionHookSpawnEvent();
            MissionHookCompleted = new MissionHook.MissionHookCompletedEvent();
        }
        
        public Mission(MissionDefinition definition)
        {
            Definition = definition;
            SkillDifficulty = new Dictionary<SkillDefinition, int>();
            Progress = new Dictionary<SkillDefinition, float>();
            definition.SkillsRequired.ForEach(s => Progress.Add(s, 0f));
            HookStatus = new Dictionary<MissionHook, bool>();
            definition.MissionHooks.MissionHooks.ForEach(h => HookStatus.Add(h, false));
            Replacements = new Dictionary<string, string>();
            ProgressChanged = new MissionProgressEvent();
            Finished = new MissionFinishedEvent();
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
        
        /// <summary>
        /// Returns the success text of the mission with replaced placeholders.
        /// </summary>
        /// <returns></returns>
        public string GetSuccessText()
        {
            return Replacements.Aggregate(Definition.MissionSucceeded,
                (current, value) => current.Replace(value.Key, value.Value));
        }
        
        /// <summary>
        /// Returns the failed text of the mission with replaced placeholders.
        /// </summary>
        /// <returns></returns>
        public string GetFailedText()
        {
            return Replacements.Aggregate(Definition.MissionFailed,
                (current, value) => current.Replace(value.Key, value.Value));
        }

        /// <summary>
        /// Finishes this mission.
        /// Notifies event listeners and removes all listeners.
        /// </summary>
        public void Finish()
        {
            Finished.Invoke(this);
        }

        public bool Completed()
        {
            return !Progress.Any(s => s.Value < 1f);
        }

        /// <summary>
        /// Remove event listeners.
        /// Call this, wenn a mission is not in progress anymore.
        /// </summary>
        public void Cleanup()
        {
            Finished.RemoveAllListeners();
            ProgressChanged.RemoveAllListeners();
        }
    }
}