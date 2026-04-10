using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowForge.Application.WorkflowEngine.Executors
{
    public interface IWorkflowExecutor
    {
        Task ExecuteAsync(
            string eventName,
            string sourceSystem,
            string payload);
    }
}
