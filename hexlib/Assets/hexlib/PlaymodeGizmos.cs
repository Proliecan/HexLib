using UnityEngine;

namespace hexlib
{
    public class PlaymodeGizmos
    {
        static Material _lineMaterial;
        public static Material GetLineMaterial()
        {
            if (!_lineMaterial)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                _lineMaterial = new Material(shader);
                _lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                _lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                _lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                _lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                _lineMaterial.SetInt("_ZWrite", 0);
            }
            return _lineMaterial;
        }
        
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            Material mat = GetLineMaterial();
            mat.SetPass(0);
            GL.PushMatrix();
            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex3(start.x, start.y, start.z);
            GL.Vertex3(end.x, end.y, end.z);
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawPolygon(Vector3[] points, Color color)
        {
            Material mat = GetLineMaterial();
            mat.SetPass(0);
            GL.PushMatrix();
            GL.Begin(GL.LINES);
            GL.Color(color);
            for (int i = 0; i < points.Length; i++)
            {
                GL.Vertex3(points[i].x, points[i].y, points[i].z);
                GL.Vertex3(points[(i + 1) % points.Length].x, points[(i + 1) % points.Length].y, points[(i + 1) % points.Length].z);
            }
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawSolidPolygon(Vector3[] points, Color color)
        {
            Material mat = GetLineMaterial();
            mat.SetPass(0);
            GL.PushMatrix();
            GL.Begin(GL.TRIANGLES);
            GL.Color(color);
            for (int i = 0; i < points.Length; i++)
            {
                GL.Vertex3(points[i].x, points[i].y, points[i].z);
                GL.Vertex3(points[(i + 1) % points.Length].x, points[(i + 1) % points.Length].y, points[(i + 1) % points.Length].z);
                GL.Vertex3(points[(i + 2) % points.Length].x, points[(i + 2) % points.Length].y, points[(i + 2) % points.Length].z);
            }
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawCircle(Vector3 center, float radius, Color color)
        {
            Material mat = GetLineMaterial();
            mat.SetPass(0);
            GL.PushMatrix();
            GL.Begin(GL.LINES);
            GL.Color(color);
            for (int i = 0; i < 360; i++)
            {
                float angle = i * Mathf.Deg2Rad;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                GL.Vertex3(center.x + x, center.y + y, center.z);
                GL.Vertex3(center.x + Mathf.Cos(angle + Mathf.Deg2Rad) * radius, center.y + Mathf.Sin(angle + Mathf.Deg2Rad) * radius, center.z);
            }
            GL.End();
            GL.PopMatrix();
        }
        
        public static void DrawString(Vector3 position, string text, int fontSize, Color color)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = color;
            style.fontSize = fontSize;
            style.alignment = TextAnchor.MiddleCenter;
            Vector3 screenPos = Camera.current.WorldToScreenPoint(position);
            screenPos.y = Screen.height - screenPos.y;
            GUI.Label(new Rect(screenPos.x, screenPos.y, 100, 100), text, style);
        }
    }
}
