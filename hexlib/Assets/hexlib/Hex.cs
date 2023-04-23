using System;
using UnityEngine;

namespace hexlib
{
    public class Hex
    {
        public Hex(int q, int r){
            Q = q;
            R = r;
        }

        public int Q{ get; }
        public int R{ get; }
        public int S => -(Q + R);

        public Vector3Int CubeCoordinates => new Vector3Int(Q, R, S);
        public Vector2Int AxialCoordinates => new Vector2Int(Q, R);

        // equality
        public static bool operator ==(Hex a, Hex b){
            if (ReferenceEquals(a, null))
                return ReferenceEquals(b, null);

            else if (ReferenceEquals(b, null))
                return false;

            else if (ReferenceEquals(a, b))
                return true;

            else
                return a.Q == b.Q && a.R == b.R;
        }

        public static bool operator !=(Hex a, Hex b){
            if (ReferenceEquals(a, null))
                return !ReferenceEquals(b, null);

            else if (ReferenceEquals(b, null))
                return true;

            else if (ReferenceEquals(a, b))
                return false;

            else
                return a.Q != b.Q || a.R != b.R;
        }

        public override bool Equals(object obj){
            // reusing the == operator
            return this == (Hex) obj;
        }

        // hashcode
        public override int GetHashCode(){
            return Q.GetHashCode() ^ R.GetHashCode();
        }

        // distance
        public static int Distance(Hex a, Hex b){
            return (Mathf.Abs(a.Q - b.Q) + Mathf.Abs(a.R - b.R) + Mathf.Abs(a.S - b.S)) / 2;
        }

        public int distanceFrom(Hex other){
            return Distance(this, other);
        }

        // neighbors
        public enum Direction
        {
            NE = 0,
            E = 1,
            SE = 2,
            SW = 3,
            W = 4,
            NW = 5
        }

        public static Hex Neighbor(Hex hex, Direction direction){
            switch (direction){
                case Direction.NE:
                    return new Hex(hex.Q + 1, hex.R);
                case Direction.E:
                    return new Hex(hex.Q + 1, hex.R - 1);
                case Direction.SE:
                    return new Hex(hex.Q, hex.R - 1);
                case Direction.SW:
                    return new Hex(hex.Q - 1, hex.R);
                case Direction.W:
                    return new Hex(hex.Q - 1, hex.R + 1);
                case Direction.NW:
                    return new Hex(hex.Q, hex.R + 1);
                default:
                    return null;
            }
        }

        public static Hex[] Neighbors(Hex hex){
            Hex[] neighbors = new Hex[6];
            for (int i = 0; i < 6; i++){
                neighbors[i] = Neighbor(hex, (Direction) i);
            }

            return neighbors;
        }

        public Hex direction(Direction direction){
            return Neighbor(this, direction);
        }

        public Hex[] directions(){
            return Neighbors(this);
        }
        
        
    }

    public class FractionalHex
    {
        public FractionalHex(double q, double r){ 
            Q = q;
            R = r;
        }

        public double Q{ get; }
        public double R{ get; }
        public double S => -(Q + R);
        
        /// <summary>
        /// ATTENTION: DOUBLES ARE ROUNDED TO FLOATS HERE!
        /// Some precision might be lost!
        /// </summary>
        public Vector3 CubeCoordinates => new Vector3((float)Q, (float)R, (float)S);
        
        /// <summary>
        /// ATTENTION: DOUBLES ARE ROUNDED TO FLOATS HERE!
        /// Some precision might be lost!
        /// 
        /// </summary>
        public Vector2 AxialCoordinates => new Vector2((float)Q, (float)R);

        /// <summary>
        /// Rounds a FractionalHex to the nearest Hex using mathematical rounding.
        /// </summary>
        /// <param name="fractionalHex">The FractionalHex to round</param>
        /// <returns>The Hex it lies in</returns>
        public static Hex Hex_Round(FractionalHex fractionalHex){
            int q = (int) Math.Round(fractionalHex.Q);
            int r = (int) Math.Round(fractionalHex.R);

            return new Hex(q, r);
        }
        
        public Hex round_to_hex(){
            return Hex_Round(this);
        }
        
        // lerping
        static double Lerp(double a, double b, double t){
            return a * (1 - t) + b * t;
        }
        
        public static FractionalHex Lerp(Hex a, Hex b, double t){
            return new FractionalHex(Lerp(a.Q, b.Q, t), Lerp(a.R, b.R, t));
        }
        
        // line drawing
        public static Hex[] LineDraw(Hex a, Hex b){
            int N = a.distanceFrom(b);
            Hex[] results = new Hex[N + 1];
            double step = 1.0 / Math.Max(N, 1);
            for (int i = 0; i <= N; i++){
                results[i] = Hex_Round(Lerp(a, b, step * i));
            }
            
            return results;
        }
        
    }
}
