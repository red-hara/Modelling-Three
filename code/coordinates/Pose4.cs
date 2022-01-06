using Godot;

public struct Pose4
{
    public Vector3 position;
    public float rotation;

    public Pose4(Vector3 position = new Vector3(), float rotation = 0)
    {
        this.position = position;
        this.rotation = rotation;
    }

    public Pose4 Inverse()
    {
        return new Pose4(
            -position.Rotated(new Vector3(0, 0, 1), -Mathf.Deg2Rad(rotation)),
            -rotation
        );
    }

    public Pose4 Interpolate(Pose4 other, float t)
    {
        Vector3 position = this.position.LinearInterpolate(other.position, t);
        float deltaAngle = Mathf.Wrap(
            other.rotation - rotation,
            -180,
            180
        );
        return new Pose4(
            position,
            rotation + deltaAngle * t
        );
    }

    public static Pose4 operator *(Pose4 self, Pose4 other)
    {
        return new Pose4(
            self.position + other.position.Rotated(
                new Vector3(0, 0, 1),
                Mathf.Deg2Rad(self.rotation)
            ),
            self.rotation + other.rotation
        );
    }

    public override string ToString()
    {
        return string.Format(
            "[{0}, {1}]",
            position,
            Mathf.Rad2Deg(rotation)
        );
    }
}
