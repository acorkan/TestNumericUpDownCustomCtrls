using System.Numerics;

namespace NumericUpDownCustomCtrls
{
    /// <summary>
    /// This class updates the Min and Max range for the integer Up/Down control.
    /// Values can only be set by the ctor.
    /// Either Min or Max can be null indicating there should not be a range chck on either (or both) limits.
    /// An optional ValueUpdate can be specified to update the value at the same time, but is ignored if null.
    /// All limits and optional value are tested for valid settings in the ctor.
    /// </summary>
    public abstract class UpDownRangeBase<T> where T : struct, IComparable<T>, IMinMaxValue<T>
    {
        protected T? MinLimit { get; private set; }
        protected T? MaxLimit { get; private set; }
        protected T? NewValue { get; private set; }
        protected T? NewIncrement { get; private set; }

        protected UpDownRangeBase(T? minLimit, T? maxLimit, T? newValue = default, T? newIncrement = default)
        {
            MinLimit = minLimit;
            MaxLimit = maxLimit;
            NewValue = newValue;
            NewIncrement = newIncrement;
            if (MaxLimit.HasValue)
            {
                if (MinLimit.HasValue && (MinLimit.Value.CompareTo(MaxLimit.Value) > 0))  // Use CompareTo for generic comparison
                {
                    throw new ArgumentOutOfRangeException($"min amd max");
                }
                if (NewValue.HasValue && (NewValue.Value.CompareTo(MaxLimit.Value) > 0))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
            }
            if (MinLimit.HasValue)
            {
                if (NewValue.HasValue && (NewValue.Value.CompareTo(MinLimit.Value) < 0))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
            }
            if (NewIncrement.HasValue)
            {
                double min = MinLimit.HasValue ? Convert.ToDouble(MinLimit.Value) : Convert.ToDouble(T.MinValue);
                double max = MaxLimit.HasValue ? Convert.ToDouble(MaxLimit.Value) : Convert.ToDouble(T.MaxValue);
                if ((max - min) < 2 * Convert.ToDouble(NewIncrement))
                {
                    throw new ArgumentOutOfRangeException("newInterval");
                }
            }
        }

        public T GetMin()
        {
            return HasMin ? MinLimit.Value : T.MinValue;
        }
        public T GetMax()
        {
            return HasMax ? MaxLimit.Value : T.MaxValue;
        }
        public T GetNewValue()
        {
            if (HasNewValue)
            {
                return NewValue.Value;
            }
            throw new InvalidOperationException("No value set");
        }
        public T GetNewIncrement()
        {
            if (HasNewIncrement)
            {
                return NewIncrement.Value;
            }
            throw new InvalidOperationException("No interval set");
        }

        public bool HasMin
        {
            get => MinLimit.HasValue;
        }
        public bool HasMax
        {
            get => MaxLimit.HasValue;
        }
        public bool HasNewValue
        {
            get => NewValue.HasValue;
        }
        public bool HasNewIncrement
        {
            get => NewIncrement.HasValue;
        }
    }
}
