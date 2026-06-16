using Godot;
using Godot.Collections;

public partial class TileSelector : MeshInstance3D
{
	public enum Mode
	{
		Break = -1,
		Place = 1,
	}


	public Mode mode = Mode.Break;


	public Vector3 GetTileCenteredPosition(Vector3 current)
	{
		return new Vector3(
			Mathf.Snapped(current.X - 0.5f, 1) + 0.5f,
			Mathf.Snapped(current.Y - 0.5f, 1) + 0.5f,
			Mathf.Snapped(current.Z - 0.5f, 1) + 0.5f
		);
	}


	public void OnObjectSelected(Dictionary target)
	{
		if (target.Count == 0)
		{
			Visible = false;
			return;
		}
		Node3D collider = (Node3D)target["collider"];
		if (collider is VoxelTerrain){
			Visible = true;
			Vector3 targetPosition = (Vector3)target["position"] + (int)mode * (Vector3)target["normal"] * 0.001f; // is this jank?
			Position = GetTileCenteredPosition(targetPosition);
		}
		else
		{
			Visible = false;
		}
	}
}
