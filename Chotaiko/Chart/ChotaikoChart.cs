using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chotaiko.Chart
{
    /// <summary>
    /// An object representing a song chart.
    /// </summary>
    class ChotaikoChart
    {
        /// <summary>
        /// The list of chart objects.
        /// </summary>
        public List<IChotaikoChartObject> Objects { get; }
        
        /// <summary>
        /// Information on this chart
        /// </summary>
        public ChotaikoChartInfo ChartInfo { get; }

        /// <summary>
        /// Creates new chart given file stream
        /// </summary>
        /// <param name="ChartFileStream">File stream to read from</param>
        public ChotaikoChart(StreamReader ChartFileStream)
        {
            // If something bad goes wrong, tell the user something is wrong!
            try
            {
                this.Objects = new List<IChotaikoChartObject>();

                // Read first line
                String AccString = ChartFileStream.ReadLine();

                // Convert to double and parse standard deviation
                double AccValue = Convert.ToDouble(AccString);

                // Read second line
                String SpeedString = ChartFileStream.ReadLine();

                // Convert to double and parse standard deviation
                double SpeedValue = Convert.ToDouble(SpeedString);

                // Read third line
                String BPMString = ChartFileStream.ReadLine();

                // Convert to double and parse standard deviation
                double BPM = Convert.ToDouble(BPMString);

                // Create chart info
                ChartInfo = new ChotaikoChartInfo(AccValue, BPM, SpeedValue);

                int NoteID = 0; // ID of note
                double LastOffset = -1;

                // Parse each object
                while (!ChartFileStream.EndOfStream)
                {
                    // Type of note
                    String NoteType = ChartFileStream.ReadLine();
                    if (NoteType.Equals("Beat"))
                    {
                        // Offset in map
                        String NoteOffsetString = ChartFileStream.ReadLine();
                        double NoteOffset = Convert.ToDouble(NoteOffsetString);

                        // If beats are not in order, the chart is in the wrong format
                        if (NoteOffset <= LastOffset) throw new ArgumentException("Beats not in order.");
                        LastOffset = NoteOffset;

                        // Theta value
                        String ThetaString = ChartFileStream.ReadLine();
                        double Theta = Convert.ToDouble(ThetaString);

                        // Create a new note
                        ChotaikoChartBeat Note = new ChotaikoChartBeat(NoteID, Theta, TimeSpan.FromTicks((long)(ChartInfo.BeatTime.Ticks * NoteOffset)));
                        this.Objects.Add(Note);

                        NoteID++;
                    }
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e.StackTrace);
#endif
                throw new ArgumentException("Chart Parse Error!");

            }
        }
    }
}
