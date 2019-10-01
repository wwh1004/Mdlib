using System;
using System.Diagnostics;
using static Mdlib.PE.NativeConstants;

namespace Mdlib.PE {
	/// <summary>
	/// 可选头
	/// </summary>
	[DebuggerDisplay("OptHdr:[P:{Utils.PointerToString(RawData)} RVA:{RVA} FileOffset:{FileOffset}]")]
	internal sealed unsafe class OptionalHeader : IRawData {
		private void* _rawData;
		private uint _offset;
		private bool _is64Bit;

		/// <summary />
		public IntPtr RawData => (IntPtr)_rawData;

		/// <summary />
		public IMAGE_OPTIONAL_HEADER32* RawValue32 => !_is64Bit ? (IMAGE_OPTIONAL_HEADER32*)_rawData : throw new InvalidOperationException();

		/// <summary />
		public IMAGE_OPTIONAL_HEADER64* RawValue64 => !_is64Bit ? throw new InvalidOperationException() : (IMAGE_OPTIONAL_HEADER64*)_rawData;

		/// <summary />
		public RVA RVA => (RVA)_offset;

		/// <summary />
		public FileOffset FileOffset => (FileOffset)_offset;

		/// <summary />
		public uint Length => !_is64Bit ? IMAGE_OPTIONAL_HEADER32.UnmanagedSize : IMAGE_OPTIONAL_HEADER64.UnmanagedSize;

		/// <summary />
		public bool Is64Bit => _is64Bit;

		/// <summary />
		public DataDirectory* DataDirectories => !_is64Bit ? (DataDirectory*)RawValue32->DataDirectory : (DataDirectory*)RawValue64->DataDirectory;

		private OptionalHeader() {
		}

		public static OptionalHeader Create(NtHeader ntHeader) {
			if (ntHeader is null)
				throw new ArgumentNullException(nameof(ntHeader));

			byte* p;
			bool is64Bit;
			OptionalHeader optionalHeader;

			p = (byte*)ntHeader.RawData + 4 + IMAGE_FILE_HEADER.UnmanagedSize;
			if (!Utils.IsValidPointer(p, Math.Max(ntHeader.FileHeader.RawValue->SizeOfOptionalHeader, (ushort)2)))
				return null;
			switch (*(ushort*)p) {
			case IMAGE_NT_OPTIONAL_HDR32_MAGIC:
				is64Bit = false;
				break;
			case IMAGE_NT_OPTIONAL_HDR64_MAGIC:
				is64Bit = true;
				break;
			default:
				return null;
			}
			if (!Utils.IsValidPointer(p, !is64Bit ? IMAGE_OPTIONAL_HEADER32.UnmanagedSize : IMAGE_OPTIONAL_HEADER32.UnmanagedSize))
				return null;
			optionalHeader = new OptionalHeader {
				_rawData = p,
				_offset = (uint)ntHeader.FileOffset + 4 + IMAGE_FILE_HEADER.UnmanagedSize,
				_is64Bit = is64Bit
			};
			return optionalHeader;
		}
	}
}
