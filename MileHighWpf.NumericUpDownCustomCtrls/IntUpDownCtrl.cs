namespace MileHighWpf.NumericUpDownCustomCtrls
{
    public sealed class IntUpDownCtrl : UpDownCtrlBase<int, IntUpDownRange>
    {
        /// <summary>
        /// Real ctor.
        /// </summary>
        public IntUpDownCtrl()
        {
            DefaultStyleKey = typeof(IntUpDownCtrl);
            this.IsTabStop = true;
            Increment = 1;
        }
        /// <summary>
        /// This ctor is just for unit tests.
        /// </summary>
        /// <param name="range"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IntUpDownCtrl(IntUpDownRange range) : this()
        {
            if (!range.HasMin) { throw new ArgumentNullException("min"); }
            if (!range.HasMax) { throw new ArgumentNullException("max"); }
            if (!range.HasNewValue) { throw new ArgumentNullException("new value"); }
            if (!range.HasNewIncrement) { throw new ArgumentNullException("new interval"); }
            Range = range;
        }
        protected override string FormatTextBlockTooltip()
        {
            return $"Range: {_minLimit} .. {_maxLimit}";
        }
   }
}