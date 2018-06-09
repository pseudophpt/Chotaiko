using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chotaiko.Chart
{
    /// <summary>
    /// This class provides a structure for the info about a chart
    /// </summary>
    class ChotaikoChartInfo
    {
        /// <summary>
        /// Factor by which to multiply hit range for an increment in AccValue
        /// </summary>
        public const double AccFactor = 0.9;

        /// <summary>
        /// Factor by which to multiply screen interval for an increment in SpeedValue
        /// </summary>
        public const double SpeedFactor = 0.8;

        /// <summary>
        /// Value of hit range at AccValue 0
        /// </summary>
        public const double AccConstant = 1000;

        /// <summary>
        /// Value of screen interval at SpeedValue 0
        /// </summary>
        public const double SpeedConstant = 2000;

        /// <summary>
        /// The accuracy value as provided in the chart file.
        /// </summary>
        public double AccValue { get; }

        /// <summary>
        /// The minimum BPM value as provided in the chart file.
        /// </summary>
        public double MinimumBPM { get; set;  }

        /// <summary>
        /// The maximum BPM value as provided in the chart file.
        /// </summary>
        public double MaximumBPM { get; set;  }

        /// <summary>
        /// The speed value as provided in the chart file.
        /// </summary>
        public double SpeedValue { get; }

        /// <summary>
        /// The interval of time between the object spawning and the offset of the object. Determined
        /// by the speed value.
        /// </summary>
        public TimeSpan ScreenInterval { get; }

        /// <summary>
        /// The interval of time between beats. Determined by the BPM value.
        /// </summary>
        public TimeSpan BeatTime { get; }

        /// <summary>
        /// The interval of time in which a hit is valid. Determined by the accuracy value.
        /// </summary>
        public TimeSpan HitRange { get; }

        /// <summary>
        /// Creates new chart information structure
        /// </summary>
        /// <param name="AccValue">Accuracy value</param>
        /// <param name="BPM">BPM value</param>
        /// <param name="SpeedValue">Speed value</param>
        public ChotaikoChartInfo(double AccValue, double SpeedValue)
        {
            this.AccValue = AccValue;
            this.SpeedValue = SpeedValue;

            // HitRange = AccConstant * AccFactor^AccValue
            this.HitRange = TimeSpan.FromMilliseconds(AccConstant * Math.Pow(AccFactor, this.AccValue));

            // ScreenInterval = AccConstant * SpeedFactor^SpeedValue
            this.ScreenInterval = TimeSpan.FromMilliseconds(SpeedConstant * Math.Pow(SpeedFactor, this.SpeedValue));

            // Mark uninitialized
            this.MinimumBPM = -1;
            this.MaximumBPM = -1;
        }

        /// <summary>
        /// Updates BPM minimum and maximum statistics
        /// </summary>
        /// <param name="BPM">BPM Value</param>
        public void UpdateBPM(double BPM)
        {
            if (this.MinimumBPM < 0) this.MinimumBPM = BPM;
            if (this.MaximumBPM < 0) this.MaximumBPM = BPM;

            if (BPM < MinimumBPM) MinimumBPM = BPM;
            if (BPM > MaximumBPM) MaximumBPM = BPM;
        }
    }
}
