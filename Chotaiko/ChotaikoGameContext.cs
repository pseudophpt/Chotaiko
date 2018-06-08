using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chotaiko.Chart;
using Chotaiko.Game;
using Chotaiko.Play;

namespace Chotaiko
{
    /// <summary>
    /// One context of the game. Each play / song has a new context.
    /// </summary>
    class ChotaikoGameContext
    {
        /// <summary>
        /// Chart to read from for objects
        /// </summary>
        private ChotaikoChart Chart;

        /// <summary>
        /// Length of time elapsed before pause
        /// </summary>
        private TimeSpan PauseTime;

        /// <summary>
        /// Start of current play instance
        /// </summary>
        private DateTime Start;

        /// <summary>
        /// Current offset of time from current play instance
        /// </summary>
        private TimeSpan CurrentOffset;

        /// <summary>
        /// All on-screem game objects
        /// </summary>
        private List<IChotaikoGameObject> OnScreenObjects;

        /// <summary>
        /// Current play
        /// </summary>
        private ChotaikoPlay CurrentPlay;

        /// <summary>
        /// Since chart objects are in order, this variable represents the ID of the object ready to be
        /// converted to drawable.
        /// </summary>
        private int CurrentID;

        /// <summary>
        /// Creates a game context given a certain chart
        /// </summary>
        /// <param name="Chart">Chart to create game context from</param>
        public ChotaikoGameContext(ChotaikoChart Chart)
        {
            this.Chart = Chart;
            this.PauseTime = TimeSpan.Zero;
            this.Start = DateTime.Now;
            this.CurrentOffset = TimeSpan.Zero;
            this.CurrentID = 0;

            this.OnScreenObjects = new List<IChotaikoGameObject>();
            this.CurrentPlay = new ChotaikoPlay(this.Chart);
        }

        /// <summary>
        /// Updates offset time
        /// </summary>
        private void UpdateOffset()
        {
            // Difference between now and the start of the current runtime
            this.CurrentOffset = (DateTime.Now - this.Start);
        }

        /// <summary>
        /// Gets time elapsed from beginning of play
        /// </summary>
        /// <returns>Time elapsed from beginning of play</returns>
        public TimeSpan GetOffset()
        {
            // Add current runtime + time added before pause
            return this.CurrentOffset + this.PauseTime;
        }

        /// <summary>
        /// Draws game context
        /// </summary>
        public void Draw()
        {
            // Update time
            UpdateOffset();

            // Draw all onscreen objects
            foreach (IChotaikoGameObject GameObject in OnScreenObjects)
                GameObject.Draw(this.CurrentOffset, this.Chart.ChartInfo);
        }

        /// <summary>
        /// Updates game context
        /// </summary>
        public void Update()
        {
            UpdateOffset();

            // Add ready objects to drawable
            for (int i = CurrentID; i < Chart.Objects.Count; i++)
            {
                // Object not ready
                if (!Chart.Objects[i].IsReady(this.CurrentOffset, this.Chart.ChartInfo)) break;
                else
                {
                    this.OnScreenObjects.Add(Chart.Objects[i].AsGameObject());
                    CurrentID++;
                }
            }

            // Update drawable objects and check if they're done
            for (int i = 0; i < OnScreenObjects.Count; i ++)
            {
                IChotaikoGameObject GameObject = OnScreenObjects[i];

                // Update object
                GameObject.Update(this.CurrentOffset, this.Chart.ChartInfo);

                // If it's done, remove it and register hit with play
                if(GameObject.IsDone(this.CurrentOffset))
                {
                    CurrentPlay.HitObject(GameObject.GetID(), GameObject.GetAccuracy());
                    OnScreenObjects.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Processes key input
        /// </summary>
        /// <param name="Key">Key which has been pressed</param>
        /// <returns>ID of note who ate key press</returns>
        public int OnPress(ChotaikoKey Key)
        {
            UpdateOffset();

            foreach(IChotaikoGameObject GameObject in OnScreenObjects)
            {
                if (GameObject.OnPress(this.CurrentOffset, this.Chart.ChartInfo)) return GameObject.GetID();
            }

            return -1;
        }

        /// <summary>
        /// Processes release of key
        /// </summary>
        /// <param name="NoteID">ID of note who originally ate key</param>
        public void OnRelease(int NoteID)
        {

        }
    }
}
