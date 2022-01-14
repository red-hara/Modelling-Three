using Godot;

class SpatialPart : Spatial, Part
{
    public override void _Process(float delta)
    {

    }

    public Pose4 GetOrigin()
    {
        return new Pose4(Translation, RotationDegrees.z);
    }
}
