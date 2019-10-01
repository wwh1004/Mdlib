using System;
using System.Diagnostics;

namespace Mdlib.PE {
	/// <summary>
	/// 文件头
	/// </summary>
	[DebuggerDisplay("FileHdr:[P:{MdlibUtils.PointerToString(RawData)} RVA:{RVA} FileOffset:{FileOffset}]")]
	internal sealed unsafe class FileHeader : IRawData<IMAGE_FILE_HEADER> {
		private void* _rawData;
		private uint _offset;

		/// <summary />
		public IntPtr RawData => (IntPtr)_rawData;

		/// <summary />
		public IMAGE_FILE_HEADER* RawValue => (IMAGE_FILE_HEADER*)_rawData;

		/// <summary />
		public RVA RVA => (RVA)_offset;

		/// <summary />
		public FileOffset FileOffset => (FileOffset)_offset;

		/// <summary />
		public uint Length => IMAGE_FILE_HEADER.UnmanagedSize;

		private FileHeader() {
		}

		public static FileHeader Create(NtHeader ntHeader) {
			if (ntHeader is null)
				throw new ArgumentNullException(nameof(ntHeader));

			byte* p;
			FileHeader fileHeader;

			p = (byte*)ntHeader.RawData + 4;
			if (!MdlibUtils.IsValidPointer(p, IMAGE_FILE_HEADER.UnmanagedSize))
				return null;
			fileHeader = new FileHeader {
				_rawData = p,
				_offset = (uint)ntHeader.FileOffset + 4
			};
			return fileHeader;
		}
	}
}
