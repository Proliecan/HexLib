using System;
using UnityEditor;
using UnityEngine;

namespace hexlib
{
    [ExecuteInEditMode]
    public class HexGrid : MonoBehaviour
    {
        public Layout.Orientation Orientation{
            get => _layout.HexOrientation;
            set => _layout = new Layout(value, cellSize, transform.position);
        }

        public Vector2 size = new(10, 10);
        public Vector2 cellSize = new(1, 1);

        Layout _layout;
        
        public Color color = Color.white;

        public enum Shape
        {
            Rectangle,
            Parallelogram,
            Hexagon
        }

        public Shape shape = Shape.Hexagon;

        public void InstantiateLayout(){
            _layout = new Layout(Layout.Orientation.Pointy, cellSize, transform.position);
        }

        private void Awake(){
            InstantiateLayout();
        }
        
        //awake in edit mode
        private void OnEnable(){
            InstantiateLayout();
        }

        private void OnRenderObject(){
            switch (shape){
                case Shape.Rectangle:
                    DrawRectangleGrid();
                    break;
                case Shape.Parallelogram:
                    DrawParallelogramGrid();
                    break;
                case Shape.Hexagon:
                    DrawHexagonGrid();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void DrawParallelogramGrid(){
            for (int q = 0; q < size.x; q++){
                for (int r = 0; r < size.y; r++){
                    var corners = _layout.HexCorners(new Hex(q, r));
                    Vector3[] verts = new Vector3[corners.Length];
                    for (int i = 0; i < corners.Length; i++){
                        verts[i] = corners[i];
                    }
                    PlaymodeGizmos.DrawPolygon(verts, color);
                }
            }
        }
        
        private void DrawRectangleGrid(){
            for (int r = 0; r < size.y; r++){
                for (int q = -r / 2; q < size.x - r / 2; q++){
                    var corners = _layout.HexCorners(new Hex(q, r));
                    Vector3[] verts = new Vector3[corners.Length];
                    for (int i = 0; i < corners.Length; i++){
                        verts[i] = corners[i];
                    }
                    PlaymodeGizmos.DrawPolygon(verts, color);
                }
            }
        }
        
        private void DrawHexagonGrid(){
            int rad = (int)size.x;
            
            for (int q = -rad; q <= rad; q++){
                int r1 = Mathf.Max(-rad, -q - rad);
                int r2 = Mathf.Min(rad, -q + rad);
                for (int r = r1; r <= r2; r++){
                    var corners = _layout.HexCorners(new Hex(q, r));
                    Vector3[] verts = new Vector3[corners.Length];
                    for (int i = 0; i < corners.Length; i++){
                        verts[i] = corners[i];
                    }
                    PlaymodeGizmos.DrawPolygon(verts, color);
                }
            }
        }
    }
    
#if UNITY_EDITOR

    // custom editor for HexGrid
    [CustomEditor(typeof(HexGrid))]
    public class HexGridEditor : Editor
    {
        public override void OnInspectorGUI(){
            var grid = target as HexGrid;

            // call base method
            base.OnInspectorGUI();

            // orientation field from string dropdown
            string[] options ={"Pointy", "Flat"};
            int selected = grid.Orientation == Layout.Orientation.Pointy ? 0 : 1;
            selected = EditorGUILayout.Popup("Orientation", selected, options);
            grid.Orientation = selected == 0 ? Layout.Orientation.Pointy : Layout.Orientation.Flat;
        }
    }

#endif
    
}
