using System;
using System.Diagnostics;
using Mdlib.PE;

namespace Mdlib.DotNet.Metadata {
	/// <summary>
	/// 元数据流
	/// </summary>
	[DebuggerDisplay("MdStm:[P:{Utils.PointerToString(RawData)} RVA:{RVA} FileOffset:{FileOffset} L:{Length}]")]
	internal abstract unsafe class MetadataStream : IRawData {
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
		public RVA RVA => _peImage.ToRVA((FileOffset)_offset);

		/// <summary />
		public FileOffset FileOffset => (FileOffset)_offset;

		/// <summary />
		public uint Length => _length;

		internal MetadataStream(IMetadata metadata, int index) : this(metadata, metadata.StreamHeaders[index]) {
		}

		internal MetadataStream(IMetadata metadata, StreamHeader header) {
			if (metadata is null)
				throw new ArgumentNullException(nameof(metadata));
			if (header is null)
				throw new ArgumentNullException(nameof(header));

			_peImage = metadata.PEImage;
			_offset = (uint)metadata.StorageSignature.FileOffset + header.Offset;
			_rawData = (byte*)_peImage.RawData + _offset;
			_length = header.Size;
		}
	}

	/// <summary>
	/// 元数据表流
	/// </summary>
	internal sealed unsafe class TableStream : MetadataStream {
		private readonly bool _isCompressed;
		private readonly bool _isBigString;
		private readonly bool _isBigGuid;
		private readonly bool _isBigBlob;

		/// <summary>
		/// 元数据架构主版本
		/// </summary>
		public byte MajorVersion {
			get => *((byte*)_rawData + 4);
			set => *((byte*)_rawData + 4) = value;
		}

		/// <summary>
		/// 元数据架构次版本
		/// </summary>
		public byte MinorVersion {
			get => *((byte*)_rawData + 5);
			set => *((byte*)_rawData + 5) = value;
		}

		/// <summary>
		/// 堆二进制标志
		/// </summary>
		public HeapFlags Flags {
			get => *((HeapFlags*)_rawData + 6);
			set => *((HeapFlags*)_rawData + 6) = value;
		}

		/// <summary>
		/// 表示存在哪些表
		/// </summary>
		public ulong ValidMask {
			get => *(ulong*)((byte*)_rawData + 8);
			set => *(ulong*)((byte*)_rawData + 8) = value;
		}

		/// <summary>
		/// 表示哪些表被排序了
		/// </summary>
		public ulong SortedMask {
			get => *(ulong*)((byte*)_rawData + 16);
			set => *(ulong*)((byte*)_rawData + 16) = value;
		}

		/// <summary />
		public IPEImage PEImage => _peImage;

		/// <summary>
		/// 是否是压缩的元数据表
		/// </summary>
		public bool IsCompressed => _isCompressed;

		/// <summary />
		public bool IsBigString => _isBigString;

		/// <summary />
		public bool IsBigGuid => _isBigGuid;

		/// <summary />
		public bool IsBigBlob => _isBigBlob;

		internal TableStream(IMetadata metadata, int index, bool isCompressed) : base(metadata, index) {
			_isCompressed = isCompressed;
			_isBigString = (Flags & HeapFlags.BigString) != 0;
			_isBigGuid = (Flags & HeapFlags.BigGuid) != 0;
			_isBigBlob = (Flags & HeapFlags.BigBlob) != 0;
		}
	}

	/// <summary>
	/// #Strings堆
	/// </summary>
	internal sealed class StringHeap : MetadataStream {
		internal StringHeap(IMetadata metadata, int index) : base(metadata, index) {
		}
	}

	/// <summary>
	/// #US堆
	/// </summary>
	internal sealed class UserStringHeap : MetadataStream {
		internal UserStringHeap(IMetadata metadata, int index) : base(metadata, index) {
		}
	}

	/// <summary>
	/// #GUID堆
	/// </summary>
	internal sealed class GuidHeap : MetadataStream {
		internal GuidHeap(IMetadata metadata, int index) : base(metadata, index) {
		}
	}

	/// <summary>
	/// #Blob堆
	/// </summary>
	internal sealed class BlobHeap : MetadataStream {
		internal BlobHeap(IMetadata metadata, int index) : base(metadata, index) {
		}
	}

	/// <summary>
	/// 未知元数据堆
	/// </summary>
	internal sealed class UnknownHeap : MetadataStream {
		/// <summary>
		/// 构造器
		/// </summary>
		/// <param name="metadata">元数据</param>
		/// <param name="index">堆的索引</param>
		public UnknownHeap(IMetadata metadata, int index) : base(metadata, index) {
		}

		/// <summary>
		/// 构造器
		/// </summary>
		/// <param name="metadata">元数据</param>
		/// <param name="header">元数据流头</param>
		public UnknownHeap(IMetadata metadata, StreamHeader header) : base(metadata, header) {
		}
	}
}
