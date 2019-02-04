using System;
using System.Diagnostics;
using Mdlib.PE;

namespace Mdlib.DotNet.Metadata {
	/// <summary>
	/// 元数据流
	/// </summary>
	[DebuggerDisplay("MdStm:[P:{Utils.PointerToString(RawData)} RVA:{RVA} FOA:{FOA} L:{Length}]")]
	public abstract unsafe class MetadataStream : IRawData {
		/// <summary />
		protected readonly IPEImage _peImage;
		/// <summary />
		protected readonly void* _rawData;
		/// <summary />
		protected readonly uint _offset;
		/// <summary />
		protected readonly uint _length;

		/// <summary />
		public IntPtr RawData => (IntPtr)_rawData;

		/// <summary />
		public RVA RVA => _peImage.ToRVA((FOA)_offset);

		/// <summary />
		public FOA FOA => (FOA)_offset;

		/// <summary />
		public uint Length => _length;

		internal MetadataStream(IMetadataManagement metadataManagement, int index) : this(metadataManagement, metadataManagement.StreamHeaders[index]) {
		}

		internal MetadataStream(IMetadataManagement metadataManagement, StreamHeader header) {
			if (metadataManagement == null)
				throw new ArgumentNullException(nameof(metadataManagement));
			if (header == null)
				throw new ArgumentNullException(nameof(header));

			_peImage = metadataManagement.PEImage;
			_offset = (uint)metadataManagement.StorageSignature.FOA + header.Offset;
			_rawData = (byte*)_peImage.RawData + _offset;
			_length = header.Size;
		}
	}
}
