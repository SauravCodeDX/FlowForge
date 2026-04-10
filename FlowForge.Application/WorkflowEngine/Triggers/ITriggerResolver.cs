using FlowForge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowForge.Application.WorkflowEngine.Triggers
{
    public interface ITriggerResolver
    {
        Task<List<Workflow>> ResolveAsync(
            string eventName,
            string sourceSystem);
    }
}
