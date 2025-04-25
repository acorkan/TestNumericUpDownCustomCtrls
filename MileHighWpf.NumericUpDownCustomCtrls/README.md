# MileHighWpf. Package

This was inspired by the on-line sample from Microsoft, but I wanted more functionality with both an Int32 version (IntUpDownCtrl) and a float version (FloatUpDownCtrl), mouse-wheel input, and the option to increase or decrease the value by x10. The internals of the control use double to hold values so when using the float version of the control there are no rounding errors over long spans of values.



## Prerequisites

This package targets Windows and is built on .Net 8.0.

## Getting started

### Properties and Bindings
The UpDown controls have the following properties (see the interface folder for a properties cheat-sheet):
* Range: This is a way of specifying the *MinLimit*, *MaxLimit*, updated *Value* and *Increment* all in a single binding. This prevents cases where you are modifying one of these and it causes a conflict with other properties before you have a chance to update them as well. Binding mode is *OneWay*.
* Value: This is the current value. If it is outside the current limits when specified then it will be set to the limit that it is violating. Binding mode can be *TwoWay* or *OneWayToSource*.
* Increment: This is the change applied to the *Value* property as the user increments or decrements the control using either the buttons or the mouse-wheel. This property defaults to 1, and can not be more than half the range between *MinLimit* and *MaxLimit*. Binding mode is either *TwoWay* or *OneWayToSource*.
* MinLimit/MaxLimit: These are the limits that constrain the *Value* property as the user increments or decrements the control using either the buttons or the mouse-wheel. When the *Value* is within the *Increment* of one of the limits then the related button will be disabled and the Value test will turn red. These default to the maximum and minimum values of either int or float. Binding mode is *OneWayToSource*.
* CanIncrement/CanDecrement: Set when the *Value* property can not be increased or decreased by the *Increment* amount. These values are synchronized with the control button states and if either are set then the *Value* text appears in red. Binding mode is *OneWayToSource*.

### Visual Elements
There are three visual elements that the user interacts with.
* Value text: The current value is displayed in a text box in either black numbers or if at a range limit then in red. In the float version of the control the decimal places are determined by the significant digits of the *Increment* property.
* Focus border: The control has a border that is activated when the control has the UI focus. This indicates the the buttons in the control will respond to the mouse-wheel and also that pressing the Alt key will cause the value to change at x10 the normal *Increment* value.
* Buttons: The up and down buttons respond to mouse-clicks or, when the mouse is over a button, to mouse-wheel scrolling when the control has the focus.

You can see an example of implementation in a small demo application at the github link below but here are the basic steps.

### Quick Start
* Create a WPF project and install the MileHighWpf.NumericUpDownCustomCtrls package.
* In the main window XAML file add the XML namespace entry as **'numUpDn'** and inset these lines in the < Grid > section:
```
<numUpDn:FloatUpDownCtrl Name="upperLimit" Width="180" Grid.Row="0" Height="40" Visibility="Visible"
        Range="{Binding FRange, Mode=OneWay}"
                       Increment="{Binding Inc, Mode=OneWay}"
        Value="{Binding FValue, Mode=OneWayToSource}"/>
<numUpDn:IntUpDownCtrl Name="lowerLimit" Width="180" Grid.Row="1" Height="40"
```
* In the corresponding ViewModel implement the following properties (this example assumes you are using CommunityToolkit.Mvvm 8.4 or higher so adapt your binding mechanism according to your MVVM model):
```
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
         Trace.WriteLine(\$"OnValueIChanged {value}");
     }
     #endregion testing numupdown
 }
 ```
 

## Feedback and Sample Usage

https://github.com/acorkan/TestNumericUpDownCustomCtrls
