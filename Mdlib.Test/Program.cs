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
				DosHeader dosHeader;
				NtHeader ntHeader;
				FileHeader fileHeader;
				OptionalHeader optionalHeader;
				SectionHeader[] sectionHeaders;
				IMetadata metadata;
				Cor20Header cor20Header;
				StorageSignature storageSignature;
				StorageHeader storageHeader;
				StreamHeader[] streamHeaders;
				TableStream tableStream;
				MetadataTable[] metadataTables;
				StringHeap stringHeap;
				UserStringHeap userStringHeap;
				GuidHeap guidHeap;
				BlobHeap blobHeap;

				dosHeader = peImage.DosHeader;
				ntHeader = peImage.NtHeader;
				fileHeader = peImage.FileHeader;
				optionalHeader = peImage.OptionalHeader;
				sectionHeaders = peImage.SectionHeaders;
				if (DEBUG) {
					Console.WriteLine("Section Names:");
					foreach (SectionHeader sectionHeader in sectionHeaders)
						Console.WriteLine("  " + sectionHeader.DisplayName);
					Console.WriteLine();
					Console.WriteLine($"IsDotNetImage: {peImage.IsDotNetImage.ToString()}");
				}
				metadata = peImage.Metadata;
				cor20Header = metadata.Cor20Header;
				storageSignature = metadata.StorageSignature;
				if (DEBUG) {
					Console.WriteLine();
					Console.WriteLine($"CLR Version: {storageSignature.DisplayVersionString}");
				}
				storageHeader = metadata.StorageHeader;
				streamHeaders = metadata.StreamHeaders;
				tableStream = metadata.TableStream;
				metadataTables = tableStream.Tables;
				if (DEBUG_DETAIL) {
					Console.WriteLine();
					Console.WriteLine("MetadataTables:");
					foreach (MetadataTable metadataTable in metadataTables)
						if (!(metadataTable is null))
							Console.WriteLine($"  {metadataTable.Type.ToString()}: RowSize={metadataTable.RowSize.ToString()} RowCount={metadataTable.RowCount.ToString()}");
				}
				foreach (MetadataTable metadataTable in metadataTables)
					if (!(metadataTable is null)) {
						TableRow[] tableRows;

						tableRows = metadataTable.Rows;
					}
				stringHeap = metadata.StringHeap;
				userStringHeap = metadata.UserStringHeap;
				guidHeap = metadata.GuidHeap;
				blobHeap = metadata.BlobHeap;
				Console.ReadKey(true);
			}
		}
	}
}
