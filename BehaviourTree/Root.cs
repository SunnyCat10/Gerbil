using Godot;
using Gerbil.BehaviourTree;
using Godot.Collections;

/// <summary>
/// Base template for behaviour root nodes.
/// </summary>
public abstract class Root : Node
{
	/// <summary>
	/// Godot Dicitionary for storing info that can be read by the behavior nodes.
	/// </summary>
	protected Dictionary Blackboard { get; set; }
	
	/// <summary>
	/// Child behavior node of the root.
	/// </summary>
	protected BaseNode Child { get; set; }

	/// <summary>
	/// Actor node that can act based on the behavior tree.
	/// </summary>
	protected Node2D Actor { get; set; }

	/// <summary>
	/// Default settings for the root properties. Should be called on the OnReady method.
	/// </summary>
	protected void SetUpRoot()
    {
		Blackboard = new Dictionary();
		Child = (BaseNode)GetChild(0);
		Actor = (Node2D)GetParent();
	}

	/// <summary>
	/// Updates the blackboard with relevant information.
	/// </summary>
	protected abstract void UpdateBlackboard();
}
