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

			using (IPEImage peImage = PEImageFactory.Create(@"..\..\TestFile\ExtremeDumper64.exe")) {
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
				IMetadataManagement metadataManagement = peImage.MetadataManagement;
				Cor20Header cor20Header = metadataManagement.Cor20Header;
				StorageSignature storageSignature = metadataManagement.StorageSignature;
				if (DEBUG) {
					Console.WriteLine();
					Console.WriteLine($"CLR Version: {storageSignature.DisplayVersionString}");
				}
				StorageHeader storageHeader = metadataManagement.StorageHeader;
				StreamHeader[] storageStreamHeaders = metadataManagement.StreamHeaders;
				TableStream tableStream = metadataManagement.TableStream;
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
				StringHeap stringHeap = metadataManagement.StringHeap;
				UserStringHeap userStringHeap = metadataManagement.UserStringHeap;
				GuidHeap guidHeap = metadataManagement.GuidHeap;
				BlobHeap blobHeap = metadataManagement.BlobHeap;
				Console.ReadKey(true);
			}
		}
	}
}
