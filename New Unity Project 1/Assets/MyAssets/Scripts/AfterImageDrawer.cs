using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageDrawer : MonoBehaviour {

	public float DistanceBetweenEach = 1;

	private bool active = false;
	private Matrix4x4[] positions;
	private Vector4[] colors;
	private Mesh mesh;

	public Material material;
	private MaterialPropertyBlock propBlock;

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
	private Color startColor;

	public Color EndColor {
		get {
			return endColor;
		}
		set {
			endColor = value;
			endColor.a = EndTransparency;
		}
	}
	private Color endColor;

	public float MarginToLast;

	private GameObject tempObject;

	void Start () {
		propBlock = new MaterialPropertyBlock();
		positions = null;
		tempObject = new GameObject();
		tempObject.hideFlags |= HideFlags.HideInHierarchy;
	}

	void Update () {
		if (active && positions != null ) {
			Graphics.DrawMeshInstanced(mesh: mesh, submeshIndex: 0, material: material, matrices: positions, count: positions.Length, properties: propBlock);
		}
	}

	public void SetMesh(Mesh m) {
		mesh = m;
	}

	public void SetMaterial(Material m) {
		material = m;
	}

	public void StartDrawing(Mesh m) {
		mesh = m;
		active = true;
	}

	public void StopDrawing() {
		active = false;
		positions = null;
	}
		
	public void UpdatePositions(Transform baseTransform, Vector3 startPosition, Vector3 endPosition) {
		Vector3 direction = (endPosition - startPosition).normalized;
		float distance = Vector3.Magnitude (endPosition - startPosition);
		int count = Mathf.FloorToInt((distance - MarginToLast) / DistanceBetweenEach);

		if (count <= 0) {
			return;
		}

		//Why is copying transforms such a bitch
		tempObject.transform.position = baseTransform.position;
		tempObject.transform.rotation = baseTransform.rotation;
		tempObject.transform.localScale = baseTransform.localScale;
		Transform tempTransform = tempObject.transform;

		tempTransform.position = startPosition;
		positions = new Matrix4x4[count];
		colors = new Vector4[count];
		for (int i = 0; i < count; ++i) {
			tempTransform.position += direction*DistanceBetweenEach;
			positions [i] = tempTransform.localToWorldMatrix;
			colors[i] = ColorManipulation.LerpColorsCorrected (StartColor, EndColor, (float)i/(float)count);
		}

		/*
		//Fade in last one
		distance = Vector3.Magnitude(endPosition-tempTransform.position);
		tempTransform.position += direction*DistanceBetweenEach;
		positions [count-1] = tempTransform.localToWorldMatrix;
		colors [count - 1] = EndColor;
		colors [count - 1].w *= DistanceBetweenEach/distance;
		*/

		propBlock.Clear ();
		propBlock.SetVectorArray ("_Color", colors);
	}
}
