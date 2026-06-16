using Godot;
using Godot.Collections;

public partial class PlayerRaycastVisualizer : MeshInstance3D
{
	public void OnCameraRaycast(Dictionary data)
	{
		if (data.Count > 0)
		{
			Position = (Vector3)data["position"];
			Visible = true;
		}
		else
		{
			Visible = false;
		}
	}
}
