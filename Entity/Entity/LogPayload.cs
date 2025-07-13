using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums;

namespace Entity.Entity;

public class LogPayload: BaseRequest
{
    public string MethodName { get; set; } = string.Empty;
    public string LogMessage { get; set; } = string.Empty;
    public string ExtraInformation { get; set; } = string.Empty;
    public LogType LogType { get; set; }
}
