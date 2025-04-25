using MileHighWpf.NumericUpDownCustomCtrls;

namespace Tests
{
    /// <summary>
    /// float.MaxValue is 340282300000000000000000000000000000000F.
    /// </summary>
    [TestClass]
    public sealed class Test1
    {
        #region IntUpDownRange
        /// <summary>
        /// Should be no problem
        /// </summary>
        [TestMethod]
        public void TestIntRangeCtor1()
        {
            IntUpDownRange range = new IntUpDownRange(-1, 2000);
        }

        /// <summary>
        /// Error for reversing min and max.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestIntRangeCtor2()
        {
            IntUpDownRange range = new IntUpDownRange(200, 20);
        }

        /// <summary>
        /// Increment is greater than half the range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestIntRangeCtor3()
        {
            IntUpDownRange range = new IntUpDownRange(-1, 2, 1, 2);
        }

        /// <summary>
        /// Increment is half the range, should be OK.
        /// </summary>
        [TestMethod]
        public void TestIntRangeCtor4()
        {
            IntUpDownRange range = new IntUpDownRange(-1, 3, 1, 2);
        }

        /// <summary>
        /// Value less than range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestIntRangeCtor5()
        {
            IntUpDownRange range = new IntUpDownRange(-1, 2000, -2);
        }

        /// <summary>
        /// Value greater than range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestIntRangeCtor6()
        {
            IntUpDownRange range = new IntUpDownRange(-1, 2000, 3000);
        }
        #endregion IntUpDownRange
        #region FloatUpDownRange
        /// <summary>
        /// Should be no problem
        /// </summary>
        [TestMethod]
        public void TestFloatRangeCtor1()
        {
            FloatUpDownRange range = new FloatUpDownRange(-1.5F, 200.0F);
        }

        /// <summary>
        /// Error for reversing min and max.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestFloatRangeCtor_MinMaxReversed()
        {
            FloatUpDownRange range = new FloatUpDownRange(200F, 20F);
        }

        /// <summary>
        /// Increment is greater than half the range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestFloatRangeCtor_IncrementGreaterThanHalfOfRange()
        {
            FloatUpDownRange range = new FloatUpDownRange(-1, 2, 1, 2);
        }

        /// <summary>
        /// Increment is half the range, should be OK.
        /// </summary>
        [TestMethod]
        public void TestFloatRangeCtor_IncrementHalfOfRange()
        {
            FloatUpDownRange range = new FloatUpDownRange(-1, 3, 1.2F, 2.0F);
        }

        /// <summary>
        /// Value less than range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestFloatRangeCtor_ValueLessThanRange()
        {
            FloatUpDownRange range = new FloatUpDownRange(-1, 2000, -2.0F);
        }

        /// <summary>
        /// Value greater than range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestFloatRangeCtor_ValueGreaterThanRange()
        {
            FloatUpDownRange range = new FloatUpDownRange(-1, 2000, 2000.001F);
        }
        #endregion FloatUpDownRange

        /// <summary>
        /// Test for rounding errors.
        /// </summary>
        [TestMethod]
        public void TestFloatRoundingErrors()
        {
            bool pass = false;
            Thread t = new Thread(() =>
            {                
                float inc = 135.01247f;
                FloatUpDownRange range = new FloatUpDownRange(float.MinValue, float.MaxValue, 0F, inc);
                FloatUpDownCtrl floatUpDownCtrl = new FloatUpDownCtrl(range);
                System.Diagnostics.Trace.WriteLine($"TestMethod2() {floatUpDownCtrl.Value}");
                int loops = 100000;
                for (int i = 0; i < loops; i++)
                {
                    floatUpDownCtrl.DoUpButtonClick();
                    System.Diagnostics.Trace.WriteLine($"  {i}: {floatUpDownCtrl.Value}");
                }
                for (int i = loops; i > 0; i--)
                {
                    floatUpDownCtrl.DoDownButtonClick();
                    System.Diagnostics.Trace.WriteLine($"  {i}: {floatUpDownCtrl.Value}");
                }

                System.Diagnostics.Trace.WriteLine($"TestMethod2() {floatUpDownCtrl.Value}");
                pass = (floatUpDownCtrl.Value == 0F);
            });
            t.SetApartmentState(ApartmentState.STA);

            t.Start();
            t.Join();
            Assert.IsTrue(pass);
            //FloatUpDownRange range = new FloatUpDownRange(-1, 2000, 1, 1);
            //FloatUpDownCtrl floatUpDownCtrl = new FloatUpDownCtrl();
        }

        /// <summary>
        /// Test for float limits.
        /// </summary>
        [TestMethod]
        public void TestFloatLimits()
        {
            bool pass = false;
            float maxReached = 0;
            float minReached = 0;
            Thread t = new Thread(() =>
            {                
                float inc = 15.01247f;
                FloatUpDownRange range = new FloatUpDownRange(-90F, 60F, 0F, inc);
                FloatUpDownCtrl floatUpDownCtrl = new FloatUpDownCtrl(range);
                System.Diagnostics.Trace.WriteLine($"TestMethod3() {floatUpDownCtrl.Value}");
                while(floatUpDownCtrl.CanIncrement)
                {
                    floatUpDownCtrl.DoUpButtonClick();
                }
                maxReached = floatUpDownCtrl.Value;
                System.Diagnostics.Trace.WriteLine($"  {maxReached}");
                while (floatUpDownCtrl.CanDecrement)
                {
                    floatUpDownCtrl.DoDownButtonClick();
                }
                minReached = floatUpDownCtrl.Value;
                System.Diagnostics.Trace.WriteLine($"  {minReached}");
                pass = (maxReached == 45.03741F) && (minReached == -75.06235F);
            });
            t.SetApartmentState(ApartmentState.STA);

            t.Start();
            t.Join();
            Assert.IsTrue(pass);
        }

        /// <summary>
        /// Test for float limits after setting range.
        /// </summary>
        [TestMethod]
        public void TestInitialRangeFloatLimits()
        {
            bool pass = false;
            bool canIncrement = false;
            bool canDecrement = false;
            Thread t = new Thread(() =>
            {
                float inc = 150.01247f;
                FloatUpDownRange range = new FloatUpDownRange(-900F, 60F, 0F, inc);
                FloatUpDownCtrl floatUpDownCtrl = new FloatUpDownCtrl(range);
                System.Diagnostics.Trace.WriteLine($"TestMethod3() {floatUpDownCtrl.Value}");
                canDecrement = floatUpDownCtrl.CanDecrement;
                canIncrement = floatUpDownCtrl.CanIncrement;
                System.Diagnostics.Trace.WriteLine($"  {canDecrement}  {canIncrement}");
                pass = canDecrement && !canIncrement;
            });
            t.SetApartmentState(ApartmentState.STA);

            t.Start();
            t.Join();
            Assert.IsTrue(pass);
        }

        /// <summary>
        /// Test for float limits after setting range.
        /// </summary>
        [TestMethod]
        public void TestInitialRangeIntLimits()
        {
            bool pass = false;
            bool canIncrement = false;
            bool canDecrement = false;
            Thread t = new Thread(() =>
            {
                int inc = 150;
                IntUpDownRange range = new IntUpDownRange(-900, 60, 0, inc);
                IntUpDownCtrl intUpDownCtrl = new IntUpDownCtrl(range);
                System.Diagnostics.Trace.WriteLine($"TestMethod3() {intUpDownCtrl.Value}");
                canDecrement = intUpDownCtrl.CanDecrement;
                canIncrement = intUpDownCtrl.CanIncrement;
                System.Diagnostics.Trace.WriteLine($"  {canDecrement}  {canIncrement}");
                pass = canDecrement && !canIncrement;
            });
            t.SetApartmentState(ApartmentState.STA);

            t.Start();
            t.Join();
            Assert.IsTrue(pass);
        }


        /// <summary>
        /// Test for int limits.
        /// </summary>
        [TestMethod]
        public void TestIntLimits()
        {
            bool pass = false;
            int maxReached = 0;
            int minReached = 0;
            Thread t = new Thread(() =>
            {
                int inc = 175;
                IntUpDownRange range = new IntUpDownRange(-900, 190, 0, inc);
                IntUpDownCtrl intUpDownCtrl = new IntUpDownCtrl(range);
                System.Diagnostics.Trace.WriteLine($"TestMethod3() {intUpDownCtrl.Value}");
                while (intUpDownCtrl.CanIncrement)
                {
                    intUpDownCtrl.DoUpButtonClick();
                }
                maxReached = intUpDownCtrl.Value;
                System.Diagnostics.Trace.WriteLine($"  {maxReached}");
                while (intUpDownCtrl.CanDecrement)
                {
                    intUpDownCtrl.DoDownButtonClick();
                }
                minReached = intUpDownCtrl.Value;
                System.Diagnostics.Trace.WriteLine($"  {minReached}");
                pass = (maxReached == 175) && (minReached == -875);
            });
            t.SetApartmentState(ApartmentState.STA);

            t.Start();
            t.Join();
            Assert.IsTrue(pass);
        }
        //[TestMethod]
        //public void TestMethod3()
        //{
        //    FloatUpDownRange range = new FloatUpDownRange(-1, 2000, 1);
        //    FloatUpDownCtrl floatUpDownCtrl = new FloatUpDownCtrl();
        //}

    }
}
