using System.Text.RegularExpressions;
using Godot;
using Godot.Collections;

public partial class TileSelector : MeshInstance3D
{
	[Export] public Color breakColor = new(1f, 0f, 0f, 0.5f);
	[Export] public Color placeColor = new(0f, 1f, 1f, 0.5f);


	public enum SelectorMode
	{
		Break = -1,
		Place = 1,
	}
	private SelectorMode mode;
	public SelectorMode Mode
	{
		get
		{
			return mode;
		}
		set
		{
			mode = value;
			switch (mode) {
                case SelectorMode.Break: // TODO: simplify this
                {
					SetColor(breakColor);
					break;
				}
				case SelectorMode.Place:
                {
					SetColor(placeColor);
					break;
				}
			}
		}
	}


    public override void _Ready()
    {
        Mode = SelectorMode.Break;
    }


	private void SetColor(Color color)
	{
        StandardMaterial3D material = new()
        {
            ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
            Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
            AlbedoColor = color
        };
        MaterialOverride = material;
	}


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
		if (collider is VoxelTerrain)
		{
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
