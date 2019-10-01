using System;
using Mdlib.PE;

namespace Mdlib.Mdlib.PE {
	/// <summary>
	/// 扩展类
	/// </summary>
	internal static unsafe class Extensions {
		public static uint GetHeadersSize(this IPEImage peImage) {
			if (peImage is null)
				throw new ArgumentNullException(nameof(peImage));

			return (uint)peImage.SectionHeaders[peImage.SectionHeaders.Length - 1].FileOffset + IMAGE_SECTION_HEADER.UnmanagedSize;
		}

		public static uint AlignUpBySection(this IPEImage peImage, uint value) {
			if (peImage is null)
				throw new ArgumentNullException(nameof(peImage));

			return Utils.AlignUp(value, !peImage.Is64Bit ? peImage.NtHeader.OptionalHeader.RawValue32->SectionAlignment : peImage.NtHeader.OptionalHeader.RawValue64->SectionAlignment);
		}

		public static uint AlignUpByFile(this IPEImage peImage, uint value) {
			if (peImage is null)
				throw new ArgumentNullException(nameof(peImage));

			return Utils.AlignUp(value, !peImage.Is64Bit ? peImage.NtHeader.OptionalHeader.RawValue32->FileAlignment : peImage.NtHeader.OptionalHeader.RawValue64->FileAlignment);
		}
	}
}
