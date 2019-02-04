using System;
using System.Diagnostics;
using static Mdlib.NativeMethods;

namespace Mdlib.PE {
	/// <summary>
	/// Nt头
	/// </summary>
	[DebuggerDisplay("NtHdr:[P:{Utils.PointerToString(RawData)} RVA:{RVA} FOA:{FOA}]")]
	public sealed unsafe class NtHeader : IRawData {
		private readonly void* _rawData;
		private readonly uint _offset;
		private readonly FileHeader _fileHeader;
		private readonly OptionalHeader _optionalHeader;
		private readonly bool _is64Bit;

		/// <summary />
		public IntPtr RawData => (IntPtr)_rawData;

		/// <summary />
		public IMAGE_NT_HEADERS32* RawValue32 => _is64Bit ? throw new InvalidOperationException("It's PE32 format.") : (IMAGE_NT_HEADERS32*)_rawData;

		/// <summary />
		public IMAGE_NT_HEADERS64* RawValue64 => _is64Bit ? (IMAGE_NT_HEADERS64*)_rawData : throw new InvalidOperationException("It's PE32+ format.");

		/// <summary />
		public RVA RVA => (RVA)_offset;

		/// <summary />
		public FOA FOA => (FOA)_offset;

		/// <summary />
		public uint Length => _is64Bit ? IMAGE_NT_HEADERS64.UnmanagedSize : IMAGE_NT_HEADERS32.UnmanagedSize;

		/// <summary />
		public uint Signature {
			get => *(uint*)_rawData;
			set => *(uint*)_rawData = value;
		}

		/// <summary />
		public FileHeader FileHeader => _fileHeader;

		/// <summary />
		public OptionalHeader OptionalHeader => _optionalHeader;

		internal NtHeader(IPEImage peImage, out bool is64Bit) {
			if (peImage == null)
				throw new ArgumentNullException(nameof(peImage));

			_offset = peImage.DosHeader.NtHeaderOffset;
			_rawData = (byte*)peImage.RawData + _offset;
			_fileHeader = new FileHeader(this);
			_optionalHeader = new OptionalHeader(this, out is64Bit);
			_is64Bit = is64Bit;
		}
	}
}
