using System;
using Mdlib.DotNet.Metadata;

namespace Mdlib.DotNet {
	/// <summary>
	/// 对应 <see cref="AssemblyRefRow"/> 的抽象类
	/// </summary>
	public abstract class AssemblyRef : IHasCustomAttribute, IHasDeclSecurity {
		#region mdfield
		/// <summary />
		protected Version _version;
		/// <summary />
		protected AssemblyAttributes _flags;
		/// <summary />
		protected byte[] _publicKeyOrToken;
		/// <summary />
		protected string _name;
		/// <summary />
		protected string _locale;
		/// <summary />
		protected byte[] _hashValue;
		#endregion

		/// <summary />
		public uint HasCustomAttributeTag => 14;

		/// <summary />
		public uint HasDeclSecurityTag => 2;

		public virtual Version Version {
			get => _version;
			set => _version = value;
		}

		public virtual AssemblyAttributes Flags {
			get => _flags;
			set => _flags = value;
		}

		public virtual byte[] PublicKeyOrToken {
			get => _publicKeyOrToken;
			set => _publicKeyOrToken = value;
		}

		public virtual string Name {
			get => _name;
			set => _name = value;
		}

		public virtual string Culture {
			get => _locale;
			set => _locale = value;
		}

		public virtual byte[] HashValue {
			get => _hashValue;
			set => _hashValue = value;
		}
	}

	/// <summary>
	/// 表示来自已有元数据的 <see cref="AssemblyRef"/>
	/// </summary>
	public sealed class AssemblyRefMD : AssemblyRef, IMetadataTokenProvider {
		private readonly MetadataToken _metadataToken;

		/// <summary />
		public MetadataToken MetadataToken => _metadataToken;
	}
}
