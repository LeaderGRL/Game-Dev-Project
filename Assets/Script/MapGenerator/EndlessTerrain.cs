using System;
using UnityEngine;
using System.Collections.Generic;

namespace Script.MapGenerator
{
    public class EndlessTerrain : MonoBehaviour
    {
        private const float maxViewDst = 300;
        [SerializeField] private Transform viewer;

        public static Vector2 viewerPosition;

        private int chunkSize;
        private int chunkVisibleInViewDst;
        private Dictionary<Vector2, TerrainChunk> terrainChunks = new();
        private List<TerrainChunk> _terrainChunksVisibleLastUpdate = new();

        private void Start()
        {
            chunkSize = MapGenerator.mapChunkSize - 1;
            chunkVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
        }

        private void Update()
        {
            viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
            UpdateVisibleChunk();
        }

        void UpdateVisibleChunk()
        {
            for (int i = 0; i < _terrainChunksVisibleLastUpdate.Count; i++)
            {
                _terrainChunksVisibleLastUpdate[i].SetVisible(false);
            }

            _terrainChunksVisibleLastUpdate.Clear();
            int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
            int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);
            for (int yOffset = -chunkVisibleInViewDst; yOffset <= chunkVisibleInViewDst; yOffset++)
            {
                for (int xOffset = -chunkVisibleInViewDst; xOffset <= chunkVisibleInViewDst; xOffset++)
                {
                    Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
                    if (terrainChunks.ContainsKey(viewedChunkCoord))
                    {
                        terrainChunks[viewedChunkCoord].UpdateTerrainChunk();
                        if (terrainChunks[viewedChunkCoord].IsVisible())
                        {
                            _terrainChunksVisibleLastUpdate.Add(terrainChunks[viewedChunkCoord]);
                        }
                    }
                    else
                    {
                        terrainChunks.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, transform));
                    }
                }
            }
        }

        public class TerrainChunk
        {
            private GameObject meshObject;
            private Vector2 position;
            private Bounds bounds;

            public TerrainChunk(Vector2 coord, int size, Transform parent)
            {
                position = coord * size;
                bounds = new Bounds(position, Vector2.one * size);
                Vector3 positionV3 = new Vector3(position.x, 0, position.y);
                meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                meshObject.transform.position = positionV3;
                meshObject.transform.localScale = Vector3.one * size / 10f;
                meshObject.transform.parent = parent;
                SetVisible(false);
            }

            public void UpdateTerrainChunk()
            {
                float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
                bool visible = viewerDstFromNearestEdge <= maxViewDst;
                SetVisible(visible);
            }

            public void SetVisible(bool visible)
            {
                meshObject.SetActive(visible);
            }

            public bool IsVisible()
            {
                return meshObject.activeSelf;
            }
        }
    }
}