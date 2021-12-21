using Godot;
using System;

namespace Gerbil.Utilities
{
    public class Debug
    {
        public static Debug Instance
        {
            get
            {
                if (instance == null)
                    instance = new Debug();
                return instance;
            }
        }

        private static Debug instance = null;
        private DebugOverlay debugOverlay;

        private Debug() { }

        public void SetupGraphicOverly(Node tree)
        {
            if (debugOverlay == null)
                debugOverlay = new DebugOverlay();
            tree.AddChild(debugOverlay);
            debugOverlay.Position = Vector2.Zero;
        }




    }
}