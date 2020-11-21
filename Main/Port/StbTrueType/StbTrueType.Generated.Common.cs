// Generated by Sichem at 07.03.2020 16:58:11

using System.Runtime.InteropServices;

namespace StbTrueTypeSharp
{
    internal unsafe partial class StbTrueType
    {
        public const int STBTT_vmove = 1;
        public const int STBTT_vline = 2;
        public const int STBTT_vcurve = 3;
        public const int STBTT_vcubic = 4;
        public const int STBTT_PLATFORM_ID_UNICODE = 0;
        public const int STBTT_PLATFORM_ID_MAC = 1;
        public const int STBTT_PLATFORM_ID_ISO = 2;
        public const int STBTT_PLATFORM_ID_MICROSOFT = 3;
        public const int STBTT_MS_EID_UNICODE_BMP = 1;
        public const int STBTT_MS_EID_UNICODE_FULL = 10;

        [StructLayout(LayoutKind.Sequential)]
        public struct stbtt_vertex
        {
            public short x;
            public short y;
            public short cx;
            public short cy;
            public short cx1;
            public short cy1;
            public byte type;
            public byte padding;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct stbtt__point
        {
            public float x;
            public float y;
        }

        public static ushort ttUSHORT(byte* p)
        {
            return (ushort)(p[0] * 256 + p[1]);
        }

        public static short ttSHORT(byte* p)
        {
            return (short)(p[0] * 256 + p[1]);
        }

        public static uint ttULONG(byte* p)
        {
            return (uint)((p[0] << 24) + (p[1] << 16) + (p[2] << 8) + p[3]);
        }

        public static int ttLONG(byte* p)
        {
            return (int)((p[0] << 24) + (p[1] << 16) + (p[2] << 8) + p[3]);
        }

        public static int stbtt__isfont(byte* font)
        {
            if (((((((font)[0]) == ('1')) && (((font)[1]) == (0))) && (((font)[2]) == (0))) && (((font)[3]) == (0))))
                return (int)(1);
            if (((((((font)[0]) == ("typ1"[0])) && (((font)[1]) == ("typ1"[1]))) && (((font)[2]) == ("typ1"[2]))) && (((font)[3]) == ("typ1"[3]))))
                return (int)(1);
            if (((((((font)[0]) == ("OTTO"[0])) && (((font)[1]) == ("OTTO"[1]))) && (((font)[2]) == ("OTTO"[2]))) && (((font)[3]) == ("OTTO"[3]))))
                return (int)(1);
            if (((((((font)[0]) == (0)) && (((font)[1]) == (1))) && (((font)[2]) == (0))) && (((font)[3]) == (0))))
                return (int)(1);
            if (((((((font)[0]) == ("true"[0])) && (((font)[1]) == ("true"[1]))) && (((font)[2]) == ("true"[2]))) && (((font)[3]) == ("true"[3]))))
                return (int)(1);
            return (int)(0);
        }

        public static int stbtt_GetFontOffsetForIndex_internal(byte* font_collection, int index)
        {
            if ((stbtt__isfont(font_collection)) != 0)
                return (int)((index) == (0) ? 0 : -1);
            if (((((((font_collection)[0]) == ("ttcf"[0])) && (((font_collection)[1]) == ("ttcf"[1]))) && (((font_collection)[2]) == ("ttcf"[2]))) && (((font_collection)[3]) == ("ttcf"[3]))))
            {
                if (((ttULONG(font_collection + 4)) == (0x00010000)) || ((ttULONG(font_collection + 4)) == (0x00020000)))
                {
                    int n = (int)(ttLONG(font_collection + 8));
                    if ((index) >= (n))
                        return (int)(-1);
                    return (int)(ttULONG(font_collection + 12 + index * 4));
                }
            }

            return (int)(-1);
        }

        public static int stbtt_GetNumberOfFonts_internal(byte* font_collection)
        {
            if ((stbtt__isfont(font_collection)) != 0)
                return (int)(1);
            if (((((((font_collection)[0]) == ("ttcf"[0])) && (((font_collection)[1]) == ("ttcf"[1]))) && (((font_collection)[2]) == ("ttcf"[2]))) && (((font_collection)[3]) == ("ttcf"[3]))))
            {
                if (((ttULONG(font_collection + 4)) == (0x00010000)) || ((ttULONG(font_collection + 4)) == (0x00020000)))
                {
                    return (int)(ttLONG(font_collection + 8));
                }
            }

            return (int)(0);
        }

        public static void stbtt_setvertex(stbtt_vertex* v, byte type, int x, int y, int cx, int cy)
        {
            v->type = (byte)(type);
            v->x = ((short)(x));
            v->y = ((short)(y));
            v->cx = ((short)(cx));
            v->cy = ((short)(cy));
        }

        public static void stbtt__add_point(stbtt__point* points, int n, float x, float y)
        {
            if (points == null)
                return;
            points[n].x = (float)(x);
            points[n].y = (float)(y);
        }

        public static int stbtt__tesselate_curve(stbtt__point* points, int* num_points, float x0, float y0, float x1, float y1, float x2, float y2, float objspace_flatness_squared, int n)
        {
            float mx = (float)((x0 + 2 * x1 + x2) / 4);
            float my = (float)((y0 + 2 * y1 + y2) / 4);
            float dx = (float)((x0 + x2) / 2 - mx);
            float dy = (float)((y0 + y2) / 2 - my);
            if ((n) > (16))
                return (int)(1);
            if ((dx * dx + dy * dy) > (objspace_flatness_squared))
            {
                stbtt__tesselate_curve(points, num_points, (float)(x0), (float)(y0), (float)((x0 + x1) / 2.0f), (float)((y0 + y1) / 2.0f), (float)(mx), (float)(my), (float)(objspace_flatness_squared), (int)(n + 1));
                stbtt__tesselate_curve(points, num_points, (float)(mx), (float)(my), (float)((x1 + x2) / 2.0f), (float)((y1 + y2) / 2.0f), (float)(x2), (float)(y2), (float)(objspace_flatness_squared), (int)(n + 1));
            }
            else
            {
                stbtt__add_point(points, (int)(*num_points), (float)(x2), (float)(y2));
                *num_points = (int)(*num_points + 1);
            }

            return (int)(1);
        }

        public static void stbtt__tesselate_cubic(stbtt__point* points, int* num_points, float x0, float y0, float x1, float y1, float x2, float y2, float x3, float y3, float objspace_flatness_squared, int n)
        {
            float dx0 = (float)(x1 - x0);
            float dy0 = (float)(y1 - y0);
            float dx1 = (float)(x2 - x1);
            float dy1 = (float)(y2 - y1);
            float dx2 = (float)(x3 - x2);
            float dy2 = (float)(y3 - y2);
            float dx = (float)(x3 - x0);
            float dy = (float)(y3 - y0);
            float longlen = (float)(CRuntime.sqrt((double)(dx0 * dx0 + dy0 * dy0)) + CRuntime.sqrt((double)(dx1 * dx1 + dy1 * dy1)) + CRuntime.sqrt((double)(dx2 * dx2 + dy2 * dy2)));
            float shortlen = (float)(CRuntime.sqrt((double)(dx * dx + dy * dy)));
            float flatness_squared = (float)(longlen * longlen - shortlen * shortlen);
            if ((n) > (16))
                return;
            if ((flatness_squared) > (objspace_flatness_squared))
            {
                float x01 = (float)((x0 + x1) / 2);
                float y01 = (float)((y0 + y1) / 2);
                float x12 = (float)((x1 + x2) / 2);
                float y12 = (float)((y1 + y2) / 2);
                float x23 = (float)((x2 + x3) / 2);
                float y23 = (float)((y2 + y3) / 2);
                float xa = (float)((x01 + x12) / 2);
                float ya = (float)((y01 + y12) / 2);
                float xb = (float)((x12 + x23) / 2);
                float yb = (float)((y12 + y23) / 2);
                float mx = (float)((xa + xb) / 2);
                float my = (float)((ya + yb) / 2);
                stbtt__tesselate_cubic(points, num_points, (float)(x0), (float)(y0), (float)(x01), (float)(y01), (float)(xa), (float)(ya), (float)(mx), (float)(my), (float)(objspace_flatness_squared), (int)(n + 1));
                stbtt__tesselate_cubic(points, num_points, (float)(mx), (float)(my), (float)(xb), (float)(yb), (float)(x23), (float)(y23), (float)(x3), (float)(y3), (float)(objspace_flatness_squared), (int)(n + 1));
            }
            else
            {
                stbtt__add_point(points, (int)(*num_points), (float)(x3), (float)(y3));
                *num_points = (int)(*num_points + 1);
            }

        }

        public static stbtt__point* stbtt_FlattenCurves(stbtt_vertex* vertices, int num_verts, float objspace_flatness, int** contour_lengths, int* num_contours, void* userdata)
        {
            stbtt__point* points = null;
            int num_points = (int)(0);
            float objspace_flatness_squared = (float)(objspace_flatness * objspace_flatness);
            int i = 0;
            int n = (int)(0);
            int start = (int)(0);
            int pass = 0;
            for (i = (int)(0); (i) < (num_verts); ++i)
            {
                if ((vertices[i].type) == (STBTT_vmove))
                    ++n;
            }
            *num_contours = (int)(n);
            if ((n) == (0))
                return null;
            *contour_lengths = (int*)(CRuntime.malloc((ulong)(sizeof(int) * n)));
            if ((*contour_lengths) == (null))
            {
                *num_contours = (int)(0);
                return null;
            }

            for (pass = (int)(0); (pass) < (2); ++pass)
            {
                float x = (float)(0);
                float y = (float)(0);
                if ((pass) == (1))
                {
                    points = (stbtt__point*)(CRuntime.malloc((ulong)(num_points * sizeof(stbtt__point))));
                    if ((points) == (null))
                        goto error;
                }
                num_points = (int)(0);
                n = (int)(-1);
                for (i = (int)(0); (i) < (num_verts); ++i)
                {
                    switch (vertices[i].type)
                    {
                        case STBTT_vmove:
                            if ((n) >= (0))
                                (*contour_lengths)[n] = (int)(num_points - start);
                            ++n;
                            start = (int)(num_points);
                            x = (float)(vertices[i].x);
                            y = (float)(vertices[i].y);
                            stbtt__add_point(points, (int)(num_points++), (float)(x), (float)(y));
                            break;
                        case STBTT_vline:
                            x = (float)(vertices[i].x);
                            y = (float)(vertices[i].y);
                            stbtt__add_point(points, (int)(num_points++), (float)(x), (float)(y));
                            break;
                        case STBTT_vcurve:
                            stbtt__tesselate_curve(points, &num_points, (float)(x), (float)(y), (float)(vertices[i].cx), (float)(vertices[i].cy), (float)(vertices[i].x), (float)(vertices[i].y), (float)(objspace_flatness_squared), (int)(0));
                            x = (float)(vertices[i].x);
                            y = (float)(vertices[i].y);
                            break;
                        case STBTT_vcubic:
                            stbtt__tesselate_cubic(points, &num_points, (float)(x), (float)(y), (float)(vertices[i].cx), (float)(vertices[i].cy), (float)(vertices[i].cx1), (float)(vertices[i].cy1), (float)(vertices[i].x), (float)(vertices[i].y), (float)(objspace_flatness_squared), (int)(0));
                            x = (float)(vertices[i].x);
                            y = (float)(vertices[i].y);
                            break;
                    }
                } (*contour_lengths)[n] = (int)(num_points - start);
            }
            return points;
        error:
            ;
            CRuntime.free(points);
            CRuntime.free(*contour_lengths);
            *contour_lengths = null;
            *num_contours = (int)(0);
            return (null);
        }

        public static void stbtt_FreeBitmap(byte* bitmap, void* userdata)
        {
            CRuntime.free(bitmap);
        }
    }
}