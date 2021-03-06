﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Stellaris
{
    public static class Helper
    {
        static int seed = new Random().Next();
        static int textureID;
        /// <summary>
        /// 如果Value不在(min, max)，返回最接近的一段的值
        /// </summary>
        public static float TryGetValue(float min, float max, float value)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        /// <summary>
        /// 如果可以，赋值
        /// </summary>
        public static void TrySetValue<T>(this IList<T> list, int index, T value)
        {
            if (index < 0) return;
            if (index >= list.Count) return;
            list[index] = value;
        }
        /// <summary>
        /// 周期函数，取不到max的值
        /// </summary>
        public static float GetLoopedValue(float min, float max, float value)
        {
            if (value < min) return value % (max - min) + max;
            if (value >= max) return value % (max - min) + min;
            return value;
        }

        public unsafe static void C_memcpy(void* a, void* b, long size)
        {
            byte* aptr = (byte*)a;
            byte* bptr = (byte*)b;
            for (long i = 0; i < size; i++) *aptr++ = *bptr++;
        }
        public static T[] InitializeArrayFromValue<T>(T value, int length)
        {
            T[] result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = value;
            }
            return result;
        }
        public static T[] InitializeArrayFromValue<T>(Func<int, T> func, int length)
        {
            T[] result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = func(i);
            }
            return result;
        }
        public static int[] FromAToB(int a, int b)
        {
            int[] result = new int[b - a];
            for (int i = a; i < b; i++)
            {
                result[i - a] = i;
            }
            return result;
        }
        public static Vector2 CenterTypeToVector2(CenterType centerType)
        {
            switch (centerType)
            {
                case CenterType.TopLeft:
                    return new Vector2(0f, 0f);
                case CenterType.TopCenter:
                    return new Vector2(0.5f, 0.5f);
                case CenterType.TopRight:
                    return new Vector2(1f, 0f);
                case CenterType.MiddleLeft:
                    return new Vector2(0f, 0.5f);
                case CenterType.MiddleCenter:
                    return new Vector2(0.5f, 0.5f);
                case CenterType.MiddleRight:
                    return new Vector2(1f, 0.5f);
                case CenterType.BottomLeft:
                    return new Vector2(0f, 1f);
                case CenterType.BottomCenter:
                    return new Vector2(0.5f, 1f);
                case CenterType.BottomRight:
                    return new Vector2(1f, 1f);
                default:
                    return default;
            }
        }
        /// <summary>
        /// 随机角度的向量
        /// </summary>
        public static Vector2 RandomAngleVec(float length, Vector2 center = default, float min = 0f, float max = 6.283f)
        {
            Random random = new Random(seed);
            float radian = min.LerpTo(max, (float)random.NextDouble(), 1f);
            float c = (float)Math.Cos(radian);
            float s = (float)Math.Sin(radian);
            seed = random.Next();
            return new Vector2(c * length, s * length) + center;
        }
        /// <summary>
        /// 随机向量
        /// </summary>
        public static Vector2 RandomVec(Vector2 min, Vector2 max)
        {
            Random random = new Random(seed);
            float x = min.X.LerpTo(max.X, (float)random.NextDouble(), 1);
            seed = random.Next();
            random = new Random(seed);
            float y = min.Y.LerpTo(max.Y, (float)random.NextDouble(), 1);
            seed = random.Next();
            return new Vector2(x, y);
        }
        public static byte[] IntToByteArray(int i)
        {
            byte[] result = new byte[4];
            result[0] = (byte)((i >> 24) & 0xFF);
            result[1] = (byte)((i >> 16) & 0xFF);
            result[2] = (byte)((i >> 8) & 0xFF);
            result[3] = (byte)(i & 0xFF);
            return result;
        }
        public static int ByteArrayToInt(byte[] bytes)
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                int shift = (3 - i) * 8;
                value += (bytes[i] & 0x000000FF) << shift;
            }
            return value;
        }
        /// <summary>
        /// 求两向量角度平均值
        /// </summary>
        public static float AngleBetween(Vector2 a, Vector2 b)
        {
            return (a + b).Angle();
        }
        public static Color ToColor(this Vector4 vec)
        {
            return new Color(vec.X, vec.Y, vec.Z, vec.W);
        }
        /// <summary>
        /// 线性插值
        /// </summary>
        public static float Lerp(float a, float b, float progress, float max)
        {
            return progress / max * b + (max - progress) / max * a;
        }
        /// <summary>
        /// 线性插值
        /// </summary>
        public static float LerpTo(this float a, float b, float progress, float max)
        {
            return progress / max * b + (max - progress) / max * a;
        }
        /// <summary>
        /// 线性插值
        /// </summary>
        public static Vector2 Lerp(Vector2 a, Vector2 b, float progress, float max)
        {
            return progress / max * b + (max - progress) / max * a;
        }
        public static Vector2[] CatmullRom(Vector2[] data, int precision)
        {
            precision += 1;
            Vector2[] result = new Vector2[(data.Length - 1) * precision + 1];
            float delta = 1f / precision;
            for (int i = 0; i < data.Length - 1; i++)
            {
                Vector2 v1 = data.TryGetValue(i - 1);
                Vector2 v2 = data[i];
                Vector2 v3 = data.TryGetValue(i + 1);
                Vector2 v4 = data.TryGetValue(i + 2);
                for (int j = 0; j < precision; j++)
                {
                    result[i * precision + j] = Vector2.CatmullRom(v1, v2, v3, v4, delta * j);
                }
            }
            result[result.Length - 1] = data[data.Length - 1];
            return result;
        }
        public static Vector2[] GetEllipse(float a, float b, float startRadian, float endRadian, float deltaRadian, Vector2 center = default)
        {
            Vector2[] result = new Vector2[(int)Math.Ceiling(Math.Abs((endRadian - startRadian) / deltaRadian))];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Vector2((float)Math.Cos(i * deltaRadian + startRadian) * a, (float)Math.Sin(i * deltaRadian + startRadian) * b) + center;
            }
            return result;
        }
        /// <summary>
        /// From Internet
        /// </summary>
        public static Color HslToRgb(float hue, float saturation, float lightness)
        {
            float num4 = 0f;
            float num5 = 0f;
            float num6 = 0f;
            float num = hue % 360f;
            float num2 = saturation / 100f;
            float num3 = lightness / 100f;
            if (num2 == 0.0)
            {
                num4 = num3;
                num5 = num3;
                num6 = num3;
            }
            else
            {
                float d = num / 60f;
                int num11 = (int)Math.Floor(d);
                float num10 = d - num11;
                float num7 = num3 * (1f - num2);
                float num8 = num3 * (1f - (num2 * num10));
                float num9 = num3 * (1f - (num2 * (1f - num10)));
                switch (num11)
                {
                    case 0:
                        num4 = num3;
                        num5 = num9;
                        num6 = num7;
                        break;
                    case 1:
                        num4 = num8;
                        num5 = num3;
                        num6 = num7;
                        break;
                    case 2:
                        num4 = num7;
                        num5 = num3;
                        num6 = num9;
                        break;
                    case 3:
                        num4 = num7;
                        num5 = num8;
                        num6 = num3;
                        break;
                    case 4:
                        num4 = num9;
                        num5 = num7;
                        num6 = num3;
                        break;
                    case 5:
                        num4 = num3;
                        num5 = num7;
                        num6 = num8;
                        break;
                }
            }
            return new Color(num4, num5, num6);
        }
        public static int[] SplitNum(int num)
        {
            int length = 0;
            for (int i = 1; ; i *= 10)
            {
                if (i > num) break;
                length += 1;
            }
            int[] cache = new int[length];
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                cache[i] = num;
                num /= 10;
            }
            for (int i = 1; i < length; i++)
            {
                result[i] = cache[length - i - 1] - cache[length - i] * 10;
            }
            result[0] = cache[length - 1];
            return result;
        }
        public static Color[] CutBitmap(Color[] bitmap, int bitmapWidth, int startX, int startY, int width, int height)
        {
            Color[] result = new Color[width * height];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    result[j * width + i] = bitmap[(j + startY) * bitmapWidth + startX + i];
                }
            }
            return result;
        }
        public static byte[] CutBitmap(byte[] bitmap, int bitmapWidth, int startX, int startY, int width, int height)
        {
            byte[] result = new byte[width * height];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    result[j * width + i] = bitmap[(j + startY) * bitmapWidth + startX + i];
                }
            }
            return result;
        }
        public static int GetTextureID(Texture2D texture2D)
        {
            if (texture2D.Tag == null)
            {
                texture2D.Tag = textureID;
                textureID++;
            }
            //Debug.WriteLine(textureID);
            return (int)texture2D.Tag;
        }
        public static Vector3 Vector3(double x, double y, double z)
        {
            return new Vector3((float)x, (float)y, (float)z);
        }
    }
}
