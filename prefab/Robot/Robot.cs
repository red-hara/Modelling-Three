using Godot;

[Tool]
public class Robot : Spatial
{
    public const float lift = 800;
    public const float protrusion = 300;
    public const float shoulderLength = 1280;
    public const float forearmLength = 1350;
    public const float wristProtrusion = 260;
    public const float wristDescent = 247;

    [Export]
    public NodePath column;
    [Export]
    public NodePath shoulder;
    [Export]
    public NodePath forearm;
    [Export]
    public NodePath wrist;
    [Export]
    public NodePath flange;
    [Export]
    public NodePath connector;
    [Export]
    public NodePath wristConnector;
    [Export]
    public NodePath mover;
    [Export]
    public NodePath forearmConnector;
    [Export]
    public NodePath columnConnector;
    [Export]
    public float A
    {
        get => Mathf.Rad2Deg(generalized.a);
        set => generalized.a = Mathf.Deg2Rad(value);
    }

    [Export]
    public float B
    {
        get => Mathf.Rad2Deg(generalized.b);
        set => generalized.b = Mathf.Deg2Rad(value);
    }
    [Export]
    public float C
    {
        get => Mathf.Rad2Deg(generalized.c);
        set => generalized.c = Mathf.Deg2Rad(value);
    }
    [Export]
    public float D
    {
        get => Mathf.Rad2Deg(generalized.d);
        set => generalized.d = Mathf.Deg2Rad(value);
    }

    private Generalized4 generalized = new Generalized4();

    [Export]
    public NodePath end;
    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        float a = generalized.a;
        float b = generalized.b;
        float c = generalized.c;
        float d = generalized.d;
        GetNode<Spatial>(column).Rotation = new Vector3(0, 0, a);
        GetNode<Spatial>(shoulder).Rotation = new Vector3(0, b, 0);
        GetNode<Spatial>(forearm).Rotation = new Vector3(0, c - b, 0);
        GetNode<Spatial>(wrist).Rotation = new Vector3(0, -c, 0);
        GetNode<Spatial>(flange).Rotation = new Vector3(0, 0, d);
        GetNode<Spatial>(connector).Rotation = new Vector3(0, -b, 0);
        GetNode<Spatial>(wristConnector).Rotation = new Vector3(0, c, 0);
        GetNode<Spatial>(mover).Rotation = new Vector3(0, c, 0);
        GetNode<Spatial>(forearmConnector).Rotation = new Vector3(0, b - c, 0);
        GetNode<Spatial>(columnConnector).Rotation = new Vector3(0, b, 0);

        if (!(end is null))
        {
            Spatial endSpatial = GetNode<Spatial>(end);
            Pose4 position = Forward(generalized);
            endSpatial.Translation = position.position;
            endSpatial.Rotation = new Vector3(0, 0, position.rotation);
        }
    }

    public static Pose4 Forward(Generalized4 generalized)
    {
        Transform pose = new Transform(
            new Quat(new Vector3(0, 0, 1), generalized.a),
            new Vector3(0, 0, lift)
        ) * new Transform(
            new Quat(new Vector3(0, 1, 0), generalized.b),
            new Vector3(protrusion, 0, 0)
        ) * new Transform(
            new Quat(new Vector3(0, 1, 0), generalized.c - generalized.b),
            new Vector3(0, 0, shoulderLength)
        ) * new Transform(
            new Quat(new Vector3(0, 1, 0), -generalized.c),
            new Vector3(forearmLength, 0, 0)
        ) * new Transform(
            new Quat(new Vector3(0, 0, 1), generalized.d),
            new Vector3(wristProtrusion, 0, -wristDescent)
        );
        return new Pose4(pose.origin, generalized.a + generalized.d);
    }
}
