using Godot;

public interface Jointable
{
    Vector3? SetJoints(Generalized4 generalized);

    Vector3? Forward(Generalized4 generalized);

    Generalized4 GetCurrentJoints();
}
