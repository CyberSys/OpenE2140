﻿#region Copyright & License Information

/*
 * Copyright 2007-2022 The Earth 2140 Developers (see AUTHORS)
 * This file is part of Earth 2140, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */

#endregion

namespace OpenRA.Mods.E2140.FileFormats;

public class DatImage
{
	public readonly int Width;
	public readonly int Height;
	public readonly byte[] Pixels;

	public DatImage(Stream stream)
	{
		this.Width = stream.ReadUInt16();
		this.Height = stream.ReadUInt16();
		var unk = stream.ReadUInt16(); // TODO whas is this?!

		this.Pixels = new byte[this.Width * this.Height];

		for (var i = 0; i < this.Pixels.Length;)
			i += stream.Read(this.Pixels, i, this.Pixels.Length - i);
	}
}
