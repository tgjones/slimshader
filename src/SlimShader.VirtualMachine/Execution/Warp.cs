using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SlimShader.VirtualMachine.Util;

namespace SlimShader.VirtualMachine.Execution
{
    /// <summary>
    /// A container class that groups together several execution contexts.
    /// </summary>
    public class Warp
    {
        private readonly BitArray _allOne;
        private readonly DivergenceStack _divergenceStack;

        public DivergenceStack DivergenceStack
        {
            get { return _divergenceStack; }
        }

        public Warp(int executionContextCount)
        {
            _allOne = BitArrayUtility.CreateAllOne(executionContextCount);

            _divergenceStack = new DivergenceStack();
            _divergenceStack.Push(0, _allOne, -1);
        }

        public static IList<ExecutionContext> GetActiveExecutionContexts(
            ExecutionContext[] executionContexts,
            DivergenceStackEntry divergenceStackEntry)
        {
            return executionContexts.Where(x => divergenceStackEntry.ActiveMask[x.Index]).ToList();
        }
    }
}