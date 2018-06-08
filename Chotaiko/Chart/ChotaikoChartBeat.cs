using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chotaiko.Game;
using Chotaiko.Play;

namespace Chotaiko.Chart
{
    /// <summary>
    /// A chart object representing the beat type object.
    /// </summary>
    class ChotaikoChartBeat : IChotaikoChartObject
    {
        /// <summary>
        /// Offset of the note in time
        /// </summary>
        TimeSpan Offset;

        /// <summary>
        /// ID of the note
        /// </summary>
        int NoteID;

        /// <summary>
        /// Creates new chart beat
        /// </summary>
        /// <param name="NoteID">ID of the note</param>
        /// <param name="Offset">Offset in time</param>
        public ChotaikoChartBeat(int NoteID, TimeSpan Offset)
        {
            this.NoteID = NoteID;
            this.Offset = Offset;
        }

        public IChotaikoGameObject AsGameObject()
        {
            return new ChotaikoGameBeat(this.Offset, this.NoteID);
        }

        public IChotaikoPlayObject AsPlayObject()
        {
            return new ChotaikoPlayBeat(this.Offset);
        }

        public int GetID()
        {
            return NoteID;
        }

        public bool IsReady(TimeSpan CurrentOffset, ChotaikoChartInfo ChartInfo)
        {
            // Return whether the current offset plus the added time on screen is larger than the 
            // offset of this object.
            return CurrentOffset + ChartInfo.ScreenInterval >= this.Offset;
        }
    }
}
