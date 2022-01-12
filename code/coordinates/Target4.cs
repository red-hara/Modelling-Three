public struct Target4
{
    public Pose4 pose;
    public int flangeRevolutions;

    public Target4(Pose4 pose, int flangeRevolutions)
    {
        this.pose = pose;
        this.flangeRevolutions = flangeRevolutions;
    }

    public static Target4 operator *(Target4 target, Pose4 pose)
    {
        return new Target4(target.pose * pose, target.flangeRevolutions);
    }

    public static Target4 operator *(Pose4 pose, Target4 target)
    {
        return new Target4(pose * target.pose, target.flangeRevolutions);
    }
}
