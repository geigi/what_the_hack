using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DefaultNamespace;
using Team;
using UE.Common;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utils;

namespace Wth.ModApi.Employees
{
    /// <summary>
    /// This class is a data container class for an employee. Do not implement any logic in here.
    /// All data that needs to be persistent should be in this class to be serialized.
    /// </summary>
    [Serializable]
    public class EmployeeData
    {
        public const int LEVELUP_THRESHOLD = 2;
        public const float LEVELUP_THRESHOLD_INCREASE_FACTOR = 1.2f;

        /// <summary>
        /// The full ame of the employee.
        /// </summary>
        public string Name
        {
            get
            {
                if (generatedData != null)
                {
                    return generatedData.name;
                }
                else
                {
                    return EmployeeDefinition.EmployeeName;
                }
            }
        }
        
        /// <summary>
        /// Level of the employee > 0.
        /// </summary>
        public int Level => level;
        private int level = 1;
        
        /// <summary>
        /// Salary of the employee.
        /// </summary>
        public int Salary = 100;

        /// <summary>
        /// Prize for buying the employee.
        /// </summary>
        public int Prize = 100;
        
        /// <summary>
        /// List of learned skills.
        /// </summary>
        public List<Skill> Skills;

        public int SkillPoints
        {
            get
            {
                return skillPoints;
            }
            set
            {
                skillPoints = value;
                if (SkillPointsChanged == null) SkillPointsChanged = new IntUnityEvent();
                SkillPointsChanged.Invoke(value);
            }
        }

        private int skillPoints = 0;

        /// <summary>
        /// Gets fired when skill points change.
        /// </summary>
        public UnityEvent<int> SkillPointsChanged
        {
            get
            {
                if (skillPointsChanged == null) skillPointsChanged = new IntUnityEvent();
                return skillPointsChanged;
            }
            set =>  skillPointsChanged = value;
        }

        [NonSerialized] 
        private UnityEvent<int> skillPointsChanged;
        
        /// <summary>
        /// Gets fired when skill points change.
        /// </summary>
        public UnityEvent<int> LevelChanged
        {
            get
            {
                if (levelChanged == null) levelChanged = new IntUnityEvent();
                return levelChanged;
            }
            set =>  levelChanged = value;
        }

        [NonSerialized] 
        private UnityEvent<int> levelChanged;
        
        /// <summary>
        /// List of specials.
        /// </summary>
        private List<EmployeeSpecial> Specials;
        
        /// <summary>
        /// Gets fired when specials change.
        /// </summary>
        public UnityEvent<EmployeeSpecial> SpecialsChanged
        {
            get
            {
                if (specialsChanged == null) specialsChanged = new EmployeeSpecialEvent();
                return specialsChanged;
            }
            set =>  specialsChanged = value;
        }

        [NonSerialized] 
        private UnityEvent<EmployeeSpecial> specialsChanged;
        
        /// <summary>
        /// World position of the employee.
        /// </summary>
        public Vector2Int Position;
        
        /// <summary>
        /// This is null when the employee is a normal generated employee.
        /// Only populated for special created Employees.
        /// </summary>
        public EmployeeDefinition EmployeeDefinition;

        /// <summary>
        /// This is null when the Employee is a special emplyoee.
        /// Only Populated for genderated employees.
        /// </summary>
        public EmployeeGeneratedData generatedData;

        /// <summary>
        /// This enum represents the current state of the employee.
        /// </summary>
        public Enums.EmployeeState State;

        /// <summary>
        /// Represents how many days the Employee wil continue to stay in the "for hire" list until he is removed.
        /// </summary>
        public int hireableDays;

        private float freeScore = 0;
        /// <summary>
        /// Score of level points that have not been spend yet.
        /// </summary>
        public float FreeScore => freeScore;

        private float usedScore = 0;
        /// <summary>
        /// Score of level points that have been spend already.
        /// </summary>
        public float UsedScore => usedScore;

        public float LevelUpScoreNeeded => (float) (Math.Pow(LEVELUP_THRESHOLD_INCREASE_FACTOR, Level) + LEVELUP_THRESHOLD);

        /// <summary>
        /// The critical failure bonus of this employee.
        /// Only EmployeeSpecials can modify this value.
        /// </summary>
        public int CriticalFailureChance
        {
            get
            {
                int result = 0;

                foreach (var special in Specials)
                {
                    result += special.GetCriticalFailureChance();
                }
            
                return 1 + result;
            }
        }
    
        /// <summary>
        /// The critical success bonus of this employee.
        /// Only EmployeeSpecials can modify this value.
        /// </summary>
        public int CriticalSuccessChance
        {
            get
            {
                int result = 0;

                foreach (var special in Specials)
                {
                    result += special.GetCriticalSuccessChance();
                }
            
                return 1 + result;
            }
        }
        
        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public EmployeeData()
        {
            SkillPointsChanged = new IntUnityEvent();
            Specials = new List<EmployeeSpecial>();
        }
        
        /// <summary>
        /// Tests whether an employee has a specific skill.
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public bool HasSkill(SkillDefinition skill)
        {
            return Skills.Any(s => s.SkillData == skill);
        }

        /// <summary>
        /// Get a specific Skill object.
        /// </summary>
        /// <param name="skill">The skill definition</param>
        /// <returns></returns>
        public Skill GetSkill(SkillDefinition skill)
        {
            return Skills.First(s => s.SkillData == skill);
        }

        public Skill GetGeneralPurpose()
        {
            return Skills.First(s => s.SkillData == ContentHub.Instance.GeneralPurposeSkill);
        }

        /// <summary>
        /// Increments the free score.
        /// All special multipliers will be combined by addition + 1f, then multiplied with the value.
        /// </summary>
        public void IncrementFreeScore(float value)
        {
            if (value > 0)
            {
                float multiplier = 1f;
                foreach (var special in Specials)
                {
                    multiplier += special.GetLearningMultiplier();
                }

                freeScore += value * multiplier;
            }
        }

        /// <summary>
        /// Spends points from the free score.
        /// </summary>
        /// <param name="value"></param>
        public void UseScore(float value)
        {
            freeScore -= value;
            usedScore += value;
        }

        /// <summary>
        /// Perform a level up of this employee.
        /// </summary>
        public void LevelUp()
        {
            if (FreeScore < LevelUpScoreNeeded) return;
            
            TeamManager.Instance.ReportLevelUp(this);
            
            SkillPoints += 1;
            UseScore(LevelUpScoreNeeded);
            level += 1;
            
            LevelChanged.Invoke(level);
        }

        /// <summary>
        /// Add a new special to this employee.
        /// </summary>
        /// <param name="special"></param>
        public void AddSpecial(EmployeeSpecial special)
        {
            Specials.Add(special);
            SpecialsChanged.Invoke(special);
        }

        /// <summary>
        /// Get the specials of this employee.
        /// </summary>
        public ReadOnlyCollection<EmployeeSpecial> GetSpecials()
        {
            return Specials.AsReadOnly();
        }
    }
}