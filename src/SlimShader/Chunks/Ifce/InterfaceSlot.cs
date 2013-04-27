using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimShader.Util;

namespace SlimShader.Chunks.Ifce
{
	public class InterfaceSlot
	{
		public uint StartSlot { get; set; }
		public uint SlotSpan { get; private set; }
		public List<uint> TypeIDs { get; private set; }
		public List<uint> TableIDs { get; private set; }

		public InterfaceSlot()
		{
			TypeIDs = new List<uint>();
			TableIDs = new List<uint>();
		}

		public static InterfaceSlot Parse(BytecodeReader reader, BytecodeReader interfaceSlotReader)
		{
			var slotSpan = interfaceSlotReader.ReadUInt32();

			var count = interfaceSlotReader.ReadUInt32();

			var typeIDsOffset = interfaceSlotReader.ReadUInt32();
			var typeIDsReader = reader.CopyAtOffset((int) typeIDsOffset);

			var tableIDsOffset = interfaceSlotReader.ReadUInt32();
			var tableIDsReader = reader.CopyAtOffset((int) tableIDsOffset);

			var result = new InterfaceSlot
			{
				SlotSpan = slotSpan
			};

			for (int i = 0; i < count; i++)
			{
				result.TypeIDs.Add(typeIDsReader.ReadUInt16());
				result.TableIDs.Add(tableIDsReader.ReadUInt32());
			}

			return result;
		}

		public override string ToString()
		{
			// For example:
			// | Type ID  |   0     |0    1    2    
			// | Table ID |         |0    1    2    
			// +----------+---------+---------------------------------------

			var sb = new StringBuilder();

			string slotSpan = (SlotSpan == 1) 
				? StartSlot.ToString() : 
				string.Format("{0}-{1}", StartSlot, StartSlot + SlotSpan - 1);
			sb.AppendLine(string.Format("// | Type ID  |   {0,-5} |{1}", slotSpan, 
				string.Join(" ", TypeIDs.Select(x => string.Format("{0,-4}", x)))));
			sb.AppendLine(string.Format("// | Table ID |         |{0}",
				string.Join(" ", TableIDs.Select(x => string.Format("{0,-4}", x)))));
			sb.AppendLine("// +----------+---------+---------------------------------------");
			return sb.ToString();
		}
	}
}