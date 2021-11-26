using Godot;

namespace Gerbil.BehaviourTree.Interfaces
{
    /// <summary>
    /// Base interface for all the actor that implement the behavior tree.
    /// </summary>
    interface IActor
    {
        public void Shoot(Vector2 shootingDirection);
    }
}