using Godot;

namespace Gerbil.BehaviourTree.Interfaces
{
    /// <summary>
    /// Base interface for all the actor that implement the behavior tree.
    /// </summary>
    interface IActor
    {
        public void Move(Vector2 direction);

        public void Shoot(Vector2 shootingDirection);

        public void Attack(int attackNumber, Vector2 attackingDirection);
    }
}