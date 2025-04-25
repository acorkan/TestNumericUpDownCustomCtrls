namespace MileHighWpf.NumericUpDownCustomCtrls
{
    /// <summary>
    /// This class updates the Min and Max range for the integer Up/Down control.
    /// Values can only be set by the ctor.
    /// Either Min or Max can be null indicating there should not be a range chck on either (or both) limits.
    /// An optional ValueUpdate can be specified to update the value at the same time, but is ignored if null.
    /// All limits and optional value are tested for valid settings in the ctor.
    /// </summary>
    public class IntUpDownRange : UpDownRangeBase<int>
    {
        public IntUpDownRange(int? min, int? max, int? newValue = default, int? newIncrement = default) : 
            base(min, max, newValue, newIncrement)
        {
        }
    }
}
