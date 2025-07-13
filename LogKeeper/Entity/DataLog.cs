using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogKeeper.Entity
{
    public class DataLog
    {
        public string Message { get; set; } = string.Empty;
        public string Level { get; set; } = "Info"; // Optional: Info, Error, etc.
        public string Source { get; set; } = "App"; // Optional
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
