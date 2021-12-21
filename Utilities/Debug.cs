using Godot;
using System;
using System.Collections.Generic;

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

        public void DisplayPathFinding(bool display)
        {
            debugOverlay.DisplayPathfinding(display);
        }

        public void AddPathFindingPath(string entityName, List<Vector2> pathfindingPath)
        {
            debugOverlay.AddPathfindingPath(entityName, pathfindingPath);
        }

        public void RemovePathfindingPath(string entityName)
        {
            debugOverlay.RemovePathfindingPath(entityName);
        }

    }
}