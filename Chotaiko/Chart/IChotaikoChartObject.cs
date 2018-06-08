using Chotaiko.Game;
using Chotaiko.Play;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chotaiko.Chart
{
    /// <summary>
    /// This is an object which provides information on one chart object. This is 
    /// directly from the point of view of the file, and has methods to create game
    /// and objects. This only stores information about the type of note, the time, 
    /// the ID, and tells whether or not the object is ready to be drawn.
    /// </summary>c
    interface IChotaikoChartObject
    {
        /// <summary>
        /// This method describes if the chart object is ready to be added to the drawing and updating loop.
        /// </summary>
        /// <param name="CurrentOffset">Current offset in time</param>
        /// <param name="ChartInfo">Information on current chart</param>
        /// <returns>Whether the object is ready to be added to the current game context</returns>
        bool IsReady(TimeSpan CurrentOffset, ChotaikoChartInfo ChartInfo);

        /// <summary>
        /// This method returns the ID of the current object. These are assigned in order, and are used to assign a key to an object upon pressing.
        /// </summary>
        /// <returns>ID of the current object.</returns>
        int GetID();

        /// <summary>
        /// Provides the current object as a drawable game object.
        /// </summary>
        /// <returns>The current object as a drawable game object.</returns>
        IChotaikoGameObject AsGameObject();

        /// <summary>
        /// Provides the current object as a maintainable play object
        /// </summary>
        /// <returns>The current object as a maintainable play object.</returns>
        IChotaikoPlayObject AsPlayObject();
    }
}
