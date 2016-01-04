using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbUnit.Settings
{
    public class IntegrationTestSettings
    {
        public bool SaveOutput { get; set; }

        public List<DatabaseConfig> Databases { get; set; }
    }
}
