using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Mdlib.DotNet.Metadata;
using static Mdlib.PE.NativeConstants;

namespace Mdlib.PE {
	/// <summary>
	/// PE映像布局方式
	/// </summary>
	internal enum ImageLayout {
		/// <summary>
		/// 文件
		/// </summary>
		File,

		/// <summary>
		/// 内存
		/// </summary>
		Memory
	}

	/// <summary>
	/// PE映像接口
	/// </summary>
	internal interface IPEImage {
		/// <summary>
		/// 当前PE映像的原始数据
		/// </summary>
		IntPtr RawData { get; }

		/// <summary>
		/// 是否为64位PE头
		/// </summary>
		bool Is64Bit { get; }

		/// <summary>
		/// 是否为.NET程序集
		/// </summary>
		bool IsDotNet { get; }

		/// <summary>
		/// PE映像布局方式
		/// </summary>
		ImageLayout Layout { get; }

		/// <summary>
		/// Dos头
		/// </summary>
		DosHeader DosHeader { get; }

		/// <summary>
		/// Nt头
		/// </summary>
		NtHeader NtHeader { get; }

		/// <summary>
		/// 文件头
		/// </summary>
		FileHeader FileHeader { get; }

		/// <summary>
		/// 可选头
		/// </summary>
		OptionalHeader OptionalHeader { get; }

		/// <summary>
		/// 节头
		/// </summary>
		SectionHeader[] SectionHeaders { get; }

		/// <summary>
		/// 元数据
		/// </summary>
		/// <exception cref="InvalidOperationException">非.NET程序集时引发</exception>
		IMetadata Metadata { get; }

		/// <summary>
		/// FileOffset => RVA
		/// </summary>
		/// <param name="fileOffset">FileOffset</param>
		/// <returns></returns>
		RVA ToRVA(FileOffset fileOffset);

		/// <summary>
		/// RVA => FileOffset
		/// </summary>
		/// <param name="rva">RVA</param>
		/// <returns></returns>
		FileOffset ToFileOffset(RVA rva);

		/// <summary>
		/// 复制数据
		/// </summary>
		/// <param name="rva"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		byte[] Copy(RVA rva, uint length);

		/// <summary>
		/// 复制数据
		/// </summary>
		/// <param name="fileOffset"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		byte[] Copy(FileOffset fileOffset, uint length);

		/// <summary>
		/// 复制数据
		/// </summary>
		/// <param name="rva"></param>
		/// <param name="destination"></param>
		/// <param name="index"></param>
		/// <param name="length"></param>
		void CopyTo(RVA rva, byte[] destination, int index, uint length);

		/// <summary>
		/// 复制数据
		/// </summary>
		/// <param name="fileOffset"></param>
		/// <param name="destination"></param>
		/// <param name="index"></param>
		/// <param name="length"></param>
		void CopyTo(FileOffset fileOffset, byte[] destination, int index, uint length);

		/// <summary>
		/// 重新加载
		/// </summary>
		void Reload();
	}

	/// <summary>
	/// PE映像工厂类
	/// </summary>
	internal static unsafe class PEImageFactory {
		/// <summary>
		/// 创建 <see cref="IPEImage"/> 实例
		/// </summary>
		/// <param name="pPEImage"></param>
		/// <param name="imageLayout"></param>
		/// <returns></returns>
		public static IPEImage Create(IntPtr pPEImage, ImageLayout imageLayout) {
			return Create((void*)pPEImage, imageLayout);
		}

		/// <summary>
		/// 创建 <see cref="IPEImage"/> 实例
		/// </summary>
		/// <param name="pPEImage"></param>
		/// <param name="layout"></param>
		/// <returns></returns>
		public static IPEImage Create(void* pPEImage, ImageLayout layout) {
			if (pPEImage is null)
				throw new ArgumentNullException(nameof(pPEImage));
			switch (layout) {
			case ImageLayout.File:
			case ImageLayout.Memory:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(layout));
			}

			return new PEImage(pPEImage, layout);
		}
	}

	[DebuggerDisplay("PEImage({Layout}):[P:{Utils.PointerToString(RawData)} L:{Length}]")]
	internal sealed unsafe class PEImage : IPEImage {
		private readonly void* _rawData;
		private readonly ImageLayout _layout;
		private bool _is64Bit;
		private bool _isDotNet;
		private DosHeader _dosHeader;
		private NtHeader _ntHeader;
		private SectionHeader[] _sectionHeaders;
		private IMetadata _metadata;

		public IntPtr RawData => (IntPtr)_rawData;

		public bool Is64Bit => _is64Bit;

		public bool IsDotNet => _isDotNet;

		public ImageLayout Layout => _layout;

		public DosHeader DosHeader => _dosHeader;

		public NtHeader NtHeader => _ntHeader;

		public FileHeader FileHeader => _ntHeader.FileHeader;

		public OptionalHeader OptionalHeader => _ntHeader.OptionalHeader;

		public SectionHeader[] SectionHeaders => _sectionHeaders;

		public IMetadata Metadata {
			get {
				if (!_isDotNet)
					throw new InvalidOperationException();

				if (_metadata is null)
					_metadata = new Metadata(this);
				return _metadata;
			}
		}

		public PEImage(void* rawData, ImageLayout layout) {
			_rawData = rawData;
			_layout = layout;
			Initialize();
		}

		public RVA ToRVA(FileOffset fileOffset) {
			switch (_layout) {
			case ImageLayout.File:
				foreach (SectionHeader sectionHeader in _sectionHeaders)
					if (fileOffset >= sectionHeader.RawAddress && fileOffset < sectionHeader.RawAddress + sectionHeader.RawSize)
						return fileOffset - sectionHeader.RawAddress + sectionHeader.VirtualAddress;
				return (RVA)fileOffset;
			case ImageLayout.Memory:
				return (RVA)fileOffset;
			default:
				throw new ArgumentOutOfRangeException(nameof(_layout));
			}
		}

		public FileOffset ToFileOffset(RVA rva) {
			switch (_layout) {
			case ImageLayout.File:
				foreach (SectionHeader sectionHeader in _sectionHeaders)
					if (rva >= sectionHeader.VirtualAddress && rva < sectionHeader.VirtualAddress + Math.Max(sectionHeader.VirtualSize, sectionHeader.RawSize))
						return rva - sectionHeader.VirtualAddress + sectionHeader.RawAddress;
				return (FileOffset)rva;
			case ImageLayout.Memory:
				return (FileOffset)rva;
			default:
				throw new ArgumentOutOfRangeException(nameof(_layout));
			}
		}

		public void Reload() {
			_is64Bit = default;
			_isDotNet = default;
			_dosHeader = default;
			_ntHeader = default;
			_sectionHeaders = default;
			_metadata = default;
			Initialize();
		}

		private void Initialize() {
			_dosHeader = DosHeader.Create(this);
			if (_dosHeader is null)
				return;
			_ntHeader = NtHeader.Create(this);
			if (_ntHeader is null)
				return;
			_is64Bit = _ntHeader.OptionalHeader.Is64Bit;
			_sectionHeaders = new SectionHeader[_ntHeader.FileHeader.RawValue->NumberOfSections];
			for (uint i = 0; i < _sectionHeaders.Length; i++) {
				_sectionHeaders[i] = SectionHeader.Create(this, i);
				if (_sectionHeaders[i] is null) {
					_sectionHeaders = null;
					return;
				}
			}
			_isDotNet = _ntHeader.OptionalHeader.DataDirectories[IMAGE_DIRECTORY_ENTRY_COM_DESCRIPTOR].Address != 0;
		}

		public byte[] Copy(RVA rva, uint length) {
			byte[] buffer;

			buffer = new byte[length];
			CopyTo(rva, buffer, 0, length);
			return buffer;
		}

		public byte[] Copy(FileOffset fileOffset, uint length) {
			byte[] buffer;

			buffer = new byte[length];
			CopyTo(fileOffset, buffer, 0, length);
			return buffer;
		}

		public void CopyTo(RVA rva, byte[] destination, int index, uint length) {
			if (destination is null)
				throw new ArgumentNullException(nameof(destination));

			CopyTo(ToFileOffset(rva), destination, index, length);
		}

		public void CopyTo(FileOffset fileOffset, byte[] destination, int index, uint length) {
			if (destination is null)
				throw new ArgumentNullException(nameof(destination));

			Marshal.Copy((IntPtr)_rawData, destination, index, (int)length);
		}
	}
}
