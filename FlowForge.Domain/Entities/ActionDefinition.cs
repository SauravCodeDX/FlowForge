using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowForge.Domain.Entities
{
    //SendEmail
    //SendSlackMessage
    //CreateCRMRecord
    //CreateJiraTicket
    public class ActionDefinition
    {
        public Guid Id { get; internal set; }

        public string Name { get; internal set; } = default!;

        public string ActionType { get; internal set; } = default!;
    }
}
