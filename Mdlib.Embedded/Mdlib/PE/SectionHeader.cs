using System;
using System.Diagnostics;
using System.Text;
using static Mdlib.PE.NativeConstants;

namespace Mdlib.PE {
	/// <summary>
	/// 节头
	/// </summary>
	[DebuggerDisplay("SectHdr:[P:{Utils.PointerToString(RawData)} RVA:{RVA} FileOffset:{FileOffset} N:{DisplayName}]")]
	internal sealed unsafe class SectionHeader : IRawData<IMAGE_SECTION_HEADER> {
		private void* _rawData;
		private uint _offset;
		private string _displayName;

		/// <summary />
		public IntPtr RawData => (IntPtr)_rawData;

		/// <summary />
		public IMAGE_SECTION_HEADER* RawValue => (IMAGE_SECTION_HEADER*)_rawData;

		/// <summary />
		public RVA RVA => (RVA)_offset;

		/// <summary />
		public FileOffset FileOffset => (FileOffset)_offset;

		/// <summary />
		public uint Length => IMAGE_SECTION_HEADER.UnmanagedSize;

		/// <summary />
		public byte* Name => RawValue->Name;

		/// <summary />
		public uint VirtualSize {
			get => RawValue->VirtualSize;
			set => RawValue->VirtualSize = value;
		}

		/// <summary />
		public RVA VirtualAddress {
			get => (RVA)RawValue->VirtualAddress;
			set => RawValue->VirtualAddress = (uint)value;
		}

		/// <summary />
		public uint RawSize {
			get => RawValue->SizeOfRawData;
			set => RawValue->SizeOfRawData = value;
		}

		/// <summary />
		public FileOffset RawAddress {
			get => (FileOffset)RawValue->PointerToRawData;
			set => RawValue->PointerToRawData = (uint)value;
		}

		/// <summary />
		public uint Characteristics {
			get => RawValue->Characteristics;
			set => RawValue->Characteristics = value;
		}

		/// <summary />
		public string DisplayName {
			get {
				if (_displayName is null)
					RefreshCache();
				return _displayName;
			}
		}

		private SectionHeader() {
		}

		public static SectionHeader Create(IPEImage peImage, uint index) {
			if (peImage is null)
				throw new ArgumentNullException(nameof(peImage));

			FileHeader fileHeader;
			uint offset;
			SectionHeader sectionHeader;

			fileHeader = peImage.NtHeader.FileHeader;
			offset = (uint)fileHeader.FileOffset + IMAGE_FILE_HEADER.UnmanagedSize + fileHeader.RawValue->SizeOfOptionalHeader + (index * IMAGE_SECTION_HEADER.UnmanagedSize);
			if (!Utils.IsValidPointer((byte*)peImage.RawData + offset, IMAGE_SECTION_HEADER.UnmanagedSize))
				return null;
			sectionHeader = new SectionHeader {
				_offset = offset,
				_rawData = (byte*)peImage.RawData + offset
			};
			return sectionHeader;
		}

		/// <summary>
		/// 刷新内部字符串缓存，这些缓存被Display*属性使用。若更改了字符串相关内容，请调用此方法。
		/// </summary>
		public void RefreshCache() {
			byte* pName;
			StringBuilder builder;

			pName = (byte*)_rawData;
			builder = new StringBuilder(IMAGE_SIZEOF_SHORT_NAME);
			for (ushort i = 0; i < IMAGE_SIZEOF_SHORT_NAME; i++) {
				if (pName[i] == 0)
					break;
				builder.Append((char)pName[i]);
			}
			_displayName = builder.ToString();
		}
	}
}
