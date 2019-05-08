using Microsoft.Xna.Framework;

namespace KeyPixels
{
    class FastCalcMono3D
    {
        public static void SmartMatrixVec3NotY(ref Vector3 V3_in, ref Matrix MM, ref Vector3 V3_out)
        {
            refe(ref V3_in.X, ref V3_in.Z, ref MM.M11, ref MM.M13, ref MM.M31, ref MM.M33, ref MM.M41, ref MM.M43, ref V3_out.X, ref V3_out.Z);
        }

        public static void SmartMatrixVec3NotY(ref Vector3 V3_in, ref Matrix MM, ref Vector2 V2_out)
        {
            refe(ref V3_in.X, ref V3_in.Z, ref MM.M11, ref MM.M13, ref MM.M31, ref MM.M33, ref MM.M41, ref MM.M43, ref V2_out.X, ref V2_out.Y);
        }

        public static void SmartMatrixVec2_XZ(ref Vector2 V2_in, ref Matrix MM, ref Vector2 V2_out)
        {
            refe(ref V2_in.X, ref V2_in.Y, ref MM.M11, ref MM.M13, ref MM.M31, ref MM.M33, ref MM.M41, ref MM.M43, ref V2_out.X, ref V2_out.Y);
        }

        private static void refe(ref float X_, ref float Z_, ref float m11, ref float m13, ref float m31, ref float m33, ref float m41,
            ref float m43, ref float X, ref float Z)
        {
            X = m11 * X_ + m31 * Z_ + m41;
            Z = m13 * X_ + m33 * Z_ + m43;
        }

        public static void SmartMatrixAddTransnotY(ref Matrix MM, ref Vector3 V3)
        {
            MM.M41 += V3.X;
            MM.M43 += V3.Z;
        }

        public static void SmartMatrixAddTransnotY(ref Matrix MM, ref Vector2 V2)
        {
            MM.M41 += V2.X;
            MM.M43 += V2.Y;
        }
    }
}
