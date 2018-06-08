using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chotaiko.Play
{
    /// <summary>
    /// A play object representing the beat type object.
    /// </summary>
    class ChotaikoPlayBeat : IChotaikoPlayObject
    {
        /// <summary>
        /// Offset of the beat.
        /// </summary>
        TimeSpan Offset;

        /// <summary>
        /// Accuracy, if applicable of the beat.
        /// </summary>
        int Accuracy;

        /// <summary>
        /// Whether the note has been hit or not
        /// </summary>
        bool Finished;

        /// <summary>
        /// Creates new play beat based on offset
        /// </summary>
        /// <param name="Offset">Offset of the note</param>
        public ChotaikoPlayBeat(TimeSpan Offset)
        {
            this.Offset = Offset;
            this.Accuracy = 0;
            this.Finished = false;
        }

        public int GetAccuracy()
        {
            return this.Accuracy;
        }

        public TimeSpan GetOffset()
        {
            return this.Offset;
        }

        public void Hit(int Accuracy)
        {
            this.Finished = true;
            this.Accuracy = Accuracy;
        }

        public bool IsFinished()
        {
            return this.Finished;
        }
    }
}
