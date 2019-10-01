using System;
using System.Diagnostics;

namespace Mdlib.PE {
	/// <summary>
	/// Nt头
	/// </summary>
	[DebuggerDisplay("NtHdr:[P:{Utils.PointerToString(RawData)} RVA:{RVA} FileOffset:{FileOffset}]")]
	internal sealed unsafe class NtHeader : IRawData {
		private void* _rawData;
		private uint _offset;
		private FileHeader _fileHeader;
		private OptionalHeader _optionalHeader;

		/// <summary />
		public IntPtr RawData => (IntPtr)_rawData;

		/// <summary />
		public IMAGE_NT_HEADERS32* RawValue32 => !_optionalHeader.Is64Bit ? (IMAGE_NT_HEADERS32*)_rawData : throw new InvalidOperationException("It's PE32 format.");

		/// <summary />
		public IMAGE_NT_HEADERS64* RawValue64 => !_optionalHeader.Is64Bit ? throw new InvalidOperationException("It's PE32+ format.") : (IMAGE_NT_HEADERS64*)_rawData;

		/// <summary />
		public RVA RVA => (RVA)_offset;

		/// <summary />
		public FileOffset FileOffset => (FileOffset)_offset;

		/// <summary />
		public uint Length => 4 + IMAGE_FILE_HEADER.UnmanagedSize + _fileHeader.RawValue->SizeOfOptionalHeader;

		/// <summary />
		public uint Signature {
			get => *(uint*)_rawData;
			set => *(uint*)_rawData = value;
		}

		/// <summary />
		public FileHeader FileHeader => _fileHeader;

		/// <summary />
		public OptionalHeader OptionalHeader => _optionalHeader;

		private NtHeader() {
		}

		public static NtHeader Create(IPEImage peImage) {
			if (peImage is null)
				throw new ArgumentNullException(nameof(peImage));

			byte* p;
			NtHeader ntHeader;

			p = (byte*)peImage.RawData + peImage.DosHeader.RawValue->e_lfanew;
			if (!Utils.IsValidPointer(p, 4))
				return null;
			ntHeader = new NtHeader {
				_rawData = p,
				_offset = peImage.DosHeader.RawValue->e_lfanew
			};
			ntHeader._fileHeader = FileHeader.Create(ntHeader);
			if (ntHeader._fileHeader is null)
				return null;
			ntHeader._optionalHeader = OptionalHeader.Create(ntHeader);
			if (ntHeader._optionalHeader is null)
				return null;
			return ntHeader;
		}
	}
}
