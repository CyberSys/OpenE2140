#region Copyright & License Information

/*
 * Copyright 2007-2023 The OpenE2140 Developers (see AUTHORS)
 * This file is part of OpenE2140, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */

#endregion

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using OpenRA.Graphics;
using OpenRA.Mods.E2140.Assets.VirtualAssets;
using OpenRA.Primitives;

namespace OpenRA.Mods.E2140.Assets.SpriteLoaders;

public class VMixSpriteFrame : ISpriteFrame
{
	public SpriteFrameType Type { get; }
	public Size Size { get; }
	public Size FrameSize { get; }
	public float2 Offset { get; }
	public byte[] Data { get; }
	public bool DisableExportPadding => true;

	public VMixSpriteFrame(SpriteFrameType type, Size size, byte[] pixels)
	{
		this.Type = type;
		this.Size = size;
		this.FrameSize = size;
		this.Offset = new float2(0, 0);
		this.Data = pixels;
	}
}

[UsedImplicitly]
public class VMixSpriteLoader : ISpriteLoader
{
	public bool TryParseSprite(Stream stream, string filename, [NotNullWhen(true)] out ISpriteFrame[]? frames, out TypeDictionary? metadata)
	{
		var identifier = stream.ReadASCII(4);

		if (identifier != "VMIX")
		{
			stream.Position -= 4;

			frames = null;
			metadata = null;

			return false;
		}

		var vmix = VMix.Cache[stream.ReadASCII(stream.ReadInt32())];

		frames = vmix.Animations.SelectMany(animation => animation.Frames)
			.Select(vmixFrame => new VMixSpriteFrame(SpriteFrameType.Rgba32, new Size((int)vmixFrame.Width, (int)vmixFrame.Height), vmixFrame.Pixels))
			.Cast<ISpriteFrame>()
			.ToArray();

		metadata = null;

		return true;
	}
}
