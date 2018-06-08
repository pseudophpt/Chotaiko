using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chotaiko.Play
{
    /// <summary>
    /// This is an object from the perspective of the main game. 
    /// It contains information on the accuracy of the hit and the offset.
    /// </summary>
    interface IChotaikoPlayObject
    {
        /// <summary>
        /// Returns the accuracy of the object, if hit yet.
        /// </summary>
        /// <returns>The accuracy of the object, if hit yet.</returns>
        int GetAccuracy();

        /// <summary>
        /// Gets the offset of the object in time.
        /// </summary>
        /// <returns>The offset of the object in time.</returns>
        TimeSpan GetOffset();

        /// <summary>
        /// Returns whether the object has been hit yet or is still either within the current drawable
        /// state or is ready to be put within the drawable state.
        /// </summary>
        /// <returns>Whether the object has been hit yet</returns>
        bool IsFinished();

        /// <summary>
        /// Marks the object as hit with a certain accuracy.
        /// </summary>
        /// <param name="Accuracy">Accuracy of the hit</param>
        void Hit(int Accuracy);
    }
}
