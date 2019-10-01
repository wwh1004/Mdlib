using System;
using System.Diagnostics;

namespace Mdlib.PE {
	/// <summary>
	/// Dos头
	/// </summary>
	[DebuggerDisplay("DosHdr:[P:{MdlibUtils.PointerToString(RawData)} NTO:{NtHeaderOffset}]")]
	internal sealed unsafe class DosHeader : IRawData<IMAGE_DOS_HEADER> {
		private void* _rawData;

		/// <summary />
		public IntPtr RawData => (IntPtr)_rawData;

		/// <summary />
		public IMAGE_DOS_HEADER* RawValue => (IMAGE_DOS_HEADER*)_rawData;

		/// <summary />
		public RVA RVA => 0;

		/// <summary />
		public FileOffset FileOffset => 0;

		/// <summary />
		public uint Length => IMAGE_DOS_HEADER.UnmanagedSize;

		private DosHeader() {
		}

		public static DosHeader Create(IPEImage peImage) {
			if (peImage is null)
				throw new ArgumentNullException(nameof(peImage));

			DosHeader dosHeader;

			dosHeader = new DosHeader {
				_rawData = (byte*)peImage.RawData
			};
			return dosHeader;
		}
	}
}
