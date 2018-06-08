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
        /// The BPM value as provided in the chart file.
        /// </summary>
        public double BPM { get; }

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
        public ChotaikoChartInfo(double AccValue, double BPM, double SpeedValue)
        {
            this.AccValue = AccValue;
            this.BPM = BPM;
            this.SpeedValue = SpeedValue;

            // HitRange = AccConstant * AccFactor^AccValue
            this.HitRange = TimeSpan.FromMilliseconds(100 * Math.Pow(AccFactor, this.AccValue));

            // ScreenInterval = AccConstant * SpeedFactor^SpeedValue
            this.ScreenInterval = TimeSpan.FromMilliseconds(SpeedConstant * Math.Pow(SpeedFactor, this.SpeedValue));

            this.BeatTime = TimeSpan.FromMinutes(1 / this.BPM);
        }
    }
}
