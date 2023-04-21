using System;
using UnityEngine;
using UnityEngine.TestTools;

namespace hexlib
{
    [ExecuteInEditMode]
    public class HexGrid : MonoBehaviour
    {
        public Vector2 size = new(10, 10);
        
        //todo dynamically in start
        public Layout HexLayout = new(Layout.Orientation.Pointy, new Vector2(10, 10), new Vector2(0, 0));

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
            
            // render grid
            for (int q = 0; q < size.x; q++){
                for (int r = 0; r < size.y; r++){
                    var corners = HexLayout.HexCorners(new Hex(q, r));
                    Gizmos.color = Color.red;
                    for (int i = 0; i < corners.Length; i++){
                        Gizmos.DrawLine(corners[i], corners[(i + 1) % corners.Length]);
                    }
                }
            }
        }
    }

    //todo custom editor for hexgrid
}
