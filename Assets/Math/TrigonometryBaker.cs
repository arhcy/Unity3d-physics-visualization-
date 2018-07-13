// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

using System;
using System.Collections.Generic;

namespace Artics.Math
{
    /// <summary>
    /// Simple solution to cache Math.Sin and Math.Cos values with listed proximity. </br>
    /// You cas use different sets of proximities.
    /// </summary>
    public class TrigonometryBaker
    {
        /// <summary>
        /// stores sin and cos values grouped by proximity
        /// </summary>
        public Dictionary<uint, double[][]> BakingArray;

        public TrigonometryBaker()
        {
            BakingArray = new Dictionary<uint, double[][]>();
        }

        /// <summary>
        /// inits a set of sin and cos values with listed proximity and stores it in <see cref="BakingArray"/> dictionary.
        /// </summary>
        /// <param name="proximity">Proximity calculates by formula: (Math.PI * 2) / proximity</param>
        /// <returns></returns>
        public double[][] InitProximity(uint proximity)
        {
            double[][] values = new double[2][];
            values[0] = new double[proximity];
            values[1] = new double[proximity];

            double angle = 0;
            double angleStep = (System.Math.PI * 2) / proximity;

            for (int i = 0; i < proximity; i++)
            {
                values[0][i] = System.Math.Sin(angle);
                values[1][i] = System.Math.Cos(angle);

                angle += angleStep;
            }

            BakingArray.Add(proximity, values);

            return values;
        }

        /// <summary>
        /// Gets calculated proximity or creates new set.
        /// </summary>
        /// <param name="proximity">Proximity calculates by formula: (Math.PI * 2) / proximity</param>
        /// <returns></returns>
        public double[][] GetProximityBake(uint proximity)
        {
            double[][] values = null;

            BakingArray.TryGetValue(proximity, out values);

            if (values == null)
                values = InitProximity(proximity);

            return values;
        }
    }
}
