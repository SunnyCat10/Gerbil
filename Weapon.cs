using System;
using System.Collections.Generic;
using System.Text;
using Godot;

namespace Gerbil
{

    //TODO: Make abstract...
    public class Weapon : KinematicBody2D
    {
        public float RateOfFire { get; protected set; }
        public int Damage { get; protected set; }
        public int ContactCost { get; protected set; }
        public Node2D WeaponNode { get; protected set; }
        public Texture WeaponTexture { get; protected set; } // Delete
        public Position2D ProjectileSpawnPoint { get; protected set; }
        public PackedScene ProjectileInstance { get; protected set; }


        // V2. 
        public Node2D CollisionBox { get; protected set; }

        public AnimatedSprite WeaponSprite { get; protected set; }


        public Weapon() { }

        public Weapon(float rateOfFire, int damage, int contactCost, Node2D weaponNode, Texture weaponTexture, Position2D projectileSpawnPoint, string projectileInstancePath)
        {
            RateOfFire = rateOfFire;
            Damage = damage;
            ContactCost = contactCost;
            WeaponNode = weaponNode;
            WeaponTexture = weaponTexture;
            ProjectileSpawnPoint = projectileSpawnPoint;
            ProjectileInstance = ResourceLoader.Load<PackedScene>(projectileInstancePath);
        }

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
