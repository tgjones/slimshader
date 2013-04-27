using System.Collections.Generic;

using System.Linq;
using System.Text;
using SlimShader.Util;

namespace SlimShader.Chunks.Ifce
{
	public class InterfacesChunk : BytecodeChunk
	{
		public uint InterfaceSlotCount { get; private set; }
		public List<ClassType> AvailableClassTypes { get; private set; }
		public List<ClassInstance> AvailableClassInstances { get; private set; }
		public List<InterfaceSlot> InterfaceSlots { get; private set; }

		public InterfacesChunk()
		{
			AvailableClassTypes = new List<ClassType>();
			AvailableClassInstances = new List<ClassInstance>();
			InterfaceSlots = new List<InterfaceSlot>();
		}

		public static InterfacesChunk Parse(BytecodeReader reader, uint sizeInBytes)
		{
			var headerReader = reader.CopyAtCurrentPosition();

			var result = new InterfacesChunk();

			var classInstanceCount = headerReader.ReadUInt32();
			var classTypeCount = headerReader.ReadUInt32();
			var interfaceSlotRecordCount = headerReader.ReadUInt32();

			// Will be same as interfaceSlotRecordCount unless there are interface arrays.
			result.InterfaceSlotCount = headerReader.ReadUInt32();

			headerReader.ReadUInt32(); // Think this is offset to start of interface slot info, but we don't need it.

			var classTypeOffset = headerReader.ReadUInt32();
			var availableClassReader = reader.CopyAtOffset((int) classTypeOffset);

			var interfaceSlotOffset = headerReader.ReadUInt32();
			var interfaceSlotReader = reader.CopyAtOffset((int) interfaceSlotOffset);

			for (uint i = 0; i < classTypeCount; i++)
			{
				var classType = ClassType.Parse(reader, availableClassReader);
				classType.ID = i; // Really??
				result.AvailableClassTypes.Add(classType);
			}

			for (uint i = 0; i < classInstanceCount; i++)
			{
				var classInstance = ClassInstance.Parse(reader, availableClassReader);
				result.AvailableClassInstances.Add(classInstance);
			}

			uint startSlot = 0;
			for (uint i = 0; i < interfaceSlotRecordCount; i++)
			{
				var interfaceSlot = InterfaceSlot.Parse(reader, interfaceSlotReader);
				interfaceSlot.StartSlot = startSlot; // Really??
				result.InterfaceSlots.Add(interfaceSlot);

				startSlot += interfaceSlot.SlotSpan;
			}

			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine("//");
			sb.AppendLine("// Available Class Types:");
			sb.AppendLine("//");
			sb.AppendLine("// Name                             ID CB Stride Texture Sampler");
			sb.AppendLine("// ------------------------------ ---- --------- ------- -------");

			foreach (var classType in AvailableClassTypes)
				sb.AppendLine("// " + classType);

			sb.AppendLine("//");

			if (AvailableClassInstances.Any())
			{
				sb.AppendLine("// Available Class Instances:");
				sb.AppendLine("//");
				sb.AppendLine("// Name                        Type CB CB Offset Texture Sampler");
				sb.AppendLine("// --------------------------- ---- -- --------- ------- -------");

				foreach (var classInstance in AvailableClassInstances)
					sb.AppendLine("// " + classInstance);

				sb.AppendLine("//");
			}

			sb.AppendLine(string.Format("// Interface slots, {0} total:", InterfaceSlotCount));
			sb.AppendLine("//");
			sb.AppendLine("//             Slots");
			sb.AppendLine("// +----------+---------+---------------------------------------");

			foreach (var interfaceSlot in InterfaceSlots)
				sb.Append(interfaceSlot);

			return sb.ToString();
		}
	}
}