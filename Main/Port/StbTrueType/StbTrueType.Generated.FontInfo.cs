// Generated by Sichem at 07.03.2020 16:58:11

using System;
using System.Runtime.InteropServices;

namespace StbTrueTypeSharp
{
	internal unsafe partial class StbTrueType
	{
		public class stbtt_fontinfo
		{
			public void* userdata;
			public byte* data;
			public int fontstart = 0;
			public int numGlyphs = 0;
			public int loca = 0;
			public int head = 0;
			public int glyf = 0;
			public int hhea = 0;
			public int hmtx = 0;
			public int kern = 0;
			public int gpos = 0;
			public int svg = 0;
			public int index_map = 0;
			public int indexToLocFormat = 0;
			public stbtt__buf cff = new stbtt__buf();
			public stbtt__buf charstrings = new stbtt__buf();
			public stbtt__buf gsubrs = new stbtt__buf();
			public stbtt__buf subrs = new stbtt__buf();
			public stbtt__buf fontdicts = new stbtt__buf();
			public stbtt__buf fdselect = new stbtt__buf();
		}

		public static int stbtt_InitFont_internal(stbtt_fontinfo info, byte* data, int fontstart)
		{
			uint cmap = 0;
			uint t = 0;
			int i = 0;
			int numTables = 0;
			info.data = data;
			info.fontstart = (int)(fontstart);
			info.cff = (stbtt__buf)(stbtt__new_buf((null), (ulong)(0)));
			cmap = (uint)(stbtt__find_table(data, (uint)(fontstart), "cmap"));
			info.loca = (int)(stbtt__find_table(data, (uint)(fontstart), "loca"));
			info.head = (int)(stbtt__find_table(data, (uint)(fontstart), "head"));
			info.glyf = (int)(stbtt__find_table(data, (uint)(fontstart), "glyf"));
			info.hhea = (int)(stbtt__find_table(data, (uint)(fontstart), "hhea"));
			info.hmtx = (int)(stbtt__find_table(data, (uint)(fontstart), "hmtx"));
			info.kern = (int)(stbtt__find_table(data, (uint)(fontstart), "kern"));
			info.gpos = (int)(stbtt__find_table(data, (uint)(fontstart), "GPOS"));
			if ((((cmap == 0) || (info.head == 0)) || (info.hhea == 0)) || (info.hmtx == 0))
				return (int)(0);
			if ((info.glyf) != 0)
			{
				if (info.loca == 0)
					return (int)(0);
			}
			else
			{
				stbtt__buf b = new stbtt__buf();
				stbtt__buf topdict = new stbtt__buf();
				stbtt__buf topdictidx = new stbtt__buf();
				uint cstype = (uint)(2);
				uint charstrings = (uint)(0);
				uint fdarrayoff = (uint)(0);
				uint fdselectoff = (uint)(0);
				uint cff = 0;
				cff = (uint)(stbtt__find_table(data, (uint)(fontstart), "CFF "));
				if (cff == 0)
					return (int)(0);
				info.fontdicts = (stbtt__buf)(stbtt__new_buf((null), (ulong)(0)));
				info.fdselect = (stbtt__buf)(stbtt__new_buf((null), (ulong)(0)));
				info.cff = (stbtt__buf)(stbtt__new_buf(data + cff, (ulong)(512 * 1024 * 1024)));
				b = (stbtt__buf)(info.cff);
				stbtt__buf_skip(&b, (int)(2));
				stbtt__buf_seek(&b, (int)(stbtt__buf_get8(&b)));
				stbtt__cff_get_index(&b);
				topdictidx = (stbtt__buf)(stbtt__cff_get_index(&b));
				topdict = (stbtt__buf)(stbtt__cff_index_get((stbtt__buf)(topdictidx), (int)(0)));
				stbtt__cff_get_index(&b);
				info.gsubrs = (stbtt__buf)(stbtt__cff_get_index(&b));
				stbtt__dict_get_ints(&topdict, (int)(17), (int)(1), &charstrings);
				stbtt__dict_get_ints(&topdict, (int)(0x100 | 6), (int)(1), &cstype);
				stbtt__dict_get_ints(&topdict, (int)(0x100 | 36), (int)(1), &fdarrayoff);
				stbtt__dict_get_ints(&topdict, (int)(0x100 | 37), (int)(1), &fdselectoff);
				info.subrs = (stbtt__buf)(stbtt__get_subrs((stbtt__buf)(b), (stbtt__buf)(topdict)));
				if (cstype != 2)
					return (int)(0);
				if ((charstrings) == (0))
					return (int)(0);
				if ((fdarrayoff) != 0)
				{
					if (fdselectoff == 0)
						return (int)(0);
					stbtt__buf_seek(&b, (int)(fdarrayoff));
					info.fontdicts = (stbtt__buf)(stbtt__cff_get_index(&b));
					info.fdselect = (stbtt__buf)(stbtt__buf_range(&b, (int)(fdselectoff), (int)(b.size - fdselectoff)));
				}
				stbtt__buf_seek(&b, (int)(charstrings));
				info.charstrings = (stbtt__buf)(stbtt__cff_get_index(&b));
			}

			t = (uint)(stbtt__find_table(data, (uint)(fontstart), "maxp"));
			if ((t) != 0)
				info.numGlyphs = (int)(ttUSHORT(data + t + 4));
			else
				info.numGlyphs = (int)(0xffff);
			info.svg = (int)(-1);
			numTables = (int)(ttUSHORT(data + cmap + 2));
			info.index_map = (int)(0);
			for (i = (int)(0); (i) < (numTables); ++i)
			{
				uint encoding_record = (uint)(cmap + 4 + 8 * i);
				switch (ttUSHORT(data + encoding_record))
				{
					case STBTT_PLATFORM_ID_MICROSOFT:
						switch (ttUSHORT(data + encoding_record + 2))
						{
							case STBTT_MS_EID_UNICODE_BMP:
							case STBTT_MS_EID_UNICODE_FULL:
								info.index_map = (int)(cmap + ttULONG(data + encoding_record + 4));
								break;
						}
						break;
					case STBTT_PLATFORM_ID_UNICODE:
						info.index_map = (int)(cmap + ttULONG(data + encoding_record + 4));
						break;
				}
			}
			if ((info.index_map) == (0))
				return (int)(0);
			info.indexToLocFormat = (int)(ttUSHORT(data + info.head + 50));
			return (int)(1);
		}

		public static int stbtt_FindGlyphIndex(stbtt_fontinfo info, int unicode_codepoint)
		{
			byte* data = info.data;
			uint index_map = (uint)(info.index_map);
			ushort format = (ushort)(ttUSHORT(data + index_map + 0));
			if ((format) == (0))
			{
				int bytes = (int)(ttUSHORT(data + index_map + 2));
				if ((unicode_codepoint) < (bytes - 6))
					return (int)(*(data + index_map + 6 + unicode_codepoint));
				return (int)(0);
			}
			else if ((format) == (6))
			{
				uint first = (uint)(ttUSHORT(data + index_map + 6));
				uint count = (uint)(ttUSHORT(data + index_map + 8));
				if ((((uint)(unicode_codepoint)) >= (first)) && (((uint)(unicode_codepoint)) < (first + count)))
					return (int)(ttUSHORT(data + index_map + 10 + (unicode_codepoint - first) * 2));
				return (int)(0);
			}
			else if ((format) == (2))
			{
				return (int)(0);
			}
			else if ((format) == (4))
			{
				ushort segcount = (ushort)(ttUSHORT(data + index_map + 6) >> 1);
				ushort searchRange = (ushort)(ttUSHORT(data + index_map + 8) >> 1);
				ushort entrySelector = (ushort)(ttUSHORT(data + index_map + 10));
				ushort rangeShift = (ushort)(ttUSHORT(data + index_map + 12) >> 1);
				uint endCount = (uint)(index_map + 14);
				uint search = (uint)(endCount);
				if ((unicode_codepoint) > (0xffff))
					return (int)(0);
				if ((unicode_codepoint) >= (ttUSHORT(data + search + rangeShift * 2)))
					search += (uint)(rangeShift * 2);
				search -= (uint)(2);
				while ((entrySelector) != 0)
				{
					ushort end = 0;
					searchRange >>= 1;
					end = (ushort)(ttUSHORT(data + search + searchRange * 2));
					if ((unicode_codepoint) > (end))
						search += (uint)(searchRange * 2);
					--entrySelector;
				}
				search += (uint)(2);
				{
					ushort offset = 0;
					ushort start = 0;
					ushort item = (ushort)((search - endCount) >> 1);
					start = (ushort)(ttUSHORT(data + index_map + 14 + segcount * 2 + 2 + 2 * item));
					if ((unicode_codepoint) < (start))
						return (int)(0);
					offset = (ushort)(ttUSHORT(data + index_map + 14 + segcount * 6 + 2 + 2 * item));
					if ((offset) == (0))
						return (int)((ushort)(unicode_codepoint + ttSHORT(data + index_map + 14 + segcount * 4 + 2 + 2 * item)));
					return (int)(ttUSHORT(data + offset + (unicode_codepoint - start) * 2 + index_map + 14 + segcount * 6 + 2 + 2 * item));
				}
			}
			else if (((format) == (12)) || ((format) == (13)))
			{
				uint ngroups = (uint)(ttULONG(data + index_map + 12));
				int low = 0;
				int high = 0;
				low = (int)(0);
				high = ((int)(ngroups));
				while ((low) < (high))
				{
					int mid = (int)(low + ((high - low) >> 1));
					uint start_char = (uint)(ttULONG(data + index_map + 16 + mid * 12));
					uint end_char = (uint)(ttULONG(data + index_map + 16 + mid * 12 + 4));
					if (((uint)(unicode_codepoint)) < (start_char))
						high = (int)(mid);
					else if (((uint)(unicode_codepoint)) > (end_char))
						low = (int)(mid + 1);
					else
					{
						uint start_glyph = (uint)(ttULONG(data + index_map + 16 + mid * 12 + 8));
						if ((format) == (12))
							return (int)(start_glyph + unicode_codepoint - start_char);
						else
							return (int)(start_glyph);
					}
				}
				return (int)(0);
			}

			return (int)(0);
		}

		public static int stbtt_GetCodepointShape(stbtt_fontinfo info, int unicode_codepoint, stbtt_vertex** vertices)
		{
			return (int)(stbtt_GetGlyphShape(info, (int)(stbtt_FindGlyphIndex(info, (int)(unicode_codepoint))), vertices));
		}

		public static int stbtt__GetGlyfOffset(stbtt_fontinfo info, int glyph_index)
		{
			int g1 = 0;
			int g2 = 0;
			if ((glyph_index) >= (info.numGlyphs))
				return (int)(-1);
			if ((info.indexToLocFormat) >= (2))
				return (int)(-1);
			if ((info.indexToLocFormat) == (0))
			{
				g1 = (int)(info.glyf + ttUSHORT(info.data + info.loca + glyph_index * 2) * 2);
				g2 = (int)(info.glyf + ttUSHORT(info.data + info.loca + glyph_index * 2 + 2) * 2);
			}
			else
			{
				g1 = (int)(info.glyf + ttULONG(info.data + info.loca + glyph_index * 4));
				g2 = (int)(info.glyf + ttULONG(info.data + info.loca + glyph_index * 4 + 4));
			}

			return (int)((g1) == (g2) ? -1 : g1);
		}

		public static int stbtt_GetGlyphBox(stbtt_fontinfo info, int glyph_index, int* x0, int* y0, int* x1, int* y1)
		{
			if ((info.cff.size) != 0)
			{
				stbtt__GetGlyphInfoT2(info, (int)(glyph_index), x0, y0, x1, y1);
			}
			else
			{
				int g = (int)(stbtt__GetGlyfOffset(info, (int)(glyph_index)));
				if ((g) < (0))
					return (int)(0);
				if ((x0) != null)
					*x0 = (int)(ttSHORT(info.data + g + 2));
				if ((y0) != null)
					*y0 = (int)(ttSHORT(info.data + g + 4));
				if ((x1) != null)
					*x1 = (int)(ttSHORT(info.data + g + 6));
				if ((y1) != null)
					*y1 = (int)(ttSHORT(info.data + g + 8));
			}

			return (int)(1);
		}

		public static int stbtt_GetCodepointBox(stbtt_fontinfo info, int codepoint, int* x0, int* y0, int* x1, int* y1)
		{
			return (int)(stbtt_GetGlyphBox(info, (int)(stbtt_FindGlyphIndex(info, (int)(codepoint))), x0, y0, x1, y1));
		}

		public static int stbtt_IsGlyphEmpty(stbtt_fontinfo info, int glyph_index)
		{
			short numberOfContours = 0;
			int g = 0;
			if ((info.cff.size) != 0)
				return (int)((stbtt__GetGlyphInfoT2(info, (int)(glyph_index), (null), (null), (null), (null))) == (0) ? 1 : 0);
			g = (int)(stbtt__GetGlyfOffset(info, (int)(glyph_index)));
			if ((g) < (0))
				return (int)(1);
			numberOfContours = (short)(ttSHORT(info.data + g));
			return (int)((numberOfContours) == (0) ? 1 : 0);
		}

		public static int stbtt__close_shape(stbtt_vertex* vertices, int num_vertices, int was_off, int start_off, int sx, int sy, int scx, int scy, int cx, int cy)
		{
			if ((start_off) != 0)
			{
				if ((was_off) != 0)
					stbtt_setvertex(&vertices[num_vertices++], (byte)(STBTT_vcurve), (int)((cx + scx) >> 1), (int)((cy + scy) >> 1), (int)(cx), (int)(cy));
				stbtt_setvertex(&vertices[num_vertices++], (byte)(STBTT_vcurve), (int)(sx), (int)(sy), (int)(scx), (int)(scy));
			}
			else
			{
				if ((was_off) != 0)
					stbtt_setvertex(&vertices[num_vertices++], (byte)(STBTT_vcurve), (int)(sx), (int)(sy), (int)(cx), (int)(cy));
				else
					stbtt_setvertex(&vertices[num_vertices++], (byte)(STBTT_vline), (int)(sx), (int)(sy), (int)(0), (int)(0));
			}

			return (int)(num_vertices);
		}

		public static int stbtt__GetGlyphShapeTT(stbtt_fontinfo info, int glyph_index, stbtt_vertex** pvertices)
		{
			short numberOfContours = 0;
			byte* endPtsOfContours;
			byte* data = info.data;
			stbtt_vertex* vertices = null;
			int num_vertices = (int)(0);
			int g = (int)(stbtt__GetGlyfOffset(info, (int)(glyph_index)));
			*pvertices = (null);
			if ((g) < (0))
				return (int)(0);
			numberOfContours = (short)(ttSHORT(data + g));
			if ((numberOfContours) > (0))
			{
				byte flags = (byte)(0);
				byte flagcount = 0;
				int ins = 0;
				int i = 0;
				int j = (int)(0);
				int m = 0;
				int n = 0;
				int next_move = 0;
				int was_off = (int)(0);
				int off = 0;
				int start_off = (int)(0);
				int x = 0;
				int y = 0;
				int cx = 0;
				int cy = 0;
				int sx = 0;
				int sy = 0;
				int scx = 0;
				int scy = 0;
				byte* points;
				endPtsOfContours = (data + g + 10);
				ins = (int)(ttUSHORT(data + g + 10 + numberOfContours * 2));
				points = data + g + 10 + numberOfContours * 2 + 2 + ins;
				n = (int)(1 + ttUSHORT(endPtsOfContours + numberOfContours * 2 - 2));
				m = (int)(n + 2 * numberOfContours);
				vertices = (stbtt_vertex*)(CRuntime.malloc((ulong)(m * sizeof(stbtt_vertex))));
				if ((vertices) == (null))
					return (int)(0);
				next_move = (int)(0);
				flagcount = (byte)(0);
				off = (int)(m - n);
				for (i = (int)(0); (i) < (n); ++i)
				{
					if ((flagcount) == (0))
					{
						flags = (byte)(*points++);
						if ((flags & 8) != 0)
							flagcount = (byte)(*points++);
					}
					else
						--flagcount;
					vertices[off + i].type = (byte)(flags);
				}
				x = (int)(0);
				for (i = (int)(0); (i) < (n); ++i)
				{
					flags = (byte)(vertices[off + i].type);
					if ((flags & 2) != 0)
					{
						short dx = (short)(*points++);
						x += (int)((flags & 16) != 0 ? dx : -dx);
					}
					else
					{
						if ((flags & 16) == 0)
						{
							x = (int)(x + (short)(points[0] * 256 + points[1]));
							points += 2;
						}
					}
					vertices[off + i].x = ((short)(x));
				}
				y = (int)(0);
				for (i = (int)(0); (i) < (n); ++i)
				{
					flags = (byte)(vertices[off + i].type);
					if ((flags & 4) != 0)
					{
						short dy = (short)(*points++);
						y += (int)((flags & 32) != 0 ? dy : -dy);
					}
					else
					{
						if ((flags & 32) == 0)
						{
							y = (int)(y + (short)(points[0] * 256 + points[1]));
							points += 2;
						}
					}
					vertices[off + i].y = ((short)(y));
				}
				num_vertices = (int)(0);
				sx = (int)(sy = (int)(cx = (int)(cy = (int)(scx = (int)(scy = (int)(0))))));
				for (i = (int)(0); (i) < (n); ++i)
				{
					flags = (byte)(vertices[off + i].type);
					x = (int)(vertices[off + i].x);
					y = (int)(vertices[off + i].y);
					if ((next_move) == (i))
					{
						if (i != 0)
							num_vertices = (int)(stbtt__close_shape(vertices, (int)(num_vertices), (int)(was_off), (int)(start_off), (int)(sx), (int)(sy), (int)(scx), (int)(scy), (int)(cx), (int)(cy)));
						start_off = ((flags & 1) != 0 ? 0 : 1);
						if ((start_off) != 0)
						{
							scx = (int)(x);
							scy = (int)(y);
							if ((vertices[off + i + 1].type & 1) == 0)
							{
								sx = (int)((x + (int)(vertices[off + i + 1].x)) >> 1);
								sy = (int)((y + (int)(vertices[off + i + 1].y)) >> 1);
							}
							else
							{
								sx = ((int)(vertices[off + i + 1].x));
								sy = ((int)(vertices[off + i + 1].y));
								++i;
							}
						}
						else
						{
							sx = (int)(x);
							sy = (int)(y);
						}
						stbtt_setvertex(&vertices[num_vertices++], (byte)(STBTT_vmove), (int)(sx), (int)(sy), (int)(0), (int)(0));
						was_off = (int)(0);
						next_move = (int)(1 + ttUSHORT(endPtsOfContours + j * 2));
						++j;
					}
					else
					{
						if ((flags & 1) == 0)
						{
							if ((was_off) != 0)
								stbtt_setvertex(&vertices[num_vertices++], (byte)(STBTT_vcurve), (int)((cx + x) >> 1), (int)((cy + y) >> 1), (int)(cx), (int)(cy));
							cx = (int)(x);
							cy = (int)(y);
							was_off = (int)(1);
						}
						else
						{
							if ((was_off) != 0)
								stbtt_setvertex(&vertices[num_vertices++], (byte)(STBTT_vcurve), (int)(x), (int)(y), (int)(cx), (int)(cy));
							else
								stbtt_setvertex(&vertices[num_vertices++], (byte)(STBTT_vline), (int)(x), (int)(y), (int)(0), (int)(0));
							was_off = (int)(0);
						}
					}
				}
				num_vertices = (int)(stbtt__close_shape(vertices, (int)(num_vertices), (int)(was_off), (int)(start_off), (int)(sx), (int)(sy), (int)(scx), (int)(scy), (int)(cx), (int)(cy)));
			}
			else if ((numberOfContours) < (0))
			{
				int more = (int)(1);
				byte* comp = data + g + 10;
				num_vertices = (int)(0);
				vertices = null;
				while ((more) != 0)
				{
					ushort flags = 0;
					ushort gidx = 0;
					int comp_num_verts = (int)(0);
					int i = 0;
					stbtt_vertex* comp_verts = null;
					stbtt_vertex* tmp = null;
					float* mtx = stackalloc float[6];
					mtx[0] = (float)(1);
					mtx[1] = (float)(0);
					mtx[2] = (float)(0);
					mtx[3] = (float)(1);
					mtx[4] = (float)(0);
					mtx[5] = (float)(0);
					float m = 0;
					float n = 0;
					flags = (ushort)(ttSHORT(comp));
					comp += 2;
					gidx = (ushort)(ttSHORT(comp));
					comp += 2;
					if ((flags & 2) != 0)
					{
						if ((flags & 1) != 0)
						{
							mtx[4] = (float)(ttSHORT(comp));
							comp += 2;
							mtx[5] = (float)(ttSHORT(comp));
							comp += 2;
						}
						else
						{
							mtx[4] = (float)(*(sbyte*)(comp));
							comp += 1;
							mtx[5] = (float)(*(sbyte*)(comp));
							comp += 1;
						}
					}
					else
					{
					}
					if ((flags & (1 << 3)) != 0)
					{
						mtx[0] = (float)(mtx[3] = (float)(ttSHORT(comp) / 16384.0f));
						comp += 2;
						mtx[1] = (float)(mtx[2] = (float)(0));
					}
					else if ((flags & (1 << 6)) != 0)
					{
						mtx[0] = (float)(ttSHORT(comp) / 16384.0f);
						comp += 2;
						mtx[1] = (float)(mtx[2] = (float)(0));
						mtx[3] = (float)(ttSHORT(comp) / 16384.0f);
						comp += 2;
					}
					else if ((flags & (1 << 7)) != 0)
					{
						mtx[0] = (float)(ttSHORT(comp) / 16384.0f);
						comp += 2;
						mtx[1] = (float)(ttSHORT(comp) / 16384.0f);
						comp += 2;
						mtx[2] = (float)(ttSHORT(comp) / 16384.0f);
						comp += 2;
						mtx[3] = (float)(ttSHORT(comp) / 16384.0f);
						comp += 2;
					}
					m = ((float)(CRuntime.sqrt((double)(mtx[0] * mtx[0] + mtx[1] * mtx[1]))));
					n = ((float)(CRuntime.sqrt((double)(mtx[2] * mtx[2] + mtx[3] * mtx[3]))));
					comp_num_verts = (int)(stbtt_GetGlyphShape(info, (int)(gidx), &comp_verts));
					if ((comp_num_verts) > (0))
					{
						for (i = (int)(0); (i) < (comp_num_verts); ++i)
						{
							stbtt_vertex* v = &comp_verts[i];
							short x = 0;
							short y = 0;
							x = (short)(v->x);
							y = (short)(v->y);
							v->x = ((short)(m * (mtx[0] * x + mtx[2] * y + mtx[4])));
							v->y = ((short)(n * (mtx[1] * x + mtx[3] * y + mtx[5])));
							x = (short)(v->cx);
							y = (short)(v->cy);
							v->cx = ((short)(m * (mtx[0] * x + mtx[2] * y + mtx[4])));
							v->cy = ((short)(n * (mtx[1] * x + mtx[3] * y + mtx[5])));
						}
						tmp = (stbtt_vertex*)(CRuntime.malloc((ulong)((num_vertices + comp_num_verts) * sizeof(stbtt_vertex))));
						if (tmp == null)
						{
							if ((vertices) != null)
								CRuntime.free(vertices);
							if ((comp_verts) != null)
								CRuntime.free(comp_verts);
							return (int)(0);
						}
						if ((num_vertices) > (0))
							CRuntime.memcpy(tmp, vertices, (ulong)(num_vertices * sizeof(stbtt_vertex)));
						CRuntime.memcpy(tmp + num_vertices, comp_verts, (ulong)(comp_num_verts * sizeof(stbtt_vertex)));
						if ((vertices) != null)
							CRuntime.free(vertices);
						vertices = tmp;
						CRuntime.free(comp_verts);
						num_vertices += (int)(comp_num_verts);
					}
					more = (int)(flags & (1 << 5));
				}
			}
			else
			{
			}

			*pvertices = vertices;
			return (int)(num_vertices);
		}

		public static stbtt__buf stbtt__cid_get_glyph_subrs(stbtt_fontinfo info, int glyph_index)
		{
			stbtt__buf fdselect = (stbtt__buf)(info.fdselect);
			int nranges = 0;
			int start = 0;
			int end = 0;
			int v = 0;
			int fmt = 0;
			int fdselector = (int)(-1);
			int i = 0;
			stbtt__buf_seek(&fdselect, (int)(0));
			fmt = (int)(stbtt__buf_get8(&fdselect));
			if ((fmt) == (0))
			{
				stbtt__buf_skip(&fdselect, (int)(glyph_index));
				fdselector = (int)(stbtt__buf_get8(&fdselect));
			}
			else if ((fmt) == (3))
			{
				nranges = (int)(stbtt__buf_get((&fdselect), (int)(2)));
				start = (int)(stbtt__buf_get((&fdselect), (int)(2)));
				for (i = (int)(0); (i) < (nranges); i++)
				{
					v = (int)(stbtt__buf_get8(&fdselect));
					end = (int)(stbtt__buf_get((&fdselect), (int)(2)));
					if (((glyph_index) >= (start)) && ((glyph_index) < (end)))
					{
						fdselector = (int)(v);
						break;
					}
					start = (int)(end);
				}
			}

			if ((fdselector) == (-1))
				stbtt__new_buf((null), (ulong)(0));
			return (stbtt__buf)(stbtt__get_subrs((stbtt__buf)(info.cff), (stbtt__buf)(stbtt__cff_index_get((stbtt__buf)(info.fontdicts), (int)(fdselector)))));
		}

		public static int stbtt__run_charstring(stbtt_fontinfo info, int glyph_index, stbtt__csctx* c)
		{
			int in_header = (int)(1);
			int maskbits = (int)(0);
			int subr_stack_height = (int)(0);
			int sp = (int)(0);
			int v = 0;
			int i = 0;
			int b0 = 0;
			int has_subrs = (int)(0);
			int clear_stack = 0;
			float* s = stackalloc float[48];
			stbtt__buf* subr_stack = stackalloc stbtt__buf[10];
			stbtt__buf subrs = (stbtt__buf)(info.subrs);
			stbtt__buf b = new stbtt__buf();
			float f = 0;
			b = (stbtt__buf)(stbtt__cff_index_get((stbtt__buf)(info.charstrings), (int)(glyph_index)));
			while ((b.cursor) < (b.size))
			{
				i = (int)(0);
				clear_stack = (int)(1);
				b0 = (int)(stbtt__buf_get8(&b));
				switch (b0)
				{
					case 0x13:
					case 0x14:
						if ((in_header) != 0)
							maskbits += (int)(sp / 2);
						in_header = (int)(0);
						stbtt__buf_skip(&b, (int)((maskbits + 7) / 8));
						break;
					case 0x01:
					case 0x03:
					case 0x12:
					case 0x17:
						maskbits += (int)(sp / 2);
						break;
					case 0x15:
						in_header = (int)(0);
						if ((sp) < (2))
							return (int)(0);
						stbtt__csctx_rmove_to(c, (float)(s[sp - 2]), (float)(s[sp - 1]));
						break;
					case 0x04:
						in_header = (int)(0);
						if ((sp) < (1))
							return (int)(0);
						stbtt__csctx_rmove_to(c, (float)(0), (float)(s[sp - 1]));
						break;
					case 0x16:
						in_header = (int)(0);
						if ((sp) < (1))
							return (int)(0);
						stbtt__csctx_rmove_to(c, (float)(s[sp - 1]), (float)(0));
						break;
					case 0x05:
						if ((sp) < (2))
							return (int)(0);
						for (; (i + 1) < (sp); i += (int)(2))
						{
							stbtt__csctx_rline_to(c, (float)(s[i]), (float)(s[i + 1]));
						}
						break;
					case 0x07:
					case 0x06:
						if ((sp) < (1))
							return (int)(0);
						int goto_vlineto = (int)((b0) == (0x07) ? 1 : 0);
						for (; ; )
						{
							if ((goto_vlineto) == (0))
							{
								if ((i) >= (sp))
									break;
								stbtt__csctx_rline_to(c, (float)(s[i]), (float)(0));
								i++;
							}
							goto_vlineto = (int)(0);
							if ((i) >= (sp))
								break;
							stbtt__csctx_rline_to(c, (float)(0), (float)(s[i]));
							i++;
						}
						break;
					case 0x1F:
					case 0x1E:
						if ((sp) < (4))
							return (int)(0);
						int goto_hvcurveto = (int)((b0) == (0x1F) ? 1 : 0);
						for (; ; )
						{
							if ((goto_hvcurveto) == (0))
							{
								if ((i + 3) >= (sp))
									break;
								stbtt__csctx_rccurve_to(c, (float)(0), (float)(s[i]), (float)(s[i + 1]), (float)(s[i + 2]), (float)(s[i + 3]), (float)(((sp - i) == (5)) ? s[i + 4] : 0.0f));
								i += (int)(4);
							}
							goto_hvcurveto = (int)(0);
							if ((i + 3) >= (sp))
								break;
							stbtt__csctx_rccurve_to(c, (float)(s[i]), (float)(0), (float)(s[i + 1]), (float)(s[i + 2]), (float)(((sp - i) == (5)) ? s[i + 4] : 0.0f), (float)(s[i + 3]));
							i += (int)(4);
						}
						break;
					case 0x08:
						if ((sp) < (6))
							return (int)(0);
						for (; (i + 5) < (sp); i += (int)(6))
						{
							stbtt__csctx_rccurve_to(c, (float)(s[i]), (float)(s[i + 1]), (float)(s[i + 2]), (float)(s[i + 3]), (float)(s[i + 4]), (float)(s[i + 5]));
						}
						break;
					case 0x18:
						if ((sp) < (8))
							return (int)(0);
						for (; (i + 5) < (sp - 2); i += (int)(6))
						{
							stbtt__csctx_rccurve_to(c, (float)(s[i]), (float)(s[i + 1]), (float)(s[i + 2]), (float)(s[i + 3]), (float)(s[i + 4]), (float)(s[i + 5]));
						}
						if ((i + 1) >= (sp))
							return (int)(0);
						stbtt__csctx_rline_to(c, (float)(s[i]), (float)(s[i + 1]));
						break;
					case 0x19:
						if ((sp) < (8))
							return (int)(0);
						for (; (i + 1) < (sp - 6); i += (int)(2))
						{
							stbtt__csctx_rline_to(c, (float)(s[i]), (float)(s[i + 1]));
						}
						if ((i + 5) >= (sp))
							return (int)(0);
						stbtt__csctx_rccurve_to(c, (float)(s[i]), (float)(s[i + 1]), (float)(s[i + 2]), (float)(s[i + 3]), (float)(s[i + 4]), (float)(s[i + 5]));
						break;
					case 0x1A:
					case 0x1B:
						if ((sp) < (4))
							return (int)(0);
						f = (float)(0.0);
						if ((sp & 1) != 0)
						{
							f = (float)(s[i]);
							i++;
						}
						for (; (i + 3) < (sp); i += (int)(4))
						{
							if ((b0) == (0x1B))
								stbtt__csctx_rccurve_to(c, (float)(s[i]), (float)(f), (float)(s[i + 1]), (float)(s[i + 2]), (float)(s[i + 3]), (float)(0.0));
							else
								stbtt__csctx_rccurve_to(c, (float)(f), (float)(s[i]), (float)(s[i + 1]), (float)(s[i + 2]), (float)(0.0), (float)(s[i + 3]));
							f = (float)(0.0);
						}
						break;
					case 0x0A:
					case 0x1D:
						if ((b0) == (0x0A))
						{
							if (has_subrs == 0)
							{
								if ((info.fdselect.size) != 0)
									subrs = (stbtt__buf)(stbtt__cid_get_glyph_subrs(info, (int)(glyph_index)));
								has_subrs = (int)(1);
							}
						}
						if ((sp) < (1))
							return (int)(0);
						v = ((int)(s[--sp]));
						if ((subr_stack_height) >= (10))
							return (int)(0);
						subr_stack[subr_stack_height++] = (stbtt__buf)(b);
						b = (stbtt__buf)(stbtt__get_subr((stbtt__buf)((b0) == (0x0A) ? subrs : info.gsubrs), (int)(v)));
						if ((b.size) == (0))
							return (int)(0);
						b.cursor = (int)(0);
						clear_stack = (int)(0);
						break;
					case 0x0B:
						if (subr_stack_height <= 0)
							return (int)(0);
						b = (stbtt__buf)(subr_stack[--subr_stack_height]);
						clear_stack = (int)(0);
						break;
					case 0x0E:
						stbtt__csctx_close_shape(c);
						return (int)(1);
					case 0x0C:
					{
						float dx1 = 0;
						float dx2 = 0;
						float dx3 = 0;
						float dx4 = 0;
						float dx5 = 0;
						float dx6 = 0;
						float dy1 = 0;
						float dy2 = 0;
						float dy3 = 0;
						float dy4 = 0;
						float dy5 = 0;
						float dy6 = 0;
						float dx = 0;
						float dy = 0;
						int b1 = (int)(stbtt__buf_get8(&b));
						switch (b1)
						{
							case 0x22:
								if ((sp) < (7))
									return (int)(0);
								dx1 = (float)(s[0]);
								dx2 = (float)(s[1]);
								dy2 = (float)(s[2]);
								dx3 = (float)(s[3]);
								dx4 = (float)(s[4]);
								dx5 = (float)(s[5]);
								dx6 = (float)(s[6]);
								stbtt__csctx_rccurve_to(c, (float)(dx1), (float)(0), (float)(dx2), (float)(dy2), (float)(dx3), (float)(0));
								stbtt__csctx_rccurve_to(c, (float)(dx4), (float)(0), (float)(dx5), (float)(-dy2), (float)(dx6), (float)(0));
								break;
							case 0x23:
								if ((sp) < (13))
									return (int)(0);
								dx1 = (float)(s[0]);
								dy1 = (float)(s[1]);
								dx2 = (float)(s[2]);
								dy2 = (float)(s[3]);
								dx3 = (float)(s[4]);
								dy3 = (float)(s[5]);
								dx4 = (float)(s[6]);
								dy4 = (float)(s[7]);
								dx5 = (float)(s[8]);
								dy5 = (float)(s[9]);
								dx6 = (float)(s[10]);
								dy6 = (float)(s[11]);
								stbtt__csctx_rccurve_to(c, (float)(dx1), (float)(dy1), (float)(dx2), (float)(dy2), (float)(dx3), (float)(dy3));
								stbtt__csctx_rccurve_to(c, (float)(dx4), (float)(dy4), (float)(dx5), (float)(dy5), (float)(dx6), (float)(dy6));
								break;
							case 0x24:
								if ((sp) < (9))
									return (int)(0);
								dx1 = (float)(s[0]);
								dy1 = (float)(s[1]);
								dx2 = (float)(s[2]);
								dy2 = (float)(s[3]);
								dx3 = (float)(s[4]);
								dx4 = (float)(s[5]);
								dx5 = (float)(s[6]);
								dy5 = (float)(s[7]);
								dx6 = (float)(s[8]);
								stbtt__csctx_rccurve_to(c, (float)(dx1), (float)(dy1), (float)(dx2), (float)(dy2), (float)(dx3), (float)(0));
								stbtt__csctx_rccurve_to(c, (float)(dx4), (float)(0), (float)(dx5), (float)(dy5), (float)(dx6), (float)(-(dy1 + dy2 + dy5)));
								break;
							case 0x25:
								if ((sp) < (11))
									return (int)(0);
								dx1 = (float)(s[0]);
								dy1 = (float)(s[1]);
								dx2 = (float)(s[2]);
								dy2 = (float)(s[3]);
								dx3 = (float)(s[4]);
								dy3 = (float)(s[5]);
								dx4 = (float)(s[6]);
								dy4 = (float)(s[7]);
								dx5 = (float)(s[8]);
								dy5 = (float)(s[9]);
								dx6 = (float)(dy6 = (float)(s[10]));
								dx = (float)(dx1 + dx2 + dx3 + dx4 + dx5);
								dy = (float)(dy1 + dy2 + dy3 + dy4 + dy5);
								if ((CRuntime.fabs((double)(dx))) > (CRuntime.fabs((double)(dy))))
									dy6 = (float)(-dy);
								else
									dx6 = (float)(-dx);
								stbtt__csctx_rccurve_to(c, (float)(dx1), (float)(dy1), (float)(dx2), (float)(dy2), (float)(dx3), (float)(dy3));
								stbtt__csctx_rccurve_to(c, (float)(dx4), (float)(dy4), (float)(dx5), (float)(dy5), (float)(dx6), (float)(dy6));
								break;
							default:
								return (int)(0);
						}
					}
					break;
					default:
						if (((b0 != 255) && (b0 != 28)) && (((b0) < (32)) || ((b0) > (254))))
							return (int)(0);
						if ((b0) == (255))
						{
							f = (float)((float)((int)(stbtt__buf_get((&b), (int)(4)))) / 0x10000);
						}
						else
						{
							stbtt__buf_skip(&b, (int)(-1));
							f = ((float)((short)(stbtt__cff_int(&b))));
						}
						if ((sp) >= (48))
							return (int)(0);
						s[sp++] = (float)(f);
						clear_stack = (int)(0);
						break;
				}
				if ((clear_stack) != 0)
					sp = (int)(0);
			}
			return (int)(0);
		}

		public static int stbtt__GetGlyphShapeT2(stbtt_fontinfo info, int glyph_index, stbtt_vertex** pvertices)
		{
			stbtt__csctx count_ctx = new stbtt__csctx();
			count_ctx.bounds = (int)(1);
			stbtt__csctx output_ctx = new stbtt__csctx();
			if ((stbtt__run_charstring(info, (int)(glyph_index), &count_ctx)) != 0)
			{
				*pvertices = (stbtt_vertex*)(CRuntime.malloc((ulong)(count_ctx.num_vertices * sizeof(stbtt_vertex))));
				output_ctx.pvertices = *pvertices;
				if ((stbtt__run_charstring(info, (int)(glyph_index), &output_ctx)) != 0)
				{
					return (int)(output_ctx.num_vertices);
				}
			}

			*pvertices = (null);
			return (int)(0);
		}

		public static int stbtt__GetGlyphInfoT2(stbtt_fontinfo info, int glyph_index, int* x0, int* y0, int* x1, int* y1)
		{
			stbtt__csctx c = new stbtt__csctx();
			c.bounds = (int)(1);
			int r = (int)(stbtt__run_charstring(info, (int)(glyph_index), &c));
			if ((x0) != null)
				*x0 = (int)((r) != 0 ? c.min_x : 0);
			if ((y0) != null)
				*y0 = (int)((r) != 0 ? c.min_y : 0);
			if ((x1) != null)
				*x1 = (int)((r) != 0 ? c.max_x : 0);
			if ((y1) != null)
				*y1 = (int)((r) != 0 ? c.max_y : 0);
			return (int)((r) != 0 ? c.num_vertices : 0);
		}

		public static int stbtt_GetGlyphShape(stbtt_fontinfo info, int glyph_index, stbtt_vertex** pvertices)
		{
			if (info.cff.size == 0)
				return (int)(stbtt__GetGlyphShapeTT(info, (int)(glyph_index), pvertices));
			else
				return (int)(stbtt__GetGlyphShapeT2(info, (int)(glyph_index), pvertices));
		}

		public static void stbtt_GetGlyphHMetrics(stbtt_fontinfo info, int glyph_index, int* advanceWidth, int* leftSideBearing)
		{
			ushort numOfLongHorMetrics = (ushort)(ttUSHORT(info.data + info.hhea + 34));
			if ((glyph_index) < (numOfLongHorMetrics))
			{
				if ((advanceWidth) != null)
					*advanceWidth = (int)(ttSHORT(info.data + info.hmtx + 4 * glyph_index));
				if ((leftSideBearing) != null)
					*leftSideBearing = (int)(ttSHORT(info.data + info.hmtx + 4 * glyph_index + 2));
			}
			else
			{
				if ((advanceWidth) != null)
					*advanceWidth = (int)(ttSHORT(info.data + info.hmtx + 4 * (numOfLongHorMetrics - 1)));
				if ((leftSideBearing) != null)
					*leftSideBearing = (int)(ttSHORT(info.data + info.hmtx + 4 * numOfLongHorMetrics + 2 * (glyph_index - numOfLongHorMetrics)));
			}

		}

		public static float stbtt_ScaleForPixelHeight(stbtt_fontinfo info, float height)
		{
			int fheight = (int)(ttSHORT(info.data + info.hhea + 4) - ttSHORT(info.data + info.hhea + 6));
			return (float)(height / fheight);
		}


		public static void stbtt_GetGlyphBitmapBoxSubpixel(stbtt_fontinfo font, int glyph, float scale_x, float scale_y, float shift_x, float shift_y, int* ix0, int* iy0, int* ix1, int* iy1)
		{
			int x0 = (int)(0);
			int y0 = (int)(0);
			int x1 = 0;
			int y1 = 0;
			if (stbtt_GetGlyphBox(font, (int)(glyph), &x0, &y0, &x1, &y1) == 0)
			{
				if ((ix0) != null)
					*ix0 = (int)(0);
				if ((iy0) != null)
					*iy0 = (int)(0);
				if ((ix1) != null)
					*ix1 = (int)(0);
				if ((iy1) != null)
					*iy1 = (int)(0);
			}
			else
			{
				if ((ix0) != null)
					*ix0 = ((int)(CRuntime.floor((double)(x0 * scale_x + shift_x))));
				if ((iy0) != null)
					*iy0 = ((int)(CRuntime.floor((double)(-y1 * scale_y + shift_y))));
				if ((ix1) != null)
					*ix1 = ((int)(CRuntime.ceil((double)(x1 * scale_x + shift_x))));
				if ((iy1) != null)
					*iy1 = ((int)(CRuntime.ceil((double)(-y0 * scale_y + shift_y))));
			}

		}

		public static void stbtt_GetGlyphBitmapBox(stbtt_fontinfo font, int glyph, float scale_x, float scale_y, int* ix0, int* iy0, int* ix1, int* iy1)
		{
			stbtt_GetGlyphBitmapBoxSubpixel(font, (int)(glyph), (float)(scale_x), (float)(scale_y), (float)(0.0f), (float)(0.0f), ix0, iy0, ix1, iy1);
		}

		public static void stbtt_GetCodepointBitmapBoxSubpixel(stbtt_fontinfo font, int codepoint, float scale_x, float scale_y, float shift_x, float shift_y, int* ix0, int* iy0, int* ix1, int* iy1)
		{
			stbtt_GetGlyphBitmapBoxSubpixel(font, (int)(stbtt_FindGlyphIndex(font, (int)(codepoint))), (float)(scale_x), (float)(scale_y), (float)(shift_x), (float)(shift_y), ix0, iy0, ix1, iy1);
		}

		public static void stbtt_GetCodepointBitmapBox(stbtt_fontinfo font, int codepoint, float scale_x, float scale_y, int* ix0, int* iy0, int* ix1, int* iy1)
		{
			stbtt_GetCodepointBitmapBoxSubpixel(font, (int)(codepoint), (float)(scale_x), (float)(scale_y), (float)(0.0f), (float)(0.0f), ix0, iy0, ix1, iy1);
		}

		public static byte* stbtt_GetGlyphBitmapSubpixel(stbtt_fontinfo info, float scale_x, float scale_y, float shift_x, float shift_y, int glyph, int* width, int* height, int* xoff, int* yoff)
		{
			int ix0 = 0;
			int iy0 = 0;
			int ix1 = 0;
			int iy1 = 0;
			stbtt__bitmap gbm = new stbtt__bitmap();
			stbtt_vertex* vertices;
			int num_verts = (int)(stbtt_GetGlyphShape(info, (int)(glyph), &vertices));
			if ((scale_x) == (0))
				scale_x = (float)(scale_y);
			if ((scale_y) == (0))
			{
				if ((scale_x) == (0))
				{
					CRuntime.free(vertices);
					return (null);
				}
				scale_y = (float)(scale_x);
			}

			stbtt_GetGlyphBitmapBoxSubpixel(info, (int)(glyph), (float)(scale_x), (float)(scale_y), (float)(shift_x), (float)(shift_y), &ix0, &iy0, &ix1, &iy1);
			gbm.w = (int)(ix1 - ix0);
			gbm.h = (int)(iy1 - iy0);
			gbm.pixels = (null);
			if ((width) != null)
				*width = (int)(gbm.w);
			if ((height) != null)
				*height = (int)(gbm.h);
			if ((xoff) != null)
				*xoff = (int)(ix0);
			if ((yoff) != null)
				*yoff = (int)(iy0);
			if (((gbm.w) != 0) && ((gbm.h) != 0))
			{
				gbm.pixels = (byte*)(CRuntime.malloc((ulong)(gbm.w * gbm.h)));
				if ((gbm.pixels) != null)
				{
					gbm.stride = (int)(gbm.w);
					stbtt_Rasterize(&gbm, (float)(0.35f), vertices, (int)(num_verts), (float)(scale_x), (float)(scale_y), (float)(shift_x), (float)(shift_y), (int)(ix0), (int)(iy0), (int)(1), info.userdata);
				}
			}

			CRuntime.free(vertices);
			return gbm.pixels;
		}

		public static byte* stbtt_GetGlyphBitmap(stbtt_fontinfo info, float scale_x, float scale_y, int glyph, int* width, int* height, int* xoff, int* yoff)
		{
			return stbtt_GetGlyphBitmapSubpixel(info, (float)(scale_x), (float)(scale_y), (float)(0.0f), (float)(0.0f), (int)(glyph), width, height, xoff, yoff);
		}

		public static void stbtt_MakeGlyphBitmapSubpixel(stbtt_fontinfo info, byte* output, int out_w, int out_h, int out_stride, float scale_x, float scale_y, float shift_x, float shift_y, int glyph)
		{
			int ix0 = 0;
			int iy0 = 0;
			stbtt_vertex* vertices;
			int num_verts = (int)(stbtt_GetGlyphShape(info, (int)(glyph), &vertices));
			stbtt__bitmap gbm = new stbtt__bitmap();
			stbtt_GetGlyphBitmapBoxSubpixel(info, (int)(glyph), (float)(scale_x), (float)(scale_y), (float)(shift_x), (float)(shift_y), &ix0, &iy0, null, null);
			gbm.pixels = output;
			gbm.w = (int)(out_w);
			gbm.h = (int)(out_h);
			gbm.stride = (int)(out_stride);
			if (((gbm.w) != 0) && ((gbm.h) != 0))
				stbtt_Rasterize(&gbm, (float)(0.35f), vertices, (int)(num_verts), (float)(scale_x), (float)(scale_y), (float)(shift_x), (float)(shift_y), (int)(ix0), (int)(iy0), (int)(1), info.userdata);
			CRuntime.free(vertices);
		}

		public static void stbtt_MakeGlyphBitmap(stbtt_fontinfo info, byte* output, int out_w, int out_h, int out_stride, float scale_x, float scale_y, int glyph)
		{
			stbtt_MakeGlyphBitmapSubpixel(info, output, (int)(out_w), (int)(out_h), (int)(out_stride), (float)(scale_x), (float)(scale_y), (float)(0.0f), (float)(0.0f), (int)(glyph));
		}
		public static void stbtt_MakeCodepointBitmapSubpixel(stbtt_fontinfo info, byte* output, int out_w, int out_h, int out_stride, float scale_x, float scale_y, float shift_x, float shift_y, int codepoint)
		{
			stbtt_MakeGlyphBitmapSubpixel(info, output, (int)(out_w), (int)(out_h), (int)(out_stride), (float)(scale_x), (float)(scale_y), (float)(shift_x), (float)(shift_y), (int)(stbtt_FindGlyphIndex(info, (int)(codepoint))));
		}
		public static void stbtt_MakeCodepointBitmap(stbtt_fontinfo info, byte* output, int out_w, int out_h, int out_stride, float scale_x, float scale_y, int codepoint)
		{
			stbtt_MakeCodepointBitmapSubpixel(info, output, (int)(out_w), (int)(out_h), (int)(out_stride), (float)(scale_x), (float)(scale_y), (float)(0.0f), (float)(0.0f), (int)(codepoint));
		}

		public static int stbtt_InitFont(stbtt_fontinfo info, byte* data, int offset)
		{
			return (int)(stbtt_InitFont_internal(info, data, (int)(offset)));
		}
	}
}