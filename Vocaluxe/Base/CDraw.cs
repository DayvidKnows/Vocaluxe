﻿#region license
// /*
//     This file is part of Vocaluxe.
// 
//     Vocaluxe is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Vocaluxe is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with Vocaluxe. If not, see <http://www.gnu.org/licenses/>.
//  */
#endregion

using System;
using System.Drawing;
using Vocaluxe.Lib.Draw;
using VocaluxeLib.Menu;

namespace Vocaluxe.Base
{
    static class CDraw
    {
        private static IDraw _Draw;

        public static void InitDraw()
        {
            switch (CConfig.Renderer)
            {
                case ERenderer.TR_CONFIG_SOFTWARE:
                    _Draw = new CDrawWinForm();
                    break;

                case ERenderer.TR_CONFIG_OPENGL:
                    try
                    {
                        _Draw = new COpenGL();
                    }
                    catch (Exception e)
                    {
                        CLog.LogError(e.Message + " - Error in initializing of OpenGL. Please check whether" +
                                      " your graphic card drivers are up to date.", true, true);
                    }
                    break;

#if WIN
                case ERenderer.TR_CONFIG_DIRECT3D:
                    try
                    {
                        _Draw = new CDirect3D();
                    }
                    catch (Exception e)
                    {
                        CLog.LogError(e.Message + " - Error in initializing of Direct3D. Please check if " +
                                      "your DirectX redistributables and graphic card drivers are up to date. You can " +
                                      "download the DirectX runtimes at http://www.microsoft.com/download/en/details.aspx?id=8109", true, true);
                    }
                    break;
#endif

                default:
                    _Draw = new CDrawWinForm();
                    break;
            }
            if (!_Draw.Init())
                CLog.LogError("Could not init drawing interface!", true, true);
        }

        public static void MainLoop()
        {
            CGraphics.InitFirstScreen();
            _Draw.MainLoop();
        }

        public static void Unload()
        {
            _Draw.Unload();
            _Draw = null;
        }

        public static int GetScreenWidth()
        {
            return _Draw.GetScreenWidth();
        }

        public static int GetScreenHeight()
        {
            return _Draw.GetScreenHeight();
        }

        public static void DrawLine(int a, int r, int g, int b, int w, int x1, int y1, int x2, int y2)
        {
            _Draw.DrawLine(a, r, g, b, w, x1, y1, x2, y2);
        }

        public static void DrawRect(SColorF color, SRectF rect, float thickness)
        {
            if (thickness <= 0f)
                return;

            _Draw.DrawColor(color, new SRectF(rect.X - thickness / 2, rect.Y - thickness / 2, rect.W + thickness, thickness, rect.Z));
            _Draw.DrawColor(color, new SRectF(rect.X - thickness / 2, rect.Y + rect.H - thickness / 2, rect.W + thickness, thickness, rect.Z));
            _Draw.DrawColor(color, new SRectF(rect.X - thickness / 2, rect.Y - thickness / 2, thickness, rect.H + thickness, rect.Z));
            _Draw.DrawColor(color, new SRectF(rect.X + rect.W - thickness / 2, rect.Y - thickness / 2, thickness, rect.H + thickness, rect.Z));
        }

        public static void DrawColor(SColorF color, SRectF rect)
        {
            _Draw.DrawColor(color, rect);
        }

        public static void DrawColorReflection(SColorF color, SRectF rect, float space, float height)
        {
            _Draw.DrawColorReflection(color, rect, space, height);
        }

        public static void ClearScreen()
        {
            _Draw.ClearScreen();
        }

        public static STexture CopyScreen()
        {
            return _Draw.CopyScreen();
        }

        public static void CopyScreen(ref STexture texture)
        {
            _Draw.CopyScreen(ref texture);
        }

        public static void MakeScreenShot()
        {
            _Draw.MakeScreenShot();
        }

        public static STexture AddTexture(Bitmap bitmap)
        {
            return _Draw.AddTexture(bitmap);
        }

        public static STexture AddTexture(string texturePath)
        {
            return _Draw.AddTexture(texturePath);
        }

        public static STexture AddTexture(int w, int h, ref byte[] data)
        {
            return _Draw.AddTexture(w, h, ref data);
        }

        public static STexture EnqueueTexture(int w, int h, ref byte[] data)
        {
            return _Draw.EnqueueTexture(w, h, ref data);
        }

        public static bool UpdateTexture(ref STexture texture, ref byte[] data)
        {
            return _Draw.UpdateTexture(ref texture, ref data);
        }

        public static void RemoveTexture(ref STexture texture)
        {
            _Draw.RemoveTexture(ref texture);
        }

        public static void DrawTexture(STexture texture)
        {
            _Draw.DrawTexture(texture);
        }

        public static void DrawTexture(STexture texture, SRectF rect)
        {
            _Draw.DrawTexture(texture, rect);
        }

        public static void DrawTexture(STexture texture, SRectF rect, SColorF color, bool mirrored = false)
        {
            _Draw.DrawTexture(texture, rect, color, mirrored);
        }

        public static void DrawTexture(STexture texture, SRectF rect, SColorF color, SRectF bounds, bool mirrored = false)
        {
            _Draw.DrawTexture(texture, rect, color, bounds, mirrored);
        }

        public static void DrawTexture(STexture texture, SRectF rect, SColorF color, float begin, float end)
        {
            _Draw.DrawTexture(texture, rect, color, begin, end);
        }

        public static void DrawTexture(CStatic staticBounds, STexture texture, EAspect aspect)
        {
            if (texture.Height <= 0f)
                return;

            RectangleF bounds = new RectangleF(staticBounds.Rect.X, staticBounds.Rect.Y, staticBounds.Rect.W, staticBounds.Rect.H);
            RectangleF rect = new RectangleF(0f, 0f, texture.Width, texture.Height);

            CHelper.SetRect(bounds, out rect, rect.Width / rect.Height, aspect);
            DrawTexture(texture, new SRectF(rect.X, rect.Y, rect.Width, rect.Height, staticBounds.Rect.Z),
                        texture.Color, new SRectF(bounds.X, bounds.Y, bounds.Width, bounds.Height, 0f));
        }

        public static void DrawTextureReflection(STexture texture, SRectF rect, SColorF color, SRectF bounds, float space, float height)
        {
            _Draw.DrawTextureReflection(texture, rect, color, bounds, space, height);
        }

        public static int TextureCount()
        {
            return _Draw.GetTextureCount();
        }
    }
}