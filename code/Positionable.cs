using Godot;

public interface Positionable
{
    Generalized4? SetPosition(Vector3 position);
    Generalized4? Inverse(Vector3 position);
    Vector3 GetCurrentPosition();
}
