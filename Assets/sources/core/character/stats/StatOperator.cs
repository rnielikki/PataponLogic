using System.Collections.Generic;
/// <summary>
/// Represents RUN-TIME change pipeline of stat calculation.
/// </summary>
namespace PataRoad.Core.Character
{
    public class StatOperator
    {
        private readonly Stat _stat;
        private readonly Stat _originalStat;
        private readonly List<IStatOperation> _pipelines = new List<IStatOperation>();

        internal StatOperator(Stat realStat)
        {
            _originalStat = realStat;
            _stat = realStat.Copy();
        }

        public Stat GetFinalStat(Rhythm.Command.CommandSong song, bool charged)
        {
            Stat stat = _stat.SetValuesTo(_originalStat);
            foreach (var pipeline in _pipelines)
            {
                pipeline.Calculate(song, charged, stat);
            }
            return stat;
        }

        /// <summary>
        /// Adds pipeline for stat calculation.
        /// </summary>
        /// <param name="operation">One pipeline instruction to add.</param>
        /// <returns><c>true</c> if there was no duplication and succesfully added, otherwise <c>false</c></returns>
        public bool Add(IStatOperation operation)
        {
            if (!_pipelines.Contains(operation))
            {
                _pipelines.Add(operation);
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Removes pipeline for stat calculation.
        /// </summary>
        /// <param name="operation">One pipeline instruction to remove.</param>
        /// <returns><c>true</c> if the opration is found and removed. If the operation doesn't exist, it returns <c>false</c>.</returns>
        public bool Remove(IStatOperation operation) => _pipelines.Remove(operation);
    }
}
