using System;
using System.Runtime.InteropServices;
using System.Threading;
using Mdlib.DotNet.Metadata;
using Mdlib.PE;

namespace Mdlib.Test {
	internal static unsafe class Program {
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Unicode, EntryPoint = "GetModuleHandleW", ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		private static void Main(string[] args) {
			Console.ReadKey(true);
			MainImpl();
			Console.WriteLine();
			Console.WriteLine("完成");
			Console.ReadKey(true);
			GC.Collect();
			GC.WaitForPendingFinalizers();
			Console.WriteLine("Disposed");
			while (true)
				Thread.Sleep(int.MaxValue);
		}

		private static void MainImpl() {
			const bool DEBUG = true;
			const bool DEBUG_DETAIL = true;

			using (IPEImage peImage = PEImageFactory.Create("ExtremeDumper64.exe")) {
				DosHeader dosHeader = peImage.DosHeader;
				NtHeader ntHeader = peImage.NtHeader;
				FileHeader fileHeader = peImage.FileHeader;
				OptionalHeader optionalHeader = peImage.OptionalHeader;
				SectionHeader[] sectionHeaders = peImage.SectionHeaders;
				if (DEBUG) {
					Console.WriteLine("Section Names:");
					foreach (SectionHeader sectionHeader in sectionHeaders)
						Console.WriteLine("  " + sectionHeader.DisplayName);
					Console.WriteLine();
					Console.WriteLine($"IsDotNetImage: {peImage.IsDotNetImage.ToString()}");
				}
				IMetadata metadata = peImage.Metadata;
				Cor20Header cor20Header = metadata.Cor20Header;
				StorageSignature storageSignature = metadata.StorageSignature;
				if (DEBUG) {
					Console.WriteLine();
					Console.WriteLine($"CLR Version: {storageSignature.DisplayVersionString}");
				}
				StorageHeader storageHeader = metadata.StorageHeader;
				StreamHeader[] storageStreamHeaders = metadata.StreamHeaders;
				TableStream tableStream = metadata.TableStream;
				MetadataTable[] metadataTables = tableStream.Tables;
				if (DEBUG_DETAIL) {
					Console.WriteLine();
					Console.WriteLine("MetadataTables:");
					foreach (MetadataTable metadataTable in metadataTables)
						if (metadataTable != null)
							Console.WriteLine($"  {metadataTable.Type.ToString()}: RowSize={metadataTable.RowSize.ToString()} RowCount={metadataTable.RowCount.ToString()}");
				}
				foreach (MetadataTable metadataTable in metadataTables)
					if (metadataTable != null) {
						TableRow[] tableRows;

						tableRows = metadataTable.Rows;
					}
				StringHeap stringHeap = metadata.StringHeap;
				UserStringHeap userStringHeap = metadata.UserStringHeap;
				GuidHeap guidHeap = metadata.GuidHeap;
				BlobHeap blobHeap = metadata.BlobHeap;
				Console.ReadKey(true);
			}
		}
	}
}
