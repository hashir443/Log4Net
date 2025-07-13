using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Enums;

namespace Entity.Entity;

public class BaseRequest
{
    public int ID { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public int LastUpdatedBy { get; set; }
    public RecordStatus Status { get; set; }
}
