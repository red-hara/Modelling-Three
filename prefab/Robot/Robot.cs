using Godot;

[Tool]
public class Robot : Spatial, Controllable
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
        get => Mathf.Rad2Deg(targetGeneralized.a);
        set => targetGeneralized.a = Mathf.Deg2Rad(value);
    }

    [Export]
    public float B
    {
        get => Mathf.Rad2Deg(targetGeneralized.b);
        set => targetGeneralized.b = Mathf.Deg2Rad(value);
    }
    [Export]
    public float C
    {
        get => Mathf.Rad2Deg(targetGeneralized.c);
        set => targetGeneralized.c = Mathf.Deg2Rad(value);
    }
    [Export]
    public float D
    {
        get => Mathf.Rad2Deg(targetGeneralized.d);
        set => targetGeneralized.d = Mathf.Deg2Rad(value);
    }

    private Generalized4 generalized = new Generalized4();
    private Generalized4 targetGeneralized = new Generalized4();

    [Export]
    public NodePath end;
    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        UpdateGeneralized(delta);
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
            if (!end.IsEmpty())
            {
                Spatial endSpatial = GetNode<Spatial>(end);
                Pose4 position = CalculateForward(generalized).pose;
                endSpatial.Translation = position.position;
                endSpatial.RotationDegrees = new Vector3(
                    0,
                    0,
                    position.rotation
                );
            }
        }
    }

    private void UpdateGeneralized(float delta)
    {
        Generalized4 velocities = MaximumJointVelocity();
        for (int i = 0; i < 4; i++)
        {
            float deltaGeneralized = Mathf.Wrap(
                targetGeneralized[i] - generalized[i],
                -Mathf.Pi,
                Mathf.Pi
            );
            if (
                Mathf.Abs(deltaGeneralized) >
                velocities[i] * delta + Mathf.Epsilon
            )
            {
                generalized[i] +=
                    Mathf.Sign(deltaGeneralized) * velocities[i] * delta;
            }
            else
            {
                generalized[i] += deltaGeneralized;
            }
        }
    }

    public Target4? SetJoints(Generalized4 generalized)
    {
        this.targetGeneralized = generalized;
        return CalculateForward(generalized);
    }

    public Target4? Forward(Generalized4 generalized)
    {
        return CalculateForward(generalized);
    }

    public Generalized4 GetCurrentJoints()
    {
        return targetGeneralized;
    }

    public Generalized4? SetPosition(Target4 position)
    {
        Generalized4? solution = CalculateInverse(position);
        if (solution is null)
        {
            return null;
        }
        targetGeneralized = solution.Value;
        return solution;
    }
    public Generalized4? Inverse(Target4 position)
    {
        return CalculateInverse(position);
    }
    public Target4 GetCurrentPosition()
    {
        return CalculateForward(targetGeneralized);
    }

    public Generalized4 MaximumJointVelocity()
    {
        return new Generalized4(
            Mathf.Deg2Rad(130),
            Mathf.Deg2Rad(130),
            Mathf.Deg2Rad(130),
            Mathf.Deg2Rad(300)
        );
    }

    private static Target4 CalculateForward(Generalized4 generalized)
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
        Pose4 pose4 = new Pose4(
            pose.origin,
            Mathf.Rad2Deg(generalized.a + generalized.d)
        );
        int rotations = Mathf.RoundToInt(generalized.d / 2 / Mathf.Pi);
        return new Target4(pose4, rotations);
    }

    private static Generalized4? CalculateInverse(Target4 target)
    {
        float a = Mathf.Atan2(
            target.pose.position.y,
            target.pose.position.x
        );
        float projectionRadius = Mathf.Sqrt(
            Mathf.Pow(target.pose.position.x, 2) +
            Mathf.Pow(target.pose.position.y, 2)
        ) - wristProtrusion - protrusion;
        float deltaZ = target.pose.position.z - lift + wristDescent;
        float l1 = shoulderLength;
        float l2 = forearmLength;
        float p = Mathf.Sqrt(
            deltaZ * deltaZ +
            projectionRadius * projectionRadius
        );
        if (p > l1 + l2 || p < Mathf.Abs(l1 - l2))
        {
            return null;
        }
        float b =
            Mathf.Pi / 2 -
            Mathf.Acos((l1 * l1 + p * p - l2 * l2) / (2 * l1 * p)) -
            Mathf.Atan2(deltaZ, projectionRadius);
        float c = Mathf.Pi / 2 -
            Mathf.Acos((l1 * l1 + l2 * l2 - p * p) / (2 * l1 * l2)) +
            b;
        float d = Mathf.Deg2Rad(target.pose.rotation) - a;
        a = Mathf.Wrap(a, -Mathf.Pi, Mathf.Pi);
        b = Mathf.Wrap(b, -Mathf.Pi, Mathf.Pi);
        c = Mathf.Wrap(c, -Mathf.Pi, Mathf.Pi);
        d = Mathf.Wrap(d, -Mathf.Pi, Mathf.Pi) +
            2 * Mathf.Pi * target.flangeRevolutions;

        if (b > Mathf.Deg2Rad(85) || b < Mathf.Deg2Rad(-42))
        {
            return null;
        }
        if (c > Mathf.Deg2Rad(120) || c < Mathf.Deg2Rad(-20))
        {
            return null;
        }
        return new Generalized4(a, b, c, d);
    }
}
