﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
[AddComponentMenu("UI/Effects/Gradient")]
public class GradientScript : BaseMeshEffect {
	
	public Color topColor;

	public Color bottomColor;

	public override void ModifyMesh(VertexHelper vh){
		if (!this.IsActive())
			return;
		List<UIVertex> vertexList = new List<UIVertex>();
		vh.GetUIVertexStream(vertexList);
		ModifyVertices(vertexList);
		vh.Clear();
		vh.AddUIVertexTriangleStream(vertexList);
	}
	public void ModifyVertices(List<UIVertex> vertexList) {
		int count = vertexList.Count;
		float bottomY = vertexList[0].position.y;
		float topY = vertexList[0].position.y;
		for (int i = 1; i < count; i++) {
			float y = vertexList[i].position.y;
			if (y > topY) {
				topY = y;
			} else if (y < bottomY) {
				bottomY = y;
			}
		}
		float uiElementHeight = topY - bottomY;
		for (int i = 0; i < count; i++) {
			UIVertex uiVertex = vertexList[i];
			uiVertex.color = Color.Lerp(bottomColor, topColor, (uiVertex.position.y - bottomY) / uiElementHeight);
			vertexList[i] = uiVertex;
		}
	}
}
