using UnityEngine;

public class TexturedQuadComponent : MonoBehaviour
{
	private const float DEFAULT_QUAD_SIZE = 5.0f;

#pragma warning disable 0649
#if !RogueLike
	[SerializeField] private Material _defaultMaterial;
#endif
#pragma warning restore 0649

	private float _quadSize;
	private MeshFilter _meshFilter;
	private MeshRenderer _meshRenderer;

	public void Init(float quadSize = DEFAULT_QUAD_SIZE)
	{
		_quadSize = quadSize;

		_meshFilter = gameObject.GetOrAddComponent<MeshFilter>();
		_meshRenderer = gameObject.GetOrAddComponent<MeshRenderer>();

		TryAddQuad();
	}

	public void Draw(Texture2D texture)
	{
		ApplyTextureMap(texture);
	}

	private void TryAddQuad()
	{
		var sharedMesh = _meshFilter.sharedMesh;

		bool shouldMakeQuad = sharedMesh == null || sharedMesh.vertexCount == 0;
		if (shouldMakeQuad)
		{
			Mesh mesh = new Mesh();
			mesh.name = "Textured_Quad";

			Vector3[] quadVertices = new Vector3[4]
			{
					new Vector3(0,0,0),
					new Vector3(_quadSize,0,0),
					new Vector3(0,_quadSize,0),
					new Vector3(_quadSize,_quadSize,0),
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

			_meshFilter.mesh = mesh;
		}
	}

	private void ApplyTextureMap(Texture2D texture)
	{
		var sharedMat = _meshRenderer.sharedMaterial;
		if (sharedMat == null)
		{
#if RogueLike
			sharedMat = new Material(IniControl.GlobalData.DefaultObjectMaterial);
#else
			Debug.Assert(_defaultMaterial != null, "No Default Material assigned, please do");
			sharedMat = new Material(_defaultMaterial);
#endif

			_meshRenderer.material = sharedMat;
		}

		_meshRenderer.sharedMaterial.mainTexture = texture;
	}
}