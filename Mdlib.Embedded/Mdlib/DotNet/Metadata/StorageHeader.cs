using System;
using System.Diagnostics;
using Mdlib.PE;

namespace Mdlib.DotNet.Metadata {
	/// <summary>
	/// 存储头
	/// </summary>
	[DebuggerDisplay("StgHdr:[P:{Utils.PointerToString(RawData)} RVA:{RVA} FileOffset:{FileOffset} SC:{StreamCount}]")]
	internal sealed unsafe class StorageHeader : IRawData<STORAGEHEADER> {
		private readonly IPEImage _peImage;
		private readonly void* _rawData;
		private readonly uint _offset;

		/// <summary />
		public IntPtr RawData => (IntPtr)_rawData;

		/// <summary />
		public STORAGEHEADER* RawValue => (STORAGEHEADER*)_rawData;

		/// <summary />
		public RVA RVA => _peImage.ToRVA((FileOffset)_offset);

		/// <summary />
		public FileOffset FileOffset => (FileOffset)_offset;

		/// <summary />
		public uint Length => STORAGEHEADER.UnmanagedSize;

		/// <summary />
		public byte Flags {
			get => RawValue->fFlags;
			set => RawValue->fFlags = value;
		}

		/// <summary />
		public byte Padding {
			get => RawValue->pad;
			set => RawValue->pad = value;
		}

		/// <summary />
		public ushort StreamCount {
			get => RawValue->iStreams;
			set => RawValue->iStreams = value;
		}

		internal StorageHeader(IMetadata metadata) {
			if (metadata is null)
				throw new ArgumentNullException(nameof(metadata));

			_peImage = metadata.PEImage;
			_offset = (uint)metadata.StorageSignature.FileOffset + metadata.StorageSignature.Length;
			_rawData = (byte*)_peImage.RawData + _offset;
		}
	}
}
