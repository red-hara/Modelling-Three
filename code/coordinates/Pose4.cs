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
            -position.Rotated(new Vector3(0, 0, 1), -rotation),
            -rotation
        );
    }

    public static Pose4 operator *(Pose4 self, Pose4 other)
    {
        return new Pose4(
            self.position + other.position.Rotated(
                new Vector3(0, 0, 1),
                self.rotation
            ),
            self.rotation * other.rotation
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
