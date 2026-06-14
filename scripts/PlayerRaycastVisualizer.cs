using Godot;
using Godot.Collections;

public partial class PlayerRaycastVisualizer : MeshInstance3D
{
	public void OnCameraRaycast(Dictionary data)
	{
		Position = (Vector3)((Dictionary)data)["position"];
	}
}
