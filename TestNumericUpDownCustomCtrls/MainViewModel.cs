using CommunityToolkit.Mvvm.ComponentModel;
using MileHighWpf.NumericUpDownCustomCtrls;
using System.Diagnostics;

namespace TestNumericUpDownCustomCtrls
{
    public partial class MainViewModel : ObservableObject 
    {
        #region testing numupdown
        public FloatUpDownRange FRange { get; } = new FloatUpDownRange(-1, 2000, 1, 0.15F);
        [ObservableProperty]
        private float _fValue;
        partial void OnFValueChanged(float value)
        {
            Trace.WriteLine($"OnFValueChanged {value}");
        }
        public float Inc { get => 1.5F; }


        public IntUpDownRange Range { get; } = new IntUpDownRange(-13, 2000, 10, 20);
        [ObservableProperty]
        private int _value;
        partial void OnValueChanged(int value)
        {
            Trace.WriteLine($"OnValueIChanged {value}");
        }
        #endregion testing numupdown
    }
}
