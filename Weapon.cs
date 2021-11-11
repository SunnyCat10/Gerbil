using Godot;

namespace Gerbil
{
    /// <summary>
    /// The base template of all the weapons in game.
    /// </summary>
    public abstract class Weapon : KinematicBody2D
    {
        /// <summary>
        /// Weapon rate of fire as cooldown between the attacks in seconds.
        /// </summary>
        public float RateOfFire { get; protected set; }

        /// <summary>
        /// Default damage value of the weapon.
        /// </summary>
        public int Damage { get; protected set; }

        /// <summary>
        /// Default contact cost value of the weapon.
        /// </summary>
        public int ContactCost { get; protected set; }

        /// <summary>
        /// Weapon node root instance.
        /// </summary>
        public Node2D RootNode { get; protected set; }

        /// <summary>
        /// Icon that will be displayed when the weapon is picked up or on the floor.
        /// </summary>
        public Texture Icon { get; protected set; }

        /// <summary>
        /// Animated sprite of the weapon which includes the different animations.
        /// </summary>
        public AnimatedSprite WeaponAnimatedSprite { get; protected set; }

        /// <summary>
        /// Collision boundry of the weapon.
        /// </summary>
        public Node2D CollisionBox { get; protected set; }

        /// <summary>
        /// Ranged projectiles spawn point, for example on the edge of the barrel of a gun.
        /// </summary>
        public Position2D ProjectileSpawnPoint { get; protected set; }

        /// <summary>
        /// Projectile packed scene, used to initiate projectile instances. 
        /// </summary>
        public PackedScene ProjectileInstance { get; protected set; }
        
        /// <summary>
        /// Picks up the weapon from the map scene and add it to the player weapon inventory.
        /// </summary>
        /// <param name="picker"> Entity that picked up the weapon, usually it will be the player. </param>
        /// <returns> Class instance that inherits the abstract weapon. </returns>
        public Weapon OnPickUp(Node2D picker)
        {
            CollisionBox.SetDeferred("disabled", true);
            GlobalPosition = new Vector2();
            GetParent().RemoveChild(this);
            picker.AddChild(this);
            return this;
        }
    }
}
