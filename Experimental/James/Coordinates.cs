using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHaMPWorkbench.Experimental.James
{
    public class Coordinate
    {
        private double _latitude, _longitude;


        public Coordinate(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        /// <summary>
        /// Latitude in degrees
        /// </summary>
        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                if (value > 90)
                {
                    throw new ArgumentOutOfRangeException(String.Format("value: {0}", value), "Latitude value cannot be greater than 90.");
                }
                if (value < -90)
                {
                    throw new ArgumentOutOfRangeException(String.Format("value: {0}", value), "Latitude value cannot be less than -90.");
                }
            }
        }

        /// <summary>
        /// Longitude in degrees
        /// </summary>
        public Double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                if (value > 180)
                {
                    throw new ArgumentOutOfRangeException(String.Format("value: {0}", value), "Longitude value cannot be greater than 180.");
                }
                if (value < -180)
                {
                    throw new ArgumentOutOfRangeException(String.Format("value: {0}", value), "Longitude value cannot be less than -180.");
                }
            }
        }


        public enum UnitsOfLength
        {
            Kilometer,
            Meter
        }

        public enum CardinalPoints
        {
            N,
            NE,
            E,
            SE,
            S,
            SW,
            W,
            NW
        }

        /// <summary>
        /// Calculate distance between two coordinates (latitude, longitude) using the Haversine Formula (http://www.movable-type.co.uk/scripts/latlong.html)
        /// </summary>
        /// <param name="coordinateA">first coordinate</param>
        /// <param name="coordinateB">second coordinate</param>
        /// <param name="unitOfLength">sets the return value unit of length</param>
        /// <returns></returns>
        public static double Distance(Coordinate coordinateA, Coordinate coordinateB, UnitsOfLength unitOfLength)
        {
            double theta = coordinateA.Longitude - coordinateB.Longitude;
            double distance = Math.Sin(coordinateA.Latitude.ToRadians()) * Math.Sin(coordinateB.Latitude.ToRadians()) +
                              Math.Cos(coordinateA.Latitude.ToRadians()) * Math.Cos(coordinateB.Latitude.ToRadians()) *
                              Math.Cos(theta.ToRadians());
            distance = Math.Acos(distance);
            distance = distance.ToDegrees();
            distance = distance * 60 * 1.1515;

            if (unitOfLength == UnitsOfLength.Kilometer)
            {
                distance = distance * 1.60934;
            }
            else if (unitOfLength == UnitsOfLength.Meter)
            {
                distance = distance * 1609.34;
            }

            return distance;
        }

        /// <summary>
        /// Takes two coordinates (latitude, longitude) in degrees and calculates the bearing in degrees
        /// </summary>
        /// <param name="coordinateA"></param>
        /// <param name="coordinateB"></param>
        /// <returns>double value in degrees, from 0 to 360</returns>
        public static double Bearing(Coordinate coordinateA, Coordinate coordinateB)
        {
            double longitudeDifference = (coordinateB.Longitude - coordinateA.Longitude).ToRadians();

            double y = Math.Sin(longitudeDifference) * Math.Cos(coordinateB.Latitude.ToRadians());
            double x = Math.Cos(coordinateA.Latitude.ToRadians()) * Math.Sin(coordinateB.Latitude.ToRadians()) -
                       Math.Sin(coordinateA.Latitude.ToRadians()) * Math.Cos(coordinateB.Latitude.ToRadians()) * Math.Cos(longitudeDifference);

            return (Math.Atan2(y, x).ToDegrees() + 360) % 360;
        }



    }
}
