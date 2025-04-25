namespace MileHighWpf.NumericUpDownCustomCtrls.Interfaces
{
    /// <summary>
    /// Use this as a hint when creating bindings for the IntUpDownCtrl.
    /// </summary>
    public interface IIntUpDownCtrl
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
        int MinLimit { set; }
        /// <summary>
        /// Binding Mode=OneWayToSource
        /// </summary>
        int MaxLimit { set; }
        /// <summary>
        /// Binding Mode=TwoWay or OneWayToSource
        /// </summary>
        int Increment { get; set; }
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
