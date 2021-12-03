using Godot;
using Godot.Collections;

namespace Gerbil.BehaviourTree.Actions
{
	/// <summary>
	/// Simple behavior node that prints a message to the console.
	/// </summary>
	public class Print : BaseNode
	{
		[Export]
		private string message = "Default";

		public override State Tick(Node2D actor, Dictionary blackboard)
		{
			GD.Print(message);
			return State.Succeeded;
		}
	}
}
