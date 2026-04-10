using FlowForge.Application.WorkflowEngine.Context;
using FlowForge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowForge.Application.WorkflowEngine.Steps
{
    public interface IStepExecutor
    {
        Task ExecuteAsync(
            WorkflowStep step,
            WorkflowExecutionContext context);
    }
}
