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
		public ushort Magic {
			get => Unsafe32->Magic;
			set => Unsafe32->Magic = value;
		}

		/// <summary />
		public byte MajorLinkerVersion {
			get => Unsafe32->MajorLinkerVersion;
			set => Unsafe32->MajorLinkerVersion = value;
		}

		/// <summary />
		public byte MinorLinkerVersion {
			get => Unsafe32->MinorLinkerVersion;
			set => Unsafe32->MinorLinkerVersion = value;
		}

		/// <summary />
		public uint SizeOfCode {
			get => Unsafe32->SizeOfCode;
			set => Unsafe32->SizeOfCode = value;
		}

		/// <summary />
		public uint SizeOfInitializedData {
			get => Unsafe32->SizeOfInitializedData;
			set => Unsafe32->SizeOfInitializedData = value;
		}

		/// <summary />
		public uint SizeOfUninitializedData {
			get => Unsafe32->SizeOfUninitializedData;
			set => Unsafe32->SizeOfUninitializedData = value;
		}

		/// <summary />
		public uint AddressOfEntryPoint {
			get => Unsafe32->AddressOfEntryPoint;
			set => Unsafe32->AddressOfEntryPoint = value;
		}

		/// <summary />
		public uint BaseOfCode {
			get => Unsafe32->BaseOfCode;
			set => Unsafe32->BaseOfCode = value;
		}

		/// <summary />
		public uint BaseOfData {
			get => !_is64Bit ? Unsafe32->BaseOfData : throw new NotSupportedException();
			set {
				if (!_is64Bit)
					Unsafe32->BaseOfData = value;
				else
					throw new NotSupportedException();
			}
		}

		/// <summary />
		public ulong ImageBase {
			get => !_is64Bit ? Unsafe32->ImageBase : Unsafe64->ImageBase;
			set {
				if (!_is64Bit)
					Unsafe32->ImageBase = (uint)value;
				else
					Unsafe64->ImageBase = value;
			}
		}

		/// <summary />
		public uint SectionAlignment {
			get => Unsafe32->SectionAlignment;
			set => Unsafe32->SectionAlignment = value;
		}

		/// <summary />
		public uint FileAlignment {
			get => Unsafe32->FileAlignment;
			set => Unsafe32->FileAlignment = value;
		}

		/// <summary />
		public ushort MajorOperatingSystemVersion {
			get => Unsafe32->MajorOperatingSystemVersion;
			set => Unsafe32->MajorOperatingSystemVersion = value;
		}

		/// <summary />
		public ushort MinorOperatingSystemVersion {
			get => Unsafe32->MinorOperatingSystemVersion;
			set => Unsafe32->MinorOperatingSystemVersion = value;
		}

		/// <summary />
		public ushort MajorImageVersion {
			get => Unsafe32->MajorImageVersion;
			set => Unsafe32->MajorImageVersion = value;
		}

		/// <summary />
		public ushort MinorImageVersion {
			get => Unsafe32->MinorImageVersion;
			set => Unsafe32->MinorImageVersion = value;
		}

		/// <summary />
		public ushort MajorSubsystemVersion {
			get => Unsafe32->MajorSubsystemVersion;
			set => Unsafe32->MajorSubsystemVersion = value;
		}

		/// <summary />
		public ushort MinorSubsystemVersion {
			get => Unsafe32->MinorSubsystemVersion;
			set => Unsafe32->MinorSubsystemVersion = value;
		}

		/// <summary />
		public uint Win32VersionValue {
			get => Unsafe32->Win32VersionValue;
			set => Unsafe32->Win32VersionValue = value;
		}
		/// <summary />
		public uint SizeOfImage {
			get => Unsafe32->SizeOfImage;
			set => Unsafe32->SizeOfImage = value;
		}

		/// <summary />
		public uint SizeOfHeaders {
			get => Unsafe32->SizeOfHeaders;
			set => Unsafe32->SizeOfHeaders = value;
		}

		/// <summary />
		public uint CheckSum {
			get => Unsafe32->CheckSum;
			set => Unsafe32->CheckSum = value;
		}

		/// <summary />
		public Subsystem Subsystem {
			get => (Subsystem)Unsafe32->Subsystem;
			set => Unsafe32->Subsystem = (ushort)value;
		}

		/// <summary />
		public DllCharacteristics DllCharacteristics {
			get => (DllCharacteristics)Unsafe32->DllCharacteristics;
			set => Unsafe32->DllCharacteristics = (ushort)value;
		}

		/// <summary />
		public ulong SizeOfStackReserve {
			get => !_is64Bit ? Unsafe32->SizeOfStackReserve : Unsafe64->SizeOfStackReserve;
			set {
				if (!_is64Bit)
					Unsafe32->SizeOfStackReserve = (uint)value;
				else
					Unsafe64->SizeOfStackReserve = value;
			}
		}

		/// <summary />
		public ulong SizeOfStackCommit {
			get => !_is64Bit ? Unsafe32->SizeOfStackCommit : Unsafe64->SizeOfStackCommit;
			set {
				if (!_is64Bit)
					Unsafe32->SizeOfStackCommit = (uint)value;
				else
					Unsafe64->SizeOfStackCommit = value;
			}
		}

		/// <summary />
		public ulong SizeOfHeapReserve {
			get => !_is64Bit ? Unsafe32->SizeOfHeapReserve : Unsafe64->SizeOfHeapReserve;
			set {
				if (!_is64Bit)
					Unsafe32->SizeOfHeapReserve = (uint)value;
				else
					Unsafe64->SizeOfHeapReserve = value;
			}
		}

		/// <summary />
		public ulong SizeOfHeapCommit {
			get => !_is64Bit ? Unsafe32->SizeOfHeapCommit : Unsafe64->SizeOfHeapCommit;
			set {
				if (!_is64Bit)
					Unsafe32->SizeOfHeapCommit = (uint)value;
				else
					Unsafe64->SizeOfHeapCommit = value;
			}
		}

		/// <summary />
		public uint LoaderFlags {
			get => !_is64Bit ? Unsafe32->LoaderFlags : Unsafe64->LoaderFlags;
			set {
				if (!_is64Bit)
					Unsafe32->LoaderFlags = value;
				else
					Unsafe64->LoaderFlags = value;
			}
		}

		public uint NumberOfRvaAndSizes {
			get => !_is64Bit ? Unsafe32->NumberOfRvaAndSizes : Unsafe64->NumberOfRvaAndSizes;
			set {
				if (!_is64Bit)
					Unsafe32->NumberOfRvaAndSizes = value;
				else
					Unsafe64->NumberOfRvaAndSizes = value;
			}
		}

		/// <summary />
		public DataDirectory* DataDirectories => !_is64Bit ? (DataDirectory*)RawValue32->DataDirectory : (DataDirectory*)RawValue64->DataDirectory;

		/// <summary />
		private IMAGE_OPTIONAL_HEADER32* Unsafe32 => (IMAGE_OPTIONAL_HEADER32*)_rawData;

		/// <summary />
		private IMAGE_OPTIONAL_HEADER64* Unsafe64 => (IMAGE_OPTIONAL_HEADER64*)_rawData;

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
