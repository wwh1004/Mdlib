#if NEED_EXTENSION_ATTRIBUTE
namespace System.Runtime.CompilerServices {
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
	internal sealed class ExtensionAttribute : Attribute {
		public ExtensionAttribute() {
		}
	}
}
#endif
