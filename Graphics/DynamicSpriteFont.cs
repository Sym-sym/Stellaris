﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stellaris.Graphics
{
    /// <summary>
    /// 动态生成字形的SpriteFont
    /// </summary>
    public class DynamicSpriteFont : IDisposable
    {
        public float height;
        public Vector2 spacing;
        public bool onlyUseUserSpcacing;
        public bool useNative;
        public IFont font;
        Dictionary<char, Glyph> Glyphs;
        GraphicsDevice graphicsDevice;
        Glyph defaultGlyph;
        Glyph defaultGlyphCn;
        /// <summary>
        /// 构建DynamicSpriteFont
        /// </summary>
        /// <param name="font">字体</param>
        /// <param name="height">高度</param>
        /// <param name="spacing">间距</param>
        public DynamicSpriteFont(GraphicsDevice graphicsDevice, IFont font, float height, Vector2 spacing = default)
        {
            Initialize(graphicsDevice, font, height, spacing, true);
        }
        /// <summary>
        /// 构建DynamicSpriteFont
        /// </summary>
        /// <param name="graphicsDevice">显卡</param>
        /// <param name="path">字体文件路径</param>
        /// <param name="height">高度</param>
        /// <param name="spacing">间距</param>
        /// <param name="useNative">使用Native代码渲染字形</param>
        public DynamicSpriteFont(GraphicsDevice graphicsDevice, string path, float height, Vector2 spacing = default, bool useNative = true)
        {
            using (FileStream fileStream = File.OpenRead(path))
            {
                Initialize(graphicsDevice, useNative ? new FontStb_Native(path) as IFont : new FontStb(path), height, spacing, useNative);
            }
        }
        /// <summary>
        /// 构建DynamicSpriteFont
        /// </summary>
        /// <param name="graphicsDevice">显卡</param>
        /// <param name="stream">字体文件流</param>
        /// <param name="height">高度</param>
        /// <param name="spacing">间距</param>
        /// <param name="useNative">使用Native代码渲染字形</param>
        public DynamicSpriteFont(GraphicsDevice graphicsDevice, Stream stream, float height, Vector2 spacing = default, bool useNative = true)
        {
            Initialize(graphicsDevice, useNative ? new FontStb_Native(stream) as IFont : new FontStb(stream), height, spacing, useNative);
        }
        private void Initialize(GraphicsDevice graphicsDevice, IFont font, float height, Vector2 spacing, bool useNative)
        {
            this.graphicsDevice = graphicsDevice;
            this.font = font;
            this.height = height;
            this.spacing = spacing;
            this.useNative = useNative;
            Glyphs = new Dictionary<char, Glyph>();
            Glyph[] Glyph = font.GetGlyphsFromCodepoint(height, new int[] { 'A', '国' }, 1f, 1f);
            defaultGlyph = Glyph[0];
            defaultGlyphCn = Glyph[1];
        }
        /// <summary>
        /// 清除所有字形
        /// </summary>
        public void ClearCache()
        {
            Glyphs.Clear();
        }
        /// <summary>
        /// 缓存字形
        /// </summary>
        /// <param name="charArray">需要缓存的字符</param>
        public void Cache(char[] charArray)
        {
            List<char> chars = charArray.ToList();
            for (int i = 0; i < chars.Count; i++)
            {
                if (Glyphs.ContainsKey(chars[i]) || chars.IndexOf(chars[i]) < i || chars[i] == ' ')
                {
                    chars.RemoveAt(i);
                    i--;
                }
            }
            if (chars.Count == 0) return;
            Glyph[] glyphArray = font.GetGlyphsFromCodepoint(height, chars.ToCodePointArray(), 1f, 1f);
            for (int i = 0; i < chars.Count; i++)
            {
                if (glyphArray[i].texture == null && glyphArray[i].Width != 0)
                {
                    glyphArray[i].texture = FontHelper.ByteDataToTexture2D(graphicsDevice, glyphArray[i].bitmap,
                    glyphArray[i].Width, glyphArray[i].Height);
                    glyphArray[i].bitmap = new byte[0];
                }
                Glyphs.Add(chars[i], glyphArray[i]);
            }
        }
        /// <summary>
        /// 通过IDrawAPI绘制文字
        /// </summary>
        public void DrawString(IDrawAPI spriteBatch, string text, Vector2 position)
        {
            DrawString(spriteBatch, text, position, Color.White, default, new Vector2(1, 1));
        }
        /// <summary>
        /// 通过IDrawAPI绘制文字
        /// </summary>
        public void DrawString(IDrawAPI spriteBatch, string text, Vector2 position, Color color)
        {
            DrawString(spriteBatch, text, position, color, default, new Vector2(1, 1));
        }
        /// <summary>
        /// 通过IDrawAPI绘制文字
        /// </summary>
        public void DrawString(IDrawAPI spriteBatch, string text, Vector2 position, Color color, Vector2 origin, float scale, float rotation = 0, SpriteEffects effects = SpriteEffects.None, float layerDepth = 1f)
        {
            DrawString(spriteBatch, text, position, color, origin, new Vector2(scale, scale), rotation, effects, layerDepth);
        }
        /// <summary>
        /// 通过IDrawAPI绘制文字
        /// </summary>
        public void DrawString(IDrawAPI spriteBatch, string text, Vector2 position, Color color, CenterType centerType, float scale, float rotation = 0, SpriteEffects effects = SpriteEffects.None, float layerDepth = 1f)
        {
            DrawString(spriteBatch, text, position, color, MeasureString(text, 1).MutiplyXY(Helper.CenterTypeToVector2(centerType)), new Vector2(scale, scale), rotation, effects, layerDepth);
        }
        /// <summary>
        /// 通过IDrawAPI绘制文字
        /// </summary>
        public void DrawString(IDrawAPI spriteBatch, string text, Vector2 position, Color color, Vector2 origin, Vector2 scale, float rotation = 0f, SpriteEffects effects = SpriteEffects.None, float layerDepth = 1f)
        {
            char[] chars = text.ToCharArray();
            Cache(chars);
            int defaultX = 0;
            float x = defaultX;
            float y = (int)(height * 0.6f);
            int spacingFix = onlyUseUserSpcacing ? 0 : 1;
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '\\' && chars.TryGetValue(i + 1) == 'n')
                {
                    y += height * 0.8f * spacingFix + spacing.Y;
                    x = defaultX;
                    i++;
                }
                else if (chars[i] == '\n')
                {
                    y += height * 0.8f * spacingFix + spacing.Y;
                    x = defaultX;
                }
                else if (chars[i] == ' ')
                {
                    x += defaultGlyph.Width * 0.5f * spacingFix + spacing.X;
                }
                else
                {
                    Glyph glyph = Glyphs[chars[i]];
                    if (rotation == 0) spriteBatch.Draw(new SpriteDrawInfo(glyph.texture, new Vector2(x + glyph.x0, y + glyph.y0).MutiplyXY(scale) + position, null, color, origin, scale, 0f, effects, layerDepth));
                    else spriteBatch.Draw(new SpriteDrawInfo(glyph.texture, new Vector2(x + glyph.x0, y + glyph.y0).MutiplyXY(scale).Rotate(rotation) + position, null, color, origin, scale, rotation, effects, layerDepth));
                    if (FontHelper.IsCn(chars[i])) x += defaultGlyphCn.Width * 1.2f * spacingFix + spacing.X;
                    else x += glyph.WidthAlt * spacingFix + spacing.X;
                }
            }
        }
        /// <summary>
        /// 测量文字长宽
        /// </summary>
        public Vector2 MeasureString(string text, Vector2 scale, float rotation = 0f)
        {
            char[] chars = text.ToCharArray();
            Cache(chars);
            int defaultX = 0;
            float x = defaultX;
            float maxX = 0;
            float y = height * 0.6f;
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '\\' && chars.TryGetValue(i + 1) == 'n')
                {
                    y += height * 0.8f + spacing.Y;
                    x = defaultX;
                    i++;
                }
                else if (chars[i] == '\n')
                {
                    y += height * 0.8f + spacing.Y;
                    x = defaultX;
                }
                else if (chars[i] == ' ')
                {
                    x += defaultGlyph.Width * 0.5f + spacing.X;
                }
                else
                {
                    Glyph Glyph = Glyphs[chars[i]];
                    if (FontHelper.IsCn(chars[i])) x += defaultGlyphCn.Width * 1.2f + spacing.X;
                    else x += Glyph.WidthAlt + spacing.X;
                }
                if (x > maxX) maxX = x;
            }
            return new Vector2(maxX, y).MutiplyXY(scale).Rotate(rotation);
        }
        public Vector2 MeasureString(string text, float scale, float rotation = 0f)
        {
            return MeasureString(text, new Vector2(scale, scale), rotation);
        }

        public void Dispose()
        {
            Glyphs.Clear();
            Glyphs = null;
            defaultGlyph = null;
            defaultGlyphCn = null;
            if (font is IDisposable disposable) disposable.Dispose();
        }
    }
}
