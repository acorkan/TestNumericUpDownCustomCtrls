namespace MileHighWpf.NumericUpDownCustomCtrls.Interfaces
{
    /// <summary>
    /// Use this as a hint when creating bindings for the FloatUpDownCtrl.
    /// </summary>
    public interface IFloatUpDownCtrl
    {
        /// <summary>
        /// Binding Mode=OneWay
        /// </summary>
        FloatUpDownRange Range { get; set; }
        /// <summary>
        /// Binding Mode=TwoWay or OneWayToSource
        /// </summary>
        float Value { get; set; }
        /// <summary>
        /// Binding Mode=OneWayToSource
        /// </summary>
        float MinLimit { set; }
        /// <summary>
        /// Binding Mode=OneWayToSource
        /// </summary>
        float MaxLimit { set; }
        /// <summary>
        /// Binding Mode=TwoWay or OneWayToSource
        /// </summary>
        float Increment { get; set; }
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
