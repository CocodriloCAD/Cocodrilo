using Rhino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.PostProcessing
{
    public static class PostProcessingUtilities
    {
        /// <summary>
        /// Computes the length of an array.
        /// Ideal for the computation of lengths.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double GetArrayLength(double[] array)
        {
            double square = array.Sum(i => i * i);
            return Math.Sqrt(square);
        }
        /// <summary>
        /// Computes the von Mises stress from a 3 dimensional array.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double GetVonMises(double[] array)
        {
            if (array.GetLength(0) == 3)
            {
                return Math.Sqrt(array[0] * array[0] - array[0] * array[1] + array[1] * array[1] + 3 * array[2] * array[2]);
            } else
            {
                double von_mises = Math.Sqrt( 1.0/2.0 * (Math.Pow(array[0] - array[1], 2) + Math.Pow(array[1] - array[2], 2) + Math.Pow(array[0] - array[2], 2)
                        + 6 * (array[3] * array[3] + array[4] * array[4] + array[5] * array[5])) );
                return von_mises;
            }
         }

        public static Rhino.DocObjects.ObjectAttributes GetStressPatternObjectAttributes(double Stress, Rhino.Geometry.Interval StressMinMax)
        {
            var attributes = RhinoDoc.ActiveDoc.CreateDefaultAttributes();
            //if (Math.Abs(Stress) > 0.01)
            //{
            //    attributes.ObjectDecoration = (Stress > 0)
            //            ? Rhino.DocObjects.ObjectDecoration.EndArrowhead
            //            : Rhino.DocObjects.ObjectDecoration.StartArrowhead;
            //}
            attributes.ObjectColor = PostProcessingUtilities.FalseColor(Stress, StressMinMax);
            attributes.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject;
            return attributes;
        }

        public static System.Drawing.Color FalseColor(double z, Rhino.Geometry.Interval MinMax)
        {
            // Simple example of one way to change a number into a color.
            double s = MinMax.NormalizedParameterAt(z);
            s = Rhino.RhinoMath.Clamp((1 - s), 0, 1);
            double r_val = 0, g_val = 0, b_val = 0;
            double hue = s * 240;
            double c = 1.0;
            double x = (1 - Math.Abs((hue / 60) % 2 - 1));
            double m = 0.0;

            if (hue < 60)
            {
                r_val = c;
                g_val = x;
                b_val = 0;
            }
            else if (hue < 120)
            {
                r_val = x;
                g_val = c;
                b_val = 0;
            }
            else if (hue < 180)
            {
                r_val = 0;
                g_val = c;
                b_val = x;
            }
            else if (hue <= 240)
            {
                r_val = 0;
                g_val = x;
                b_val = c;
            }
            else
            {
                r_val = 0;
                g_val = 0;
                b_val = 0;
            }
            r_val += m;
            g_val += m;
            b_val += m;
            //if (s > 0.5)
            //{
            //    r_val = (int)((s-0.5) * 2 * 255);
            //    g_val = (int)((1 - s) * 2 * 255);
            //    b_val = 0;
            //}
            //else
            //{
            //    r_val = 0;
            //    g_val = (int)(s * 2 * 255);
            //    b_val = (int)((0.5-s) * 2 * 255);
            //}


            return System.Drawing.Color.FromArgb((int)(r_val * 255), (int)(g_val * 255), (int)(b_val * 255));
        }
    }
}
