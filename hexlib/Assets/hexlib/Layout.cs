﻿using System;
using UnityEngine;
using Object = System.Object;

namespace hexlib
{
    public class Layout
    {
        public class Orientation
        {
            public Orientation(double f0, double f1, double f2, double f3, double b0, double b1, double b2, double b3,
                double startAngle){
                F0 = f0;
                F1 = f1;
                F2 = f2;
                F3 = f3;
                B0 = b0;
                B1 = b1;
                B2 = b2;
                B3 = b3;
                StartAngle = startAngle;
            }

            public double F0{ get; }
            public double F1{ get; }
            public double F2{ get; }
            public double F3{ get; }
            public double B0{ get; }
            public double B1{ get; }
            public double B2{ get; }
            public double B3{ get; }
            public double StartAngle{ get; }
            
            public static Orientation Pointy => new(Mathf.Sqrt(3.0f), Mathf.Sqrt(3.0f) / 2.0f, 0.0f, 3.0f / 2.0f,
                Mathf.Sqrt(3.0f) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f, 0.5f);
            public static Orientation Flat => new(3.0f / 2.0f, 0.0f, Mathf.Sqrt(3.0f) / 2.0f, Mathf.Sqrt(3.0f),
                2.0f / 3.0f, 0.0f, -1.0f / 3.0f, Mathf.Sqrt(3.0f) / 3.0f, 0.0f);
            
            // equality
            protected bool Equals(Orientation other){
                return F0.Equals(other.F0) && F1.Equals(other.F1) && F2.Equals(other.F2) && F3.Equals(other.F3) &&
                       B0.Equals(other.B0) && B1.Equals(other.B1) && B2.Equals(other.B2) && B3.Equals(other.B3) &&
                       StartAngle.Equals(other.StartAngle);
            }
            
            public override bool Equals(Object obj){
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Orientation) obj);
            }
            
            //operator overloads
            public static bool operator ==(Orientation left, Orientation right){
                return Equals(left, right);
            }
            
            public static bool operator !=(Orientation left, Orientation right){
                return !Equals(left, right);
            }
            
            // hashcode
            public override int GetHashCode(){
                unchecked{
                    var hashCode = F0.GetHashCode();
                    hashCode = (hashCode * 397) ^ F1.GetHashCode();
                    hashCode = (hashCode * 397) ^ F2.GetHashCode();
                    hashCode = (hashCode * 397) ^ F3.GetHashCode();
                    hashCode = (hashCode * 397) ^ B0.GetHashCode();
                    hashCode = (hashCode * 397) ^ B1.GetHashCode();
                    hashCode = (hashCode * 397) ^ B2.GetHashCode();
                    hashCode = (hashCode * 397) ^ B3.GetHashCode();
                    hashCode = (hashCode * 397) ^ StartAngle.GetHashCode();
                    return hashCode;
                }
            }
        }
        
        /// <summary>
        ///  Creates a new layout with the given orientation, size and origin
        /// </summary>
        /// <param name="orientation">
        /// The orientation of the layout. Either Layout.Pointy or Layout.Flat
        /// </param>
        /// <param name="size">
        ///  The size of the hexes in the layout (width and height)
        /// </param>
        /// <param name="origin">
        ///  The origin of the layout (center of the hex at 0,0)
        /// </param>
        public Layout(Orientation orientation, Vector2 size, Vector2 origin){
            HexOrientation = orientation;
            Size = size;
            Origin = origin;
        }

        public Orientation HexOrientation{ get; }
        public Vector2 Size{ get; }
        public Vector2 Origin{ get; }

        /// <summary>
        ///  Converts a hex to world coordinates
        /// </summary>
        /// <param name="hex">
        ///  The hex to convert
        /// </param>
        /// <returns>
        ///  The world coordinates of the center of the hex
        /// </returns>
        public Vector2 HexToWorld(Hex hex){
            var x = (HexOrientation.F0 * hex.Q + HexOrientation.F1 * hex.R) * Size.x;
            var y = (HexOrientation.F2 * hex.Q + HexOrientation.F3 * hex.R) * Size.y;
            return new Vector2((float) x, (float) y) + Origin;
        }

        public FractionalHex WorldToHex(Vector2 world){
            var pt = new Vector2(world.x - Origin.x, world.y - Origin.y);
            var q = HexOrientation.B0 * pt.x + HexOrientation.B1 * pt.y;
            var r = HexOrientation.B2 * pt.x + HexOrientation.B3 * pt.y;
            return new FractionalHex(q, r);
        }
        
        // corners
        public Vector2 HexCornerOffset(int corner){
            var angle = 2.0d * Math.PI * (HexOrientation.StartAngle - corner) / 6.0d;
            return new Vector2((float) (Size.x * Math.Cos(angle)), (float) (Size.y * Math.Sin(angle)));
        }
        
        public Vector2[] HexCorners(Hex hex){
            var corners = new Vector2[6];
            var center = HexToWorld(hex);
            for (var i = 0; i < 6; i++){
                var offset = HexCornerOffset(i);
                corners[i] = new Vector2(center.x + offset.x, center.y + offset.y);
            }

            return corners;
        }
    }
}
