using Chotaiko.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chotaiko.Game
{
    /// <summary>
    /// A game object representing the beat type object.
    /// </summary>
    class ChotaikoGameBeat : IChotaikoGameObject
    {
        /// <summary>
        /// Nullable time interval representing the time when the object was hit. If null,
        /// the object hasn't been hit yet.
        /// </summary>
        private TimeSpan? HitOffset;

        /// <summary>
        /// Offset of the note
        /// </summary>
        private TimeSpan Offset;

        /// <summary>
        /// ID of the note
        /// </summary>
        private int NoteID;

        /// <summary>
        /// Accuracy of the note
        /// </summary>
        private int Accuracy;

        /// <summary>
        /// Angle in standard position (0 to 1)
        /// </summary>
        double Theta;

        /// <summary>
        /// Creates new game beat
        /// </summary>
        /// <param name="Offset">Offset of beat</param>
        public ChotaikoGameBeat(TimeSpan Offset, int NoteID, double Theta)
        {
            this.Offset = Offset;
            this.NoteID = NoteID;
            this.Theta = Theta;
            this.Accuracy = 0;
        }

        public int GetAccuracy()
        {
            return this.Accuracy;
        }

        public void Draw(TimeSpan CurrentOffset, ChotaikoChartInfo ChartInfo)
        {
            /*// Error
            double Error = -(CurrentOffset - this.Offset).TotalMilliseconds;

            // Error percentage
            double ErrorPercent = 30 * Error / ChartInfo.ScreenInterval.TotalMilliseconds;

            if (ErrorPercent < 30 && ErrorPercent > 0)
            {
                for (int i = 0; i < ErrorPercent; i++) Console.Write("|");
                Console.WriteLine();
            }*/
            Console.WriteLine("Beat with angle " + this.Theta + " ID " + this.NoteID);
        }

        public int GetID()
        {
            return this.NoteID;
        }

        public bool IsDone(TimeSpan CurrentOffset)
        {
            if (HitOffset.HasValue)
                return CurrentOffset > this.HitOffset + TimeSpan.FromMilliseconds(100);
            else return false;
        }

        public bool OnPress(TimeSpan CurrentOffset, ChotaikoChartInfo ChartInfo)
        {
            // Already hit
            if (this.HitOffset.HasValue)
            {
                return false;
            }

            // Too late
            if (CurrentOffset > this.Offset + ChartInfo.HitRange)
            {
                this.HitOffset = this.Offset + ChartInfo.HitRange;
                return false;
            }

            // Too early
            else if (CurrentOffset < this.Offset - ChartInfo.HitRange)
            {
                // Hit now
                this.HitOffset = CurrentOffset;
                
                // Miss
                this.Accuracy = 0;
            }

            // Within range
            else
            {
                // Hit now
                this.HitOffset = CurrentOffset;

                // Hit error
                double Error = Math.Abs((CurrentOffset - this.Offset).TotalMilliseconds);

                // Error percentage
                double ErrorPercent = Error / ChartInfo.HitRange.TotalMilliseconds;

                // Accuracy = Hit error * 100
                this.Accuracy = 100 - Convert.ToInt32(100 * ErrorPercent);
            }

            Console.WriteLine(this.Accuracy);

            return true;
        }

        public void OnRelease()
        {
            throw new NotImplementedException();
        }

        public void Update(TimeSpan CurrentOffset, ChotaikoChartInfo ChartInfo)
        {
            // If too late, set as hit and accuracy to 0
            if (!HitOffset.HasValue && CurrentOffset > this.Offset + ChartInfo.HitRange)
            {
                this.HitOffset = CurrentOffset;
                this.Accuracy = 0;
            }
        }
    }
}
