using Godot;
using Godot.Collections;

public partial class TileSelector : MeshInstance3D
{
	public enum Mode
	{
		Break,
		Place,
	}

	public Mode mode = Mode.Break;

	public void OnCameraRaycast(Dictionary data)
	{
		Position = (Vector3)data["position"];
	}
}
