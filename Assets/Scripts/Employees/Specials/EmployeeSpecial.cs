using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Wth.ModApi.Employees
{
    /// <summary>
    /// Employee specials are held by an employee and alter his skills and behavoir in different ways. Needs to be extended.
    /// </summary>
    [Serializable]
    public abstract class EmployeeSpecial : ISerializable
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public EmployeeSpecial() { }

        /// <summary>
        /// Constructor used for deserialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public EmployeeSpecial(SerializationInfo info, StreamingContext context) { }

        /// <summary>
        /// Returns the display name of this special.
        /// </summary>
        /// <returns></returns>
        public abstract String GetDisplayName();

        /// <summary>
        /// Returns the description of this special.
        /// </summary>
        /// <returns></returns>
        public abstract String GetDescription();

        /// <summary>
        /// Returns the score cost of this special.
        /// Negative values are used for negative specials; positive values for positive specials.
        /// </summary>
        /// <returns></returns>
        public abstract float GetScoreCost();

        /// <summary>
        /// Override if this special alters the hiring cost absolutely.
        /// The value will be added to the hiring cost at the end of the calculation.
        /// </summary>
        /// <returns></returns>
        public virtual int GetHiringCostAbsoluteBonus()
        {
            return 0;
        }

        /// <summary>
        /// Override if this special alters the hiring cost relatively.
        /// The value will be multiplied with the hiring cost at the end of the calculation.
        /// </summary>
        /// <returns></returns>
        public virtual float GetHiringCostRelativeFactor()
        {
            return 1;
        }

        /// <summary>
        /// Override if this special alters the salary absolutely.
        /// The value will be added to the salary at the end of the calculation.
        /// </summary>
        /// <returns></returns>
        public virtual int GetSalaryAbsoluteBonus()
        {
            return 0;
        }

        /// <summary>
        /// Override if this special alters the salary relatively.
        /// The value will be multiplied with the salary at the end of the calculation.
        /// </summary>
        /// <returns></returns>
        public virtual float GetSalaryRelativeFactor()
        {
            return 1;
        }

        /// <summary>
        /// Returns the chance for the employee to have a critical failure
        /// Critical failure is defined as a dice roll with a 20 sided dice that has a result lower than (1 + criticalFailureChance)
        /// </summary>
        /// <returns></returns>
        public virtual int GetCriticalFailureChance()
        {
            return 0;
        }

        /// <summary>
        /// Returns the chance for the employee to have a critical success
        /// Critical emoji_success is defined as a dice roll with a 20 sided dice that has a result greater than (20 - criticalSuccessChance)
        /// </summary>
        /// <returns></returns>
        public virtual int GetCriticalSuccessChance()
        {
            return 0;
        }

        /// <summary>
        /// Returns the learning multiplier for this special.
        /// Note: All special multipliers will be combined by addition + 1f and then multiplied with the score value.
        /// </summary>
        /// <returns></returns>
        public virtual float GetLearningMultiplier()
        {
            return 0.0f;
        }

        /// <summary>
        /// Get's called when a level up has occured.
        /// </summary>
        public virtual void OnLevelUp()
        {
        }

        /// <summary>
        /// Override if the special should be hidden in the UI.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsHidden()
        {
            return false;
        }

        /// <summary>
        /// Override if the special should not be learnable for employees.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsLearnable()
        {
            return true;
        }
        
        /// <summary>
        /// Override if the special has a custom action that will be executed on a time step change.
        /// </summary>
        /// <param name="employeeData"></param>
        public virtual void OnTimeStepChanged(EmployeeData employeeData) { }

        /// <summary>
        /// Override if the special has a custom action that will be executed on day change.
        /// </summary>
        /// <param name="employeeData"></param>
        public virtual void OnDayChanged(EmployeeData employeeData) { }

        /// <summary>
        /// This method fills the SerializationInfo on serialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}