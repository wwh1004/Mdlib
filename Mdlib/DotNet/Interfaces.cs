using Mdlib.DotNet.Metadata;

namespace Mdlib.DotNet {
	/// <summary />
	public interface IMetadataTokenProvider {
		/// <summary>
		/// 元数据标记
		/// </summary>
		MetadataToken MetadataToken { get; }
	}

	/// <summary>
	/// 编码标记 TypeDefOrRef 接口
	/// </summary>
	public interface ITypeDefOrRef {
		/// <summary>
		/// 编码标记标签
		/// </summary>
		uint TypeDefOrRefTag { get; }
	}

	/// <summary>
	/// 编码标记 HasConstant 接口
	/// </summary>
	public interface IHasConstant {
		/// <summary>
		/// 编码标记标签
		/// </summary>
		uint HasConstantTag { get; }
	}

	/// <summary>
	/// 编码标记 HasCustomAttribute 接口
	/// </summary>
	public interface IHasCustomAttribute {
		/// <summary>
		/// 编码标记标签
		/// </summary>
		uint HasCustomAttributeTag { get; }
	}

	/// <summary>
	/// 编码标记 HasFieldMarshal 接口
	/// </summary>
	public interface IHasFieldMarshal {
		/// <summary>
		/// 编码标记标签
		/// </summary>
		uint HasFieldMarshalTag { get; }
	}

	/// <summary>
	/// 编码标记 HasDeclSecurity 接口
	/// </summary>
	public interface IHasDeclSecurity {
		/// <summary>
		/// 编码标记标签
		/// </summary>
		uint HasDeclSecurityTag { get; }
	}

	/// <summary>
	/// 编码标记 MemberRefParent 接口
	/// </summary>
	public interface IMemberRefParent {
		/// <summary>
		/// 编码标记标签
		/// </summary>
		uint MemberRefParentTag { get; }
	}

	/// <summary>
	/// 编码标记 HasSemantic 接口
	/// </summary>
	public interface IHasSemantic {
		/// <summary>
		/// 编码标记标签
		/// </summary>
		uint HasSemanticTag { get; }
	}

	/// <summary>
	/// 编码标记 MethodDefOrRef 接口
	/// </summary>
	public interface IMethodDefOrRef {
		/// <summary>
		/// 编码标记标签
		/// </summary>
		uint MethodDefOrRefTag { get; }
	}

	/// <summary>
	/// 编码标记 MemberForwarded 接口
	/// </summary>
	public interface IMemberForwarded {
		/// <summary>
		/// 编码标记标签
		/// </summary>
		uint MemberForwardedTag { get; }
	}

	/// <summary>
	/// 编码标记 Implementation 接口
	/// </summary>
	public interface IImplementation {
		/// <summary>
		/// 编码标记标签
		/// </summary>
		uint ImplementationTag { get; }
	}

	/// <summary>
	/// 编码标记 CustomAttributeType 接口
	/// </summary>
	public interface ICustomAttributeType {
		/// <summary>
		/// 编码标记标签
		/// </summary>
		uint CustomAttributeTypeTag { get; }
	}

	/// <summary>
	/// 编码标记 ResolutionScope 接口
	/// </summary>
	public interface IResolutionScope {
		/// <summary>
		/// 编码标记标签
		/// </summary>
		uint ResolutionScopeTag { get; }
	}

	/// <summary>
	/// 编码标记 TypeOrMethodDef 接口
	/// </summary>
	public interface ITypeOrMethodDef {
		/// <summary>
		/// 编码标记标签
		/// </summary>
		uint TypeOrMethodDefTag { get; }
	}
}
