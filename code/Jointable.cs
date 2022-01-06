using Godot;

public interface Jointable
{
    Target4? SetJoints(Generalized4 generalized);

    Target4? Forward(Generalized4 generalized);

    Generalized4 GetCurrentJoints();
}
