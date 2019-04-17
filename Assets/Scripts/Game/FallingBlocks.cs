using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class FallingBlocks : MonoBehaviour
{
    public LevelCell[,] block;
    public int width, height;

    private Mesh currentMesh;

    public void UpdateGrid()
    {
#if UNITY_EDITOR
        if (currentMesh != null) DestroyImmediate(currentMesh);
#else
        if (currentMesh != null) Destroy(currentMesh);
#endif

        currentMesh = MakeMesh();

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.sharedMesh = currentMesh;
    }

    private Mesh MakeMesh()
    {
        return MeshUtils.CreateGridMesh(
            width, height,
            pos => block[pos.x, pos.y].type < 0,
            pos => Color.HSVToRGB(block[pos.x, pos.y].type / 4f, 1, 1));
    }
}
