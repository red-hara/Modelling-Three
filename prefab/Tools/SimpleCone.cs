using Godot;

[Tool]
public class SimpleCone : Spatial, Tool
{
    [Export]
    public NodePath rotatingPart;

    [Export]
    public float angle;

    [Export]
    public NodePath light;

    public override void _Process(float delta)
    {
        Spatial part = GetNode<Spatial>(rotatingPart);
        part.RotationDegrees = new Vector3(0, 0, angle);
    }

    public Pose4 GetToolCenterPoint()
    {
        return new Pose4(new Vector3(0, 500, 0), angle) *
            new Pose4(new Vector3(0, 500, -50), 90);
    }

    public void TurnOn()
    {
        GetNode<Spatial>(light).Visible = true;
    }

    public void TurnOff()
    {
        GetNode<Spatial>(light).Visible = false;
    }
}
