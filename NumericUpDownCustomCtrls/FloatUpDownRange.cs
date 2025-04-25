namespace NumericUpDownCustomCtrls
{
    /// <summary>
    /// This class updates the Min and Max range for the float Up/Down control.
    /// Values can only be set by the ctor.
    /// Either Min or Max can be null indicating there should not be a range chck on either (or both) limits.
    /// An optional ValueUpdate can be specified to update the value at the same time, but is ignored if null.
    /// All limits and optional value are tested for valid settings in the ctor.
    /// </summary>
    public class FloatUpDownRange : UpDownRangeBase<float>
    {
        public FloatUpDownRange(float? min, float? max, float? newValue = null, float? newIncrement = null) : 
            base(min, max, newValue, newIncrement)
        {
        }
    }
}
