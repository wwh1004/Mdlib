#if NEED_EXTENSION_ATTRIBUTE
namespace System.Runtime.CompilerServices {
	/// <summary />
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
	internal sealed class ExtensionAttribute : Attribute {
		/// <summary />
		public ExtensionAttribute() {
		}
	}
}
#endif
