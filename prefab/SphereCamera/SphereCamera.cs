using Godot;

public class SphereCamera : Spatial
{
    [Export]
    public NodePath camera;

    [Export]
    public float alpha;
    [Export]
    public float beta;

    [Export]
    float angularVelocity = 15;
    [Export]
    float linearVelocity = 500;

    public override void _Process(float delta)
    {
        Camera cam = GetNode<Camera>(camera);
        cam.Translation = new Vector3(0, 0, 0);
        RotationDegrees = new Vector3(beta, alpha, 0);

        if (!Input.IsPhysicalKeyPressed(((int)KeyList.Shift)))
        {
            if (Input.IsActionPressed("camera_left"))
            {
                Translation += delta *
                    (Transform.basis.Xform(new Vector3(-linearVelocity, 0, 0)));
            }
            if (Input.IsActionPressed("camera_right"))
            {
                Translation += delta *
                    (Transform.basis.Xform(new Vector3(linearVelocity, 0, 0)));
            }
            if (Input.IsActionPressed("camera_forward"))
            {
                Translation += delta *
                    (Transform.basis.Xform(new Vector3(0, 0, -linearVelocity)));
            }
            if (Input.IsActionPressed("camera_back"))
            {
                Translation += delta *
                    (Transform.basis.Xform(new Vector3(0, 0, linearVelocity)));
            }
            if (Input.IsActionPressed("camera_up"))
            {
                Translation += delta * new Vector3(0, linearVelocity, 0);
            }
            if (Input.IsActionPressed("camera_down"))
            {
                Translation += delta * new Vector3(0, -linearVelocity, 0);
            }
        }
        else
        {
            if (Input.IsActionPressed("camera_left"))
            {
                alpha += delta * angularVelocity;
            }
            if (Input.IsActionPressed("camera_right"))
            {
                alpha -= delta * angularVelocity;
            }
            if (Input.IsActionPressed("camera_forward"))
            {
                beta += delta * angularVelocity;
            }
            if (Input.IsActionPressed("camera_back"))
            {
                beta -= delta * angularVelocity;
            }
        }
    }
}
