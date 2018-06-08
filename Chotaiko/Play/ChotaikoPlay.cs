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
        /// <summary>
        /// Weight to exponentiate strain average by in calculating PV
        /// </summary>
        public const double StrainWeight = 1.4;

        /// <summary>
        /// Weight to exponentiate length to to calculate length bonus
        /// </summary>
        public const double LengthWeight = 1.00002;

        /// <summary>
        /// Value to multiply final PV by
        /// </summary>
        public const double PVConstant = 10;

        /// <summary>
        /// Objects in the play
        /// </summary>
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

        /// <summary>
        /// Calculates performance value (PV) based on the accuracies of each note
        /// </summary>
        /// <returns>Performance value</returns>
        public double CalculatePerformanceValue()
        {
            // Unweighted performance value
            double UnweightedPV = 0;

            // Strain value of last object
            double Strain = 0;

            // Length of chart is difference between first and last objects
            TimeSpan ChartLength = PlayObjects[PlayObjects.Count - 1].GetOffset() - PlayObjects[0].GetOffset();

            for (int i = 0; i < PlayObjects.Count - 1; i ++)
            {
                IChotaikoPlayObject FirstObject = PlayObjects[i];
                IChotaikoPlayObject SecondObject = PlayObjects[i + 1];

                // Add accuracy to strain
                Strain += ((double)(FirstObject.GetAccuracy()) / 100);

                // Time difference between objects
                TimeSpan TimeDifference = SecondObject.GetOffset() - FirstObject.GetOffset();

                // Inverse of strain to decay
                double InverseStrain = InversePVDecay(Strain);

                // Strain at clicking of second object
                double AfterStrain = PVDecay(InverseStrain + TimeDifference.TotalMilliseconds);

                // Strain is now start of next object
                Strain = AfterStrain;

                // Accumulated PV over this interval = Integral of strain
                double AccumulatedPV = IntegratePVDecay(InverseStrain, InverseStrain + TimeDifference.TotalMilliseconds);

                // Add accumulated PV to total unweighted PV
                UnweightedPV += AccumulatedPV;
            }

            double WeightedPV = WeightPV(UnweightedPV, ChartLength);

            return WeightedPV;
        }

        private double WeightPV (double UnweightedPV, TimeSpan ChartLength)
        {
            double WeightedPV = UnweightedPV;

            // Average over length
            WeightedPV = 1000 * UnweightedPV / ChartLength.TotalMilliseconds;

            // Apply strain weight
            WeightedPV = Math.Pow(WeightedPV, StrainWeight);

            // Apply length bonus
            double LengthBonus = -(Math.Pow(LengthWeight, -ChartLength.TotalMilliseconds)) + 1;
            WeightedPV *= LengthBonus;

            // Multiply by PV constant
            WeightedPV *= PVConstant;

            // Return
            return WeightedPV;
        }

        /// <summary>
        /// Returns decayed value
        /// </summary>
        /// <param name="x">Value to decay</param>
        /// <returns>Decayed value</returns>
        private double PVDecay (double x)
        {
            return Math.Pow(2, -x);
        }

        /// <summary>
        /// Returns inverse decayed value
        /// </summary>
        /// <param name="x">Value to inverse decay</param>
        /// <returns>Inverse decayed value</returns>
        private double InversePVDecay (double x)
        {
            return -Math.Log(x, 2);
        }

        /// <summary>
        /// Integrates PV decay function
        /// </summary>
        /// <param name="LeftBound">Left bound of integral</param>
        /// <param name="RightBound">Right bound of integral</param>
        /// <returns>Definite integral of PV decay unction</returns>
        private double IntegratePVDecay (double LeftBound, double RightBound)
        {
            return IntegratePVDecay(RightBound) - IntegratePVDecay(LeftBound);
        }

        /// <summary>
        /// Integrates PV decay function. No, I didn't forget + C ;)
        /// </summary>
        /// <param name="x">Value to integrate</param>
        /// <returns>Integral of decay at x</returns>
        private double IntegratePVDecay (double x)
        {
            return -Math.Pow(2, -x) / Math.Log(2);
        }
    }
}
