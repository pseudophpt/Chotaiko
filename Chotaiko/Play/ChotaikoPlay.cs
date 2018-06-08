using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chotaiko.Chart;

namespace Chotaiko.Play
{
    /// <summary>
    /// This class represents a whole play. One play consists of all the play objects. 
    /// Its constructor takes in a chart from which to construct the play.
    /// </summary>
    class ChotaikoPlay
    {
        List<IChotaikoPlayObject> PlayObjects;

        /// <summary>
        /// Creatres new plat based on chart
        /// </summary>
        /// <param name="Chart">Chart to create play from</param>
        public ChotaikoPlay(ChotaikoChart Chart)
        {
            this.PlayObjects = new List<IChotaikoPlayObject>();

            // Convert chart objects to play objects
            for (int i = 0; i < Chart.Objects.Count; i ++)
                this.PlayObjects.Add(Chart.Objects[i].AsPlayObject());
        }

        /// <summary>
        /// Hits a certain object in the play
        /// </summary>
        /// <param name="NoteID">ID of the object</param>
        /// <param name="Accuracy">Accuracy of the hit</param>
        public void HitObject(int NoteID, int Accuracy)
        {
            this.PlayObjects[NoteID].Hit(Accuracy);
        }
    }
}
