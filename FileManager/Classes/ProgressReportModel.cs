using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    public class ProgressReportModel
    {
        public List<WebsiteDataModel> SitesDownloaded { get; set; } = new List<WebsiteDataModel>();

        public int ProcentageComplete { get; set; } = 0;

    }
}
