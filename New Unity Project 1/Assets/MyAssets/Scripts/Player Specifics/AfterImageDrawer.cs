using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageDrawer : MonoBehaviour {

	public float DistanceBetweenDraws = 0.3f;

	private bool active = false;
	[HideInInspector]
	public MeshFilter meshFilter;
	private Mesh mesh;

	protected List<Matrix4x4[]> positions;
	protected List<Vector4[]> colors;
	protected List<MaterialPropertyBlock> propBlocks;
	protected int count = 0;

	public Material material;

	public float StartTransparency = 1;
	public float EndTransparency = 0.08f;

	public Color StartColor {
		get {
			return startColor;
		}
		set {
			startColor = value;
			startColor.a = StartTransparency;
		}
	}
	private Color startColor = Color.red;

	public Color EndColor {
		get {
			return endColor;
		}
		set {
			endColor = value;
			endColor.a = EndTransparency;
		}
	}
	private Color endColor = Color.blue;

	public float MarginToLast;

	private GameObject tempObject;

	void Awake() {
		if (meshFilter != null) {
			mesh = meshFilter.mesh;
		}
		ProtectedAwake ();
	}

	protected virtual void ProtectedAwake() {
	}

	private void Start () {
		positions = new List<Matrix4x4[]> ();
		colors = new List<Vector4[]> ();
		propBlocks = new List<MaterialPropertyBlock> ();

		tempObject = new GameObject();
		tempObject.hideFlags |= HideFlags.HideInHierarchy;

		ProtectedStart ();
	}

	protected virtual void ProtectedStart() {
		
	}

	protected virtual void Update () {
		if (active && positions != null && count > 0) {
			int tempCount = count;
			for(int i = 0; tempCount > 0; ++i, tempCount -= 1023) {
				Graphics.DrawMeshInstanced (mesh: mesh, 
					submeshIndex: 0, 
					material: material, 
					matrices: positions[i], 
					count: Mathf.Min(tempCount, 1023),
					properties: propBlocks[i]);
			}
		}
	}

	public void SetMesh(Mesh m) {
		mesh = m;
	}

	public bool MeshIsSet() {
		return mesh != null;
	}

	public void SetMaterial(Material m) {
		material = m;
	}

	public void StartDrawing() {
		active = true;
	}

	public void StopDrawing() {
		active = false;
	}

	protected void AllocateInstanceMemory(int size) {
		//Make sure we have space for the points
		for (int i = positions.Count; i <= (size-1) / 1023; ++i) {
			positions.Add (new Matrix4x4[1023]);
			colors.Add(new Vector4[1023]);
			propBlocks.Add (new MaterialPropertyBlock());
		}
	}

	protected void CalculateAndSetColors() {
		int tempCount = count;
		for(int i = 0; tempCount > 0; ++i, tempCount -= 1023) {
			int startIndex = i * 1023;
			for (int j = 0; j < Mathf.Min(tempCount, 1023); ++j) {
				colors [i][j] = ColorManipulation.LerpColorsCorrected (StartColor, EndColor, (float)(startIndex+j)/(float)count);
			}
			propBlocks[i].Clear ();
			propBlocks[i].SetVectorArray ("_Color", colors[i]);
		}
	}

	private Vector3 previousEnd = Vector3.zero;
	public void UpdatePositions(Transform baseTransform, Vector3 startPosition, Vector3 endPosition) {
		if (Vector3.Magnitude(previousEnd-endPosition) < 0.0001f ) {
			return;
		}
		previousEnd = endPosition;

		Vector3 direction = (endPosition - startPosition).normalized;
		float distance = Vector3.Magnitude (endPosition - startPosition);
		count = Mathf.FloorToInt((distance - MarginToLast) / DistanceBetweenDraws);

		if (count <= 0) {
			return;
		}

		AllocateInstanceMemory (count); //Could possibly reuse a single list as memory, since we're setting it every frame. But RewindDrawer keeps them, so easier this way.

		//Why is copying transforms such a bitch
		tempObject.transform.position = baseTransform.position;
		tempObject.transform.rotation = baseTransform.rotation;
		tempObject.transform.localScale = baseTransform.localScale;
		Transform tempTransform = tempObject.transform;
		tempTransform.position = startPosition;

		int tempCount = count;
		for(int i = 0; tempCount > 0; ++i, tempCount -= 1023) {
			for (int j = 0; j < Mathf.Min(tempCount, 1023); ++j) {
				tempTransform.position += direction*DistanceBetweenDraws;
				positions [i][j] = tempTransform.localToWorldMatrix;
				colors [i][j] = ColorManipulation.LerpColorsCorrected (StartColor, EndColor, (float)(i*1023+j)/(float)count);
			}
		}

		CalculateAndSetColors ();
	}
}
