using System;
using System.Runtime.ExceptionServices;

namespace Mdlib {
	internal static unsafe class Utils {
		[HandleProcessCorruptedStateExceptions]
		public static bool IsValidPointer(void* p, uint size) {
			if (p is null)
				return false;

			try {
				_ = *(byte*)p;
				_ = *((byte*)p + size - 1);
				return true;
			}
			catch (AccessViolationException) {
				return false;
			}
		}

		public static uint AlignUp(uint value, uint alignment) {
			return (value + alignment - 1) & ~(alignment - 1);
		}

		public static string PointerToString(IntPtr value) {
			return "0x" + ((ulong)value > uint.MaxValue ? value.ToString("X16") : value.ToString("X8"));
		}
	}
}
