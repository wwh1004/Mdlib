using System;
using Mdlib.DotNet.Metadata;

namespace Mdlib.DotNet {
	/// <summary>
	/// 对应 <see cref="AssemblyRow"/> 的抽象类
	/// </summary>
	public abstract class AssemblyDef : IHasCustomAttribute, IHasDeclSecurity {
		#region mdfield
		/// <summary />
		protected AssemblyHashAlgorithm _hashAlgorithmId;
		/// <summary />
		protected Version _version;
		/// <summary />
		protected AssemblyAttributes _flags;
		/// <summary />
		protected byte[] _publicKey;
		/// <summary />
		protected string _name;
		/// <summary />
		protected string _locale;
		#endregion

		/// <summary />
		public uint HasCustomAttributeTag => 14;

		/// <summary />
		public uint HasDeclSecurityTag => 2;

		public virtual AssemblyHashAlgorithm HashAlgorithmId {
			get => _hashAlgorithmId;
			set => _hashAlgorithmId = value;
		}

		public virtual Version Version {
			get => _version;
			set => _version = value;
		}

		public virtual AssemblyAttributes Flags {
			get => _flags;
			set => _flags = value;
		}

		public virtual byte[] PublicKey {
			get => _publicKey;
			set => _publicKey = value;
		}

		public virtual string Name {
			get => _name;
			set => _name = value;
		}

		public virtual string Culture {
			get => _locale;
			set => _locale = value;
		}
	}

	/// <summary>
	/// 表示来自已有元数据的 <see cref="AssemblyDef"/>
	/// </summary>
	public sealed class AssemblyDefMD : AssemblyDef, IMetadataTokenProvider {
		private readonly MetadataToken _metadataToken;

		/// <summary />
		public MetadataToken MetadataToken => _metadataToken;
	}
}
