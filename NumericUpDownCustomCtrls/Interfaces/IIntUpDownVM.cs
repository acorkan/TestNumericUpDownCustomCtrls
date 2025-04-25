namespace NumericUpDownCustomCtrls.Interfaces
{
    public interface IIntUpDownVM
    {
        /// <summary>
        /// Binding Mode=OneWay
        /// </summary>
        IntUpDownRange Range {get; set; }
        /// <summary>
        /// Binding Mode=TwoWay or OneWayToSource
        /// </summary>
        int Value { get; set; }
        /// <summary>
        /// Binding Mode=OneWayToSource
        /// </summary>
        int MinValue { set; }
        /// <summary>
        /// Binding Mode=OneWayToSource
        /// </summary>
        int MaxValue { set; }
        /// <summary>
        /// Binding Mode=OneWayToSource
        /// </summary>
        bool CanIncrement { set; }
        /// <summary>
        /// Binding Mode=OneWayToSource
        /// </summary>
        bool CanDecrement { set; }
    }
}
