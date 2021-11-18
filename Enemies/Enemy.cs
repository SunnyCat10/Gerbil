using System;
using System.Collections.Generic;
using System.Text;
using Godot;

namespace Gerbil.Enemies
{
    public abstract class Enemy : KinematicBody2D
    {
        public int Health { get; protected set; }

        protected CollisionShape2D collisionShape;
        protected AnimatedSprite animatedSprite;
        protected string deathAnimationName;

        public void OnHit(int damage)
        {
            Health -= damage;
            if (Health <= 0)
                OnDeath();
        }

        public async void OnDeath()
        {
            collisionShape.SetDeferred("disabled", true);
            animatedSprite.Play(deathAnimationName);
            await ToSignal(animatedSprite, "animation_finished");
            QueueFree();
        }
    }
}
