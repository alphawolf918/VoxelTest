using System.Collections.Generic;
using UnityEngine;

public class MeshData
{

	private List<Vector3> _v3s = new List<Vector3>();
	private List<int> _tris = new List<int>();
	private List<Vector2> _uvs = new List<Vector2>();

	public MeshData(List<Vector3> v, List<int> t, Vector2[] u)
	{
		_v3s = v;
		_tris = t;
		_uvs = new List<Vector2>(u);
	}

	public MeshData()
	{

	}

	public void AddPos(Vector3 loc)
	{
		for (int i = 0; i < _v3s.Count; i++)
		{
			_v3s[i] = _v3s[i] + loc;
		}
	}

	public void Merge(MeshData m)
	{
		if (m._v3s.Count <= 0)
		{
			return;
		}

		if (_v3s.Count <= 0)
		{
			_v3s = m._v3s;
			_tris = m._tris;
			_uvs = m._uvs;
			return;
		}

		int c = _v3s.Count;

		_v3s.AddRange(m._v3s);

		for (int t = 0; t < m._tris.Count; t++)
		{
			_tris.Add(m._tris[t] + c);

		}

		_uvs.AddRange(m._uvs);
	}

	public Mesh ToMesh()
	{
		Mesh mesh = new Mesh();

		mesh.vertices = _v3s.ToArray();
		mesh.triangles = _tris.ToArray();
		mesh.uv = _uvs.ToArray();

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		//
		//MeshUtility.Optimize(mesh);
		//
		return mesh;
	}

}