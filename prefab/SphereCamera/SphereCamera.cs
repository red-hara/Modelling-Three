using Godot;

public class SphereCamera : Spatial
{
    [Export]
    public NodePath camera;

    [Export]
    public float radius = 5000;
    [Export]
    public float slide = 0;

    [Export]
    public float alpha;
    [Export]
    public float beta;

    [Export]
    float angularVelocity = 45;
    [Export]
    float linearVelocity = 500;

    public override void _Process(float delta)
    {
        Camera cam = GetNode<Camera>(camera);
        cam.Translation = new Vector3(slide, 0, radius);
        RotationDegrees = new Vector3(beta, alpha, 0);

        if (Input.IsPhysicalKeyPressed(((int)KeyList.Shift)))
        {
            if (Input.IsActionPressed("camera_left"))
            {
                slide -= delta * linearVelocity;
            }
            if (Input.IsActionPressed("camera_right"))
            {
                slide += delta * linearVelocity;
            }
            if (Input.IsActionPressed("camera_up"))
            {
                radius -= delta * linearVelocity;
            }
            if (Input.IsActionPressed("camera_down"))
            {
                radius += delta * linearVelocity;
            }
        }
        else
        {
            if (Input.IsActionPressed("camera_left"))
            {
                alpha -= delta * angularVelocity;
            }
            if (Input.IsActionPressed("camera_right"))
            {
                alpha += delta * angularVelocity;
            }
            if (Input.IsActionPressed("camera_up"))
            {
                beta -= delta * angularVelocity;
            }
            if (Input.IsActionPressed("camera_down"))
            {
                beta += delta * angularVelocity;
            }
        }
    }
}
