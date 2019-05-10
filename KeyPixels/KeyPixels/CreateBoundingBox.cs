using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace KeyPixels
{
    class CreateBoundingBox
    {
        public BoundingBox bBox;
        private Vector2[] pVec2;
        

        public CreateBoundingBox(Model model, Matrix meshTransform)
        {
            Vector3 meshMax = new Vector3(float.MinValue);
            Vector3 meshMin = new Vector3(float.MaxValue);
            pVec2 = new Vector2[4];
            
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

                    VertexPositionNormalTexture[] vertexData = new VertexPositionNormalTexture[part.NumVertices];
                    part.VertexBuffer.GetData(part.VertexOffset * stride, vertexData, 0, part.NumVertices, stride);

                    Vector3 vertPosition = new Vector3();

                    for (int i = 0; i < vertexData.Length; i++)
                    {
                        vertPosition = vertexData[i].Position;

                        meshMin = Vector3.Min(meshMin, vertPosition);
                        meshMax = Vector3.Max(meshMax, vertPosition);
                    }
                }
                meshMin = Vector3.Transform(meshMin, meshTransform);
                meshMax = Vector3.Transform(meshMax, meshTransform);
            }
            bBox = new BoundingBox(meshMin, meshMax);
            bBoxToVec2();
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
