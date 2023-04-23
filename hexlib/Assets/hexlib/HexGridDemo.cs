using System;
using UnityEngine;
using UnityEngine.TestTools;

namespace hexlib
{
    [ExecuteInEditMode]
    public class HexGridDemo : MonoBehaviour
    {
        public Vector2 size = new(10, 10);
        public Vector2 worldPoint = new(12, 3);

        //todo dynamically in start
        public Layout HexLayout = new(Layout.Orientation.Pointy, new Vector2(1, 1), Vector2.zero);

        //render using GL
        private void OnRenderObject(){
            // render 0 0  hex
        }

        private void OnDrawGizmos(){
            // // render 0 0  hex
            // var corners = HexLayout.HexCorners(new Hex(0, 0));
            // Gizmos.color = Color.red;
            // for (int i = 0; i < corners.Length; i++){
            //     Gizmos.DrawLine(corners[i], corners[(i + 1) % corners.Length]);
            // }

            // render grid in parallelogram shape
            for (int q = 0; q < size.x; q++){
                for (int r = 0; r < size.y; r++){
                    var corners = HexLayout.HexCorners(new Hex(q, r));
                    Gizmos.color = Color.red;
                    for (int i = 0; i < corners.Length; i++){
                        Gizmos.DrawLine(corners[i], corners[(i + 1) % corners.Length]);
                    }
                }
            }

            // render grid in rectangular shape
            for (int r = 0; r < size.y; r++){
                for (int q = -r / 2; q < size.x - r / 2; q++){
                    var corners = HexLayout.HexCorners(new Hex(q, r));
                    Gizmos.color = Color.blue;
                    for (int i = 0; i < corners.Length; i++){
                        Gizmos.DrawLine(corners[i], corners[(i + 1) % corners.Length]);
                    }
                }
            }
            
            // render hexagonal grid
            int rad = (int)size.x;
            
            for (int q = -rad; q <= rad; q++){
                int r1 = Mathf.Max(-rad, -q - rad);
                int r2 = Mathf.Min(rad, -q + rad);
                for (int r = r1; r <= r2; r++){
                    var corners = HexLayout.HexCorners(new Hex(q, r));
                    Gizmos.color = Color.green;
                    for (int i = 0; i < corners.Length; i++){
                        Gizmos.DrawLine(corners[i], corners[(i + 1) % corners.Length]);
                    }
                }
            }
            
            
            // draw world point
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(worldPoint, 0.1f);
            
            // draw hex at world point
            var hex = HexLayout.WorldToHex(worldPoint);
            var corners2 = HexLayout.HexCorners(hex.round_to_hex());
            
            Gizmos.color = Color.magenta;
            for (int i = 0; i < corners2.Length; i++){
                Gizmos.DrawLine(corners2[i], corners2[(i + 1) % corners2.Length]);
            }
            
            // draw line to world point
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(HexLayout.Origin, worldPoint);
            
            // draw hexes on line
            var hexes = FractionalHex.LineDraw(new Hex(0,0), hex.round_to_hex());
            for (int i = 0; i < hexes.Length-1; i++){
                var corners3 = HexLayout.HexCorners(hexes[i]);
                Gizmos.color = Color.yellow;
                for (int j = 0; j < corners3.Length; j++){
                    Gizmos.DrawLine(corners3[j], corners3[(j + 1) % corners3.Length]);
                }
            }

        }
    }
}
