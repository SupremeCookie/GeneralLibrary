using UnityEngine;

public class TexturedQuadComponent : MonoBehaviour
{
	private const float DEFAULT_QUAD_SIZE = 5.0f;

	[SerializeField] private Material quadMaterial;

	private float quadSize;
	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;

	public void Init(float quadSize = DEFAULT_QUAD_SIZE)
	{
		this.quadSize = quadSize;

		meshFilter = gameObject.GetOrAddComponent<MeshFilter>();
		meshRenderer = gameObject.GetOrAddComponent<MeshRenderer>();

		TryCreateQuad();
	}

	public void DrawTexture(Texture2D texture)
	{
		ApplyTextureMap(texture);
	}

	private void TryCreateQuad()
	{
		var sharedMesh = meshFilter.sharedMesh;

		bool shouldMakeQuad = sharedMesh == null || sharedMesh.vertexCount == 0;
		if (shouldMakeQuad)
		{
			Mesh mesh = new Mesh();
			mesh.name = "Textured_Quad";

			Vector3[] quadVertices = new Vector3[4]
			{
					new Vector3(0,0,0),
					new Vector3(quadSize,0,0),
					new Vector3(0,quadSize,0),
					new Vector3(quadSize,quadSize,0),
			};
			mesh.vertices = quadVertices;

			int[] quadTris = new int[6]
			{
					0,2,1,
					2,3,1,
			};
			mesh.triangles = quadTris;

			Vector3[] quadNormals = new Vector3[4]
			{
					Vector3.back,
					Vector3.back,
					Vector3.back,
					Vector3.back,
			};
			mesh.normals = quadNormals;

			Vector2[] quadUVs = new Vector2[4]
			{
					new Vector2(0,0),
					new Vector2(1,0),
					new Vector2(0,1),
					new Vector2(1,1),
			};
			mesh.uv = quadUVs;

			meshFilter.mesh = mesh;
		}
	}

	private void ApplyTextureMap(Texture2D texture)
	{
		var sharedMat = meshRenderer.sharedMaterial;
		if (sharedMat == null)
		{
			Debug.Assert(quadMaterial != null, "No Material assigned, please do");
			sharedMat = new Material(quadMaterial);

			meshRenderer.material = sharedMat;
		}

		meshRenderer.sharedMaterial.mainTexture = texture;

#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(sharedMat);
#endif
	}
}