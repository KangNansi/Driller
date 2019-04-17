using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class MeshUtils
{
    public static Mesh CreateGridMesh(int width, int height, Func<Vector2Int, bool> filter, Func<Vector2Int, Color> colorSelect)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (filter(pos)) continue;

                int count = vertices.Count;

                vertices.Add(new Vector3(x, y, 0));
                vertices.Add(new Vector3(x, y + 1, 0));
                vertices.Add(new Vector3(x + 1, y + 1, 0));
                vertices.Add(new Vector3(x + 1, y, 0));

                triangles.Add(count);
                triangles.Add(count + 1);
                triangles.Add(count + 2);

                triangles.Add(count);
                triangles.Add(count + 2);
                triangles.Add(count + 3);

                Color color = colorSelect(pos);
                colors.Add(color);
                colors.Add(color);
                colors.Add(color);
                colors.Add(color);
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors.ToArray();
        return mesh;
    }
}
