using System.Collections.Generic;
using Godot;

[Tool]
public class Trace : ImmediateGeometry
{

    private int pointCount = 5_000;

    private LinkedList<Vector3> points = new LinkedList<Vector3>();

    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        Transform global = GlobalTransform;
        points.AddLast(global.origin);
        while (points.Count > pointCount)
        {
            points.RemoveFirst();
        }
        Clear();
        Begin(Mesh.PrimitiveType.LineStrip);
        foreach (Vector3 point in points)
        {
            AddVertex(global.XformInv(point));
        }
        End();
    }
}
