using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using System.Numerics;

namespace MileHighWpf.NumericUpDownCustomCtrls
{
    /// <summary>
    /// This uses double as the backing field to allow for decimal values and avouid roundiong errors when T is float.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    [TemplatePart(Name = "UpButtonElement", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "DownButtonElement", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "TextBlock", Type = typeof(TextBlock))]
    [TemplateVisualState(Name = "InRange", GroupName = "ValueStates")]
    [TemplateVisualState(Name = "AtLimit", GroupName = "ValueStates")]
    [TemplateVisualState(Name = "Focused", GroupName = "FocusedStates")]
    [TemplateVisualState(Name = "Unfocused", GroupName = "FocusedStates")]
    public abstract class UpDownCtrlBase<T, R> : Control where T : struct, IComparable<T>, IMinMaxValue<T> where R : UpDownRangeBase<T>
    {
        public string FormatString { get; set; } = "";
        public int SigDigits { get; set; } = 0;

        protected double _increment;
        protected double _value;
        protected double _minLimit;
        protected double _maxLimit;
        protected bool _internalUpdate = false;

        protected virtual void UpdateFormatFromIncrement()
        {
            FormatString = ""; 
            SigDigits = 0;
        }
        protected virtual string GetTextElementText(T value) { return value.ToString(); }

        public UpDownCtrlBase()
        {
            DefaultStyleKey = typeof(UpDownCtrlBase<T,R>);
            this.IsTabStop = true;
            UpdateEnables();
            UpdateStates(true);
        }

        #region Value Property
        public static readonly DependencyProperty ValueProperty =
                  DependencyProperty.Register(
                      "Value", typeof(T), typeof(UpDownCtrlBase<T, R>),
                      new PropertyMetadata(default(T),
                          new PropertyChangedCallback(ValueChangedCallback)));//,
                          //new CoerceValueCallback(CoerceValueCallback)));
        public T Value
        {
            get => (T)Convert.ChangeType(_value, typeof(T));
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        protected void AssignNewValue(double newValue)
        {
            try
            {
                _internalUpdate = true;
                _value = newValue;
                SetValue(ValueProperty, (T)Convert.ChangeType(_value, typeof(T)));
                UpdateEnables();
                UpdateStates(true);
                UpdateText();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _internalUpdate = false;
            }
        }
        private static void ValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            UpDownCtrlBase<T, R> ctl = (UpDownCtrlBase<T, R>)obj;
            T newValue = (T)args.NewValue;
            Trace.WriteLine($"ValueChangedCallback {newValue}");
            // Don't update if we are the source of the change.
            if (ctl._internalUpdate) { return; }
            Trace.WriteLine($"   update internal");
            ctl.AssignNewValue(Convert.ToDouble(newValue));
        }
        #endregion Value Property
        #region Range Property
        public static readonly DependencyProperty RangeProperty =
                  DependencyProperty.Register(
                      nameof(Range), typeof(R), typeof(UpDownCtrlBase<T,R>),
                      new PropertyMetadata(
                          new PropertyChangedCallback(RangeValueChangedCallback)));
        public R Range
        {
            set { SetValue(RangeProperty, value); }
        }
        private static void RangeValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            UpDownCtrlBase<T,R> ctl = (UpDownCtrlBase<T,R>)obj;
            R newRange = (R)args.NewValue;

            ctl._maxLimit = Convert.ToDouble(newRange.HasMax ? newRange.GetMax() : T.MaxValue);
            ctl._minLimit = Convert.ToDouble(newRange.HasMin ? newRange.GetMin() : T.MinValue);

            ctl.SetValue(MinLimitProperty, Convert.ChangeType(ctl._minLimit, typeof(T)));
            ctl.SetValue(MaxLimitProperty, Convert.ChangeType(ctl._maxLimit, typeof(T)));

            // Optional update of value here, it is already guarenteed to be in range.
            if (newRange.HasNewValue)
            {
                ctl.AssignNewValue(Convert.ToDouble(newRange.GetNewValue()));
            }
            if (newRange.HasNewIncrement)
            {
                ctl._increment = Convert.ToDouble(newRange.GetNewIncrement());
                ctl.SetValue(IncrementProperty, newRange.GetNewIncrement());
            }
            else // We need to range test the existing value
            {
                double newValue = ctl.CropValue(ctl._value);
                if(ctl._value != newValue)
                {
                    ctl.AssignNewValue(newValue);
                }
            }

            // Call UpdateStates because the Value might have caused the
            // control to change ValueStates.
            ctl.UpdateStates(true);
            ctl.UpdateEnables();
            ctl.UpdateFormatFromIncrement();
            ctl.UpdateText();
        }
        #endregion Range Property
        #region Increment Property
        public static readonly DependencyProperty IncrementProperty =
                  DependencyProperty.Register(
                      nameof(Increment), typeof(T), typeof(UpDownCtrlBase<T,R>),
                      new PropertyMetadata(default(T), new PropertyChangedCallback(IncrementValueChangedCallback)));
        public T Increment
        {
            get => (T)GetValue(IncrementProperty);
            set { SetValue(IncrementProperty, value); }
        }
        private static void IncrementValueChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            UpDownCtrlBase<T,R> ctl = (UpDownCtrlBase<T,R>)obj;
            ctl._increment = Convert.ToDouble(ctl.Increment);
            ctl.UpdateFormatFromIncrement();
            ctl.UpdateText();
        }
        #endregion Increment Property
        #region Min Limit Property, is ReadOnly
        public static readonly DependencyProperty MinLimitProperty =
            DependencyProperty.Register(
                      nameof(MinLimit), typeof(T), typeof(UpDownCtrlBase<T,R>),
                      new PropertyMetadata());
        public T MinLimit
        {
            get => (T)GetValue(MinLimitProperty);
        }
        #endregion Min Limit Property
        #region Max Limit Property, is ReadOnly
        public static readonly DependencyProperty MaxLimitProperty =
            DependencyProperty.Register(
                      nameof(MaxLimit), typeof(T), typeof(UpDownCtrlBase<T,R>),
                      new PropertyMetadata());
        public T MaxLimit
        {
            get => (T)GetValue(MaxLimitProperty);
        }
        #endregion Max Limit Property
        #region CanDecrementProperty, is ReadOnly
        public static readonly DependencyProperty CanDecrementProperty =
            DependencyProperty.Register(
                nameof(CanDecrement), typeof(bool), typeof(UpDownCtrlBase<T,R>),
                new PropertyMetadata(false));
        public bool CanDecrement
        {
            get => (bool)GetValue(CanDecrementProperty);
            set { SetValue(CanDecrementProperty, value); }
        }
        #endregion CanDecrementProperty, is ReadOnly
        #region CanIncrementProperty, is ReadOnly
        public static readonly DependencyProperty CanIncrementProperty =
            DependencyProperty.Register(
                nameof(CanIncrement), typeof(bool), typeof(UpDownCtrlBase<T,R>),
                new PropertyMetadata(false)); 
        public bool CanIncrement
        {
            get => (bool)GetValue(CanIncrementProperty);
            set { SetValue(CanIncrementProperty, value); }
        }
        #endregion CanIncrementProperty, is ReadOnly

        public bool IsAtMaxLimit { get => (_maxLimit - _value) < _increment; }

        public bool IsAtMinLimit { get => (_value - _minLimit) < _increment; }

        protected void UpdateText()
        {
            if (TextElement != null)
            {
                TextElement.Text = GetTextElementText(Value);
                TextElement.ToolTip = FormatTextBlockTooltip();
            }
        }
        protected void UpdateEnables()
        {
            CanIncrement = !IsAtMaxLimit;
            if (UpButtonElement != null)
            {
                UpButtonElement.IsEnabled = !IsAtMaxLimit;
            }
            CanDecrement = !IsAtMinLimit;
            if (DownButtonElement != null)
            {
                DownButtonElement.IsEnabled = !IsAtMinLimit;
            }
        }
        protected void UpdateStates(bool useTransitions)
        {
            VisualStateManager.GoToState(this, (IsAtMaxLimit || IsAtMinLimit) ? "AtLimit" : "InRange", useTransitions);
            VisualStateManager.GoToState(this, IsFocused ? "Focused" : "Unfocused", useTransitions);
        }

        public override void OnApplyTemplate()
        {
            UpButtonElement = GetTemplateChild("UpButton") as RepeatButton;
            DownButtonElement = GetTemplateChild("DownButton") as RepeatButton;
            TextElement = GetTemplateChild("TextBlock") as TextBlock;
            UpdateStates(false);
        }

        /// <summary>
        /// Get me as close to the new value as you can using the increment value.
        /// </summary>
        /// <param name="incrementChange"></param>
        /// <returns></returns>
        private bool TryChangeValue(int incrementChange)
        {
            double testValue = _value + _increment * incrementChange;
            if((testValue > _maxLimit) || (testValue < _minLimit)) 
            {
                AssignNewValue(CropValue(testValue));
                return false;
            }
            else
            {
                AssignNewValue(testValue);
                return true;
            }
        }

        private double CropValue(double newValue)
        {
            if(newValue < _minLimit)
            {
                double allowedChange = _minLimit - _value;
                int allowedIncrements = (int)(allowedChange / _increment);
                return _value + (allowedIncrements * _increment);
            }
            else if (newValue > _maxLimit)
            {
                double allowedChange = _maxLimit - _value;
                int allowedIncrements = (int)(allowedChange / _increment);
                return _value + (allowedIncrements * _increment);
            }
            return newValue;
        }

        protected virtual string FormatTextBlockTooltip()
        {
            return $"Range: {_minLimit} .. {_maxLimit}";
        }

        private TextBlock _textElement;
        protected TextBlock TextElement
        {
            get => _textElement;
            set
            {
                _textElement = value;
                if (_textElement != null)
                {
                    _textElement.ToolTip = FormatTextBlockTooltip();
                }
            }
        }

        private RepeatButton _downButtonElement;
        private RepeatButton DownButtonElement
        {
            get => _downButtonElement;
            set
            {
                if (_downButtonElement != null)
                {
                    _downButtonElement.Click -=
                        new RoutedEventHandler(DownButtonElementClick);
                    _downButtonElement.PreviewMouseWheel -= _downButtonElement_PreviewMouseWheel;
                }
                _downButtonElement = value;

                if (_downButtonElement != null)
                {
                    _downButtonElement.Click +=
                        new RoutedEventHandler(DownButtonElementClick);
                    _downButtonElement.PreviewMouseWheel += _downButtonElement_PreviewMouseWheel;
                }
            }
        }

        private void ProcessMouseWheel(int delta)
        {
            int incrementSteps = 1;
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
            {
                incrementSteps *= 10;
            }
            if (delta > 0)
            {
                TryChangeValue(incrementSteps);
            }
            else
            {
                TryChangeValue(-incrementSteps);
            }
        }

        private void _downButtonElement_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (IsFocused)
            {
                ProcessMouseWheel(e.Delta);
            }
        }

        public void DoDownButtonClick()
        {
            DownButtonElementClick(null, null);
        }

        public void DownButtonElementClick(object sender, RoutedEventArgs e)
        {
            int incrementSteps = 1;
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
            {
                incrementSteps *= 10;
            }
            TryChangeValue(-incrementSteps);

            Focus();
        }

        private RepeatButton _upButtonElement;
        private RepeatButton UpButtonElement
        {
            get => _upButtonElement;
            set
            {
                if (_upButtonElement != null)
                {
                    _upButtonElement.Click -=
                        new RoutedEventHandler(UpButtonElementClick);
                    _upButtonElement.PreviewMouseWheel -= _upButtonElement_PreviewMouseWheel;
                }
                _upButtonElement = value;

                if (_upButtonElement != null)
                {
                    _upButtonElement.Click +=
                        new RoutedEventHandler(UpButtonElementClick);
                    _upButtonElement.PreviewMouseWheel += _upButtonElement_PreviewMouseWheel;
                }
            }
        }

        private void _upButtonElement_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (IsFocused)
            {
                ProcessMouseWheel(e.Delta);
            }
        }

        public void DoUpButtonClick()
        {
            UpButtonElementClick(null, null);
        }
        public void UpButtonElementClick(object sender, RoutedEventArgs e)
        {
            int incrementSteps = 1;
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
            {
                incrementSteps *= 10;
            }
            TryChangeValue(incrementSteps);
            Focus();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            Focus();
        }


        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            UpdateStates(true);
            UpdateEnables();
            UpdateText();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            UpdateStates(true);
            UpdateEnables();
            UpdateText();
        }
    }
}