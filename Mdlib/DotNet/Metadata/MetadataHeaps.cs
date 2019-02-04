namespace Mdlib.DotNet.Metadata {
	/// <summary>
	/// #Strings堆
	/// </summary>
	public sealed class StringHeap : MetadataStream {
		internal StringHeap(IMetadataManagement metadataManagement, int index) : base(metadataManagement, index) {
		}
	}

	/// <summary>
	/// #US堆
	/// </summary>
	public sealed class UserStringHeap : MetadataStream {
		internal UserStringHeap(IMetadataManagement metadataManagement, int index) : base(metadataManagement, index) {
		}
	}

	/// <summary>
	/// #GUID堆
	/// </summary>
	public sealed class GuidHeap : MetadataStream {
		internal GuidHeap(IMetadataManagement metadataManagement, int index) : base(metadataManagement, index) {
		}
	}

	/// <summary>
	/// #Blob堆
	/// </summary>
	public sealed class BlobHeap : MetadataStream {
		internal BlobHeap(IMetadataManagement metadataManagement, int index) : base(metadataManagement, index) {
		}
	}

	/// <summary>
	/// 未知元数据堆
	/// </summary>
	public sealed class UnknownHeap : MetadataStream {
		/// <summary>
		/// 构造器
		/// </summary>
		/// <param name="metadataManagement">元数据管理接口</param>
		/// <param name="index">堆的索引</param>
		public UnknownHeap(IMetadataManagement metadataManagement, int index) : base(metadataManagement, index) {
		}

		/// <summary>
		/// 构造器
		/// </summary>
		/// <param name="metadataManagement">元数据管理接口</param>
		/// <param name="header">元数据流头</param>
		public UnknownHeap(IMetadataManagement metadataManagement, StreamHeader header) : base(metadataManagement, header) {
		}
	}
}
