using Chotaiko.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chotaiko.Game
{
    /// <summary>
    /// This is an object from the perspective of the game during play. This includes methods to draw,
    /// update, and handle key presses.
    /// </summary>
    interface IChotaikoGameObject
    {
        /// <summary>
        /// Returns the accuracy of the object, if hit
        /// </summary>
        /// <returns>Accuracy of object if hit</returns>
        int GetAccuracy();

        /// <summary>
        /// This method draws the object.
        /// </summary>
        /// <param name="CurrentOffset">Current offset in time</param>
        void Draw(TimeSpan CurrentOffset, ChotaikoChartInfo ChartInfo);

        /// <summary>
        /// This method updates the object.
        /// </summary>
        /// <param name="CurrentOffset">Current offset in time</param>
        /// <param name="ChartInfo">Information on current chart</param>
        void Update(TimeSpan CurrentOffset, ChotaikoChartInfo ChartInfo);

        /// <summary>
        /// This method describes whether the object is done rendering and updating.
        /// </summary>
        /// <param name="CurrentOffset">Current offset in time</param>
        /// <returns>Whether the object is ready to be discarded from the current gamee context.</returns>
        bool IsDone(TimeSpan CurrentOffset);

        /// <summary>
        /// This method handles a keypress from the user.
        /// </summary>
        /// <param name="CurrentOffset">Current offset in time</param>
        /// <param name="ChartInfo">Information on current chart</param>
        /// <returns>Whether the press was handled and should be considered eaten.</returns>
        bool OnPress(TimeSpan CurrentOffset, ChotaikoChartInfo ChartInfo);

        /// <summary>
        /// This method handles a key release.
        /// </summary>
        void OnRelease();

        /// <summary>
        /// Gets the ID of the game object
        /// </summary>
        /// <returns>ID of the game object</returns>
        int GetID();
    }
}
