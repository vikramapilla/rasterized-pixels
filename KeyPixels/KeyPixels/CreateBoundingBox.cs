﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace KeyPixels
{
    class CreateBoundingBox
    {
        public BoundingBox bBox;
        private Vector2[] pVec2;
        
        
        public CreateBoundingBox(Model model, Matrix meshTransform)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), meshTransform);
                        
                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            // Create and return bounding box
            bBox = new BoundingBox(min, max);
        }

        public Vector2[] getVec2ar()
        {
            return pVec2;
        }

        public void calcPointsBoxXZ(ref Matrix MM)
        {
            bBoxToVec2();
            Vector2[] temp = new Vector2[4];
            for (int i = 0; i < 4; ++i)
            {
                FastCalcMono3D.SmartMatrixVec2_XZ(ref pVec2[i], ref MM, ref temp[i]);
                temp[i].X += MM.M41;
                temp[i].Y += MM.M43;
            }
            pVec2 = temp;
        }

        private void bBoxToVec2()
        {
            pVec2[0].X = bBox.Min.X;
            pVec2[0].Y = bBox.Min.Z;
            pVec2[1].X = -bBox.Min.X;
            pVec2[1].Y = -bBox.Max.Z;
            pVec2[2].X = bBox.Max.X;
            pVec2[2].Y = bBox.Max.Z;
            pVec2[3].X = -bBox.Max.X;
            pVec2[3].Y = -bBox.Min.Z;
        }
    }
}
