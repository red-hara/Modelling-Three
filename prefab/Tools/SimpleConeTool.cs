using Godot;

/// <summary>This <c>Tool</c> represents simple welding mechanism with variable
/// orientation.</summary>
[Tool]
public class SimpleConeTool : Spatial, Tool
{
    /// <summary>Path to the rotation part of the tool.</summary>
    [Export]
    public NodePath rotatingPart;

    /// <summary>Internal tool angle, in degrees.</summary>
    [Export]
    public float angle;

    /// <summary>Path to the light that simulates welding.</summary>
    [Export]
    public NodePath light;

    public override void _Process(float delta)
    {
        // Extract Spatial and apply local rotation.
        Spatial part = GetNode<Spatial>(rotatingPart);
        part.RotationDegrees = new Vector3(0, 0, angle);
    }

    public Pose4 GetToolCenterPoint()
    {
        // The tool has two segments, apply multiplication to find resulting
        // Pose4
        return new Pose4(new Vector3(0, 500, 0), angle) *
            new Pose4(new Vector3(0, 500, -50), 90);
    }

    /// <summary>Turn the welding light on.</summary>
    public void TurnOn()
    {
        GetNode<Spatial>(light).Visible = true;
    }

    /// <summary>Turn the welding light off.</summary>
    public void TurnOff()
    {
        GetNode<Spatial>(light).Visible = false;
    }
}
