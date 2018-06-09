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

                double AccValue = 0;
                double SpeedValue = 0;

                // Parse properties
                while (true)
                {
                    String Line = ChartFileStream.ReadLine();

                    String[] Words = Line.Split(' ');

                    // Empty line
                    if (Words.Length == 0) break;

                    String Properties = Words[0];

                    // If notes, exit properties loop
                    if (Properties.Equals("Notes:")) break;

                    // No value
                    if (Words.Length < 2) break;

                    String Value = Words[1];

                    // Set accuracy value
                    if (Properties.Equals("AccValue:")) AccValue = Convert.ToDouble(Value);

                    // Set speed value
                    else if (Properties.Equals("SpeedValue:")) SpeedValue = Convert.ToDouble(Value);
                }

                // Create chart info
                ChartInfo = new ChotaikoChartInfo(AccValue, SpeedValue);

                // Current note ID
                int NoteID = 0;

                // Offset of previous note
                TimeSpan LastOffset = TimeSpan.Zero;

                // Time between beats and offset
                TimeSpan? BeatTime = null;
                TimeSpan? Offset = null;

                // Parse each object
                while (!ChartFileStream.EndOfStream)
                {
                    // Type of note
                    String NoteString = ChartFileStream.ReadLine();

                    String[] NoteParams = NoteString.Split(' ');

                    if (NoteParams[0].Equals("Beat"))
                    {
                        // Check parameter size
                        if (NoteParams.Length != 3) throw new ArgumentException("Invalid param count");

                        // Offset in map
                        String NoteOffsetString = NoteParams[1];
                        double NoteOffsetValue = Convert.ToDouble(NoteOffsetString);

                        // Calculate beat location
                        TimeSpan NoteOffset = Offset.Value + TimeSpan.FromTicks((long)(BeatTime.Value.Ticks * NoteOffsetValue));

                        // If beats are not in order, the chart is in the wrong format
                        if (NoteOffset < LastOffset) throw new ArgumentException("Beats not in order.");
                        LastOffset = NoteOffset;

                        // Theta value
                        String ThetaString = NoteParams[2];
                        double Theta = Convert.ToDouble(ThetaString);

                        // Create a new note
                        ChotaikoChartBeat Note = new ChotaikoChartBeat(NoteID, Theta, NoteOffset);
                        this.Objects.Add(Note);

                        NoteID++;
                    }

                    if (NoteParams[0].Equals("Strum"))
                    {
                        // Check parameter size
                        if (NoteParams.Length != 2) throw new ArgumentException("Invalid param count");

                        // Offset in map
                        String NoteOffsetString = NoteParams[1];
                        double NoteOffsetValue = Convert.ToDouble(NoteOffsetString);

                        // Calculate beat location
                        TimeSpan NoteOffset = Offset.Value + TimeSpan.FromTicks((long)(BeatTime.Value.Ticks * NoteOffsetValue));

                        // If beats are not in order, the chart is in the wrong format
                        if (NoteOffset < LastOffset) throw new ArgumentException("Beats not in order.");
                        LastOffset = NoteOffset;

                        // Create a new note
                        ChotaikoChartStrum Note = new ChotaikoChartStrum(NoteID, NoteOffset);
                        this.Objects.Add(Note);

                        NoteID++;
                    }

                    if (NoteParams[0].Equals("BPM"))
                    {
                        // Check parameter size
                        if (NoteParams.Length != 3) throw new ArgumentException("Invalid param count");

                        // Offset in map
                        String OffsetString = NoteParams[1];
                        Offset = TimeSpan.FromMilliseconds(Convert.ToInt32(OffsetString));

                        // BPM in map
                        String BPMString = NoteParams[2];
                        double BPM = Convert.ToDouble(BPMString);
                        BeatTime = TimeSpan.FromSeconds(60 / BPM);

                        // Update Chart BPM
                        ChartInfo.UpdateBPM(BPM);
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
