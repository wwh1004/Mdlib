using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Mdlib.DotNet.Metadata;
using Mdlib.PE;

namespace Mdlib.Test {
	internal static class Program {
#pragma warning disable IDE0059
		private static void Main() {
			int indent;
			IPEImage peImage;
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
			StringHeap stringHeap;
			UserStringHeap userStringHeap;
			GuidHeap guidHeap;
			BlobHeap blobHeap;

			indent = 0;
			peImage = PEImageFactory.Create(Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().ManifestModule), ImageLayout.Memory);
			dosHeader = peImage.DosHeader;
			ntHeader = peImage.NtHeader;
			fileHeader = ntHeader.FileHeader;
			optionalHeader = ntHeader.OptionalHeader;
			sectionHeaders = peImage.SectionHeaders;
			Print("SectionHeaders:", indent);
			indent += 2;
			foreach (SectionHeader sectionHeader in sectionHeaders) {
				PrintSectionHeader(sectionHeader, indent);
				PrintNewLine();
			}
			indent -= 2;
			metadata = peImage.Metadata;
			cor20Header = metadata.Cor20Header;
			storageSignature = metadata.StorageSignature;
			Print($"CLR Version: {storageSignature.DisplayVersionString}", indent);
			storageHeader = metadata.StorageHeader;
			streamHeaders = metadata.StreamHeaders;
			Print("StreamHeaders:", indent);
			indent += 2;
			foreach (StreamHeader streamHeader in streamHeaders) {
				PrintStreamHeader(streamHeader, indent);
				PrintNewLine();
			}
			indent -= 2;
			tableStream = metadata.TableStream;
			stringHeap = metadata.StringHeap;
			userStringHeap = metadata.UserStringHeap;
			guidHeap = metadata.GuidHeap;
			blobHeap = metadata.BlobHeap;
			peImage.Reload();
			Console.ReadKey(true);
			while (true)
				Thread.Sleep(int.MaxValue);
		}
#pragma warning restore IDE0059

		private static unsafe void PrintSectionHeader(SectionHeader value, int indent) {
			PrintRawData(value, indent);
			Print($"Name: {value.DisplayName}", indent);
			Print($"VirtualAddress: {value.RawValue->VirtualAddress:X8}", indent);
			Print($"VirtualSize: {value.RawValue->VirtualSize:X8}", indent);
			Print($"RawAddress: {value.RawValue->VirtualAddress:X8}", indent);
			Print($"RawSize: {value.RawValue->SizeOfRawData:X8}", indent);
		}

		private static void PrintStreamHeader(StreamHeader value, int indent) {
			PrintRawData(value, indent);
			Print($"Name: {value.DisplayName}", indent);
			Print($"Offset: {value.Offset:X8}", indent);
			Print($"Size: {value.Size:X8}", indent);
		}

		private static void PrintRawData(IRawData value, int indent) {
			Print($"IRawData:{{ RVA: {(uint)value.RVA:X8} FileOffset: {(uint)value.FileOffset:X8} Length: {value.Length:X8} }}", indent);
		}

		private static void Print(string value, int indent) {
			Console.WriteLine(new string(' ', indent) + value);
		}

		private static void PrintNewLine() {
			Console.WriteLine();
		}
	}
}
