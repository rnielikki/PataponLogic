using PataRoad.Core.Character;
using System;

namespace PataRoad.SceneLogic.EquipmentScene
{
    internal class StatDisplayMap
    {
        private UnityEngine.UI.Text _text { get; }
        private string _format { get; }
        private Func<Stat, int> _intValueGetter { get; set; }
        private Func<Stat, float> _floatValueGetter { get; set; }
        private Func<Stat, string> _stringValueGetter { get; set; }

        private bool _isFloat = true;
        private bool _hasStringFormatGetter;
        private bool _positiveIsBetter = true;

        private UnityEngine.Color _positiveColor;
        private UnityEngine.Color _negativeColor;
        private UnityEngine.Color _neutralColor;

        internal StatDisplayMap(UnityEngine.UI.Text text, string format)
        {
            _text = text;
            _format = format;
        }
        internal StatDisplayMap SetValueGetter(Func<Stat, int> valueGetter)
        {
            _isFloat = false;
            _intValueGetter = valueGetter;
            return this;
        }
        internal StatDisplayMap SetValueGetter(Func<Stat, float> valueGetter)
        {
            _floatValueGetter = valueGetter;
            return this;
        }
        internal StatDisplayMap SetFormatGetter(Func<Stat, string> valueGetter)
        {
            _hasStringFormatGetter = true;
            _stringValueGetter = valueGetter;
            return this;
        }
        internal StatDisplayMap SetNegativeIsBetter()
        {
            _positiveIsBetter = false;
            return this;
        }

        internal void AssignColors(UnityEngine.Color positive, UnityEngine.Color neutral, UnityEngine.Color negative)
        {
            _positiveColor = positive;
            _neutralColor = neutral;
            _negativeColor = negative;
        }

        internal void UpdateText(Stat stat)
        {
            _text.text = GetString(stat);
        }
        internal void SetText(string text)
        {
            _text.text = text;
        }
        internal void ResetColor()
        {
            _text.color = _neutralColor;
        }
        internal void CompareValue(float current, float oldValue, float newValue)
        {
            _text.text = GetSafeFloatValue(current - oldValue + newValue);
            CompareAndUpdate(oldValue, newValue);
        }
        internal void CompareOneByOne(Stat currentStat, Stat oldStat, Stat newStat)
        {
            if (_isFloat)
            {
                var oldValue = _floatValueGetter(oldStat);
                var newValue = _floatValueGetter(newStat);
                var currentValue = _floatValueGetter(currentStat);
                CompareValue(currentValue, oldValue, newValue);
            }
            else
            {
                var oldValue = _intValueGetter(oldStat);
                var newValue = _intValueGetter(newStat);
                var currentValue = _intValueGetter(currentStat);
                _text.text = (currentValue - oldValue + newValue).ToString(_format);
                CompareAndUpdate(oldValue, newValue);
            }
        }
        private void CompareAndUpdate<T>(T oldValue, T newValue) where T : IComparable
        {
            //new value is bigger -> positive as default
            int comparisonResult = -(oldValue.CompareTo(newValue));

            if (comparisonResult == 0)
            {
                _text.color = _neutralColor;
            }
            else if ((_positiveIsBetter && comparisonResult > 0) || (!_positiveIsBetter && comparisonResult < 0))
            {
                _text.color = _positiveColor;
            }
            else
            {
                _text.color = _negativeColor;
            }
            if (comparisonResult != 0) _text.text += comparisonResult > 0 ? "▴" : "▾";
        }
        private string GetString(Stat stat)
        {
            if (_hasStringFormatGetter) return _stringValueGetter(stat);
            else if (_isFloat) return GetSafeFloatValue(_floatValueGetter(stat));
            else return _intValueGetter(stat).ToString(_format);
        }
        private string GetSafeFloatValue(float value)
        {
            if (value != UnityEngine.Mathf.Infinity)
            {
                return value.ToString(_format);
            }
            else return "∞";
        }
    }
}
