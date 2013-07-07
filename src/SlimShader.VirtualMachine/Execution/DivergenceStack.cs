using System.Collections;
using System.Collections.Generic;

namespace SlimShader.VirtualMachine.Execution
{
    public class DivergenceStack
    {
        private readonly Stack<DivergenceStackEntry> _stack;

        public DivergenceStack()
        {
            _stack = new Stack<DivergenceStackEntry>();
        }

        public void Push(int nextPC, BitArray activeMask, int reconvergencePC)
        {
            _stack.Push(new DivergenceStackEntry(nextPC, activeMask, reconvergencePC));
        }

        public DivergenceStackEntry Peek()
        {
            return _stack.Peek();
        }

        public DivergenceStackEntry Pop()
        {
            return _stack.Pop();
        }
    }
}