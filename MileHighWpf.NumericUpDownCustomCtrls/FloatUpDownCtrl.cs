namespace MileHighWpf.NumericUpDownCustomCtrls
{
    public sealed class FloatUpDownCtrl : UpDownCtrlBase<float, FloatUpDownRange>
    {
        /// <summary>
        /// Real ctor.
        /// </summary>
        public FloatUpDownCtrl()
        {
            DefaultStyleKey = typeof(FloatUpDownCtrl);
            this.IsTabStop = true;
            Increment = 1.0f;
        }
        /// <summary>
        /// This ctor is just for unit tests.
        /// </summary>
        /// <param name="range"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public FloatUpDownCtrl(FloatUpDownRange range) : this()
        {
            if (!range.HasMin) { throw new ArgumentNullException("min"); }
            if (!range.HasMax) { throw new ArgumentNullException("max"); }
            if (!range.HasNewValue) { throw new ArgumentNullException("new value"); }
            if (!range.HasNewIncrement) { throw new ArgumentNullException("new interval"); }
            Range = range;
        }
        protected override string FormatTextBlockTooltip()
        {
            return $"Range: {_minLimit.ToString(FormatString)} .. {_maxLimit.ToString(FormatString)}";
        }
        /// <summary>
        /// Use the decimal precision of the Increment value to determine the number of significant digits to display.
        /// </summary>
        protected override void UpdateFormatFromIncrement()
        { 
            // Look like a float.
            string inc = ((float)_increment).ToString();
            SigDigits = inc.Contains('.') 
                ? inc.Length - 1 - inc.IndexOf('.') 
                : 0;
            FormatString = (SigDigits == 0)
                ? "F1"
                : "F" + SigDigits.ToString();
        }
        protected override string GetTextElementText(float value)
        { 
            return value.ToString(FormatString); 
        }
    }
}
