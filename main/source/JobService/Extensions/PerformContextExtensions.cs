using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Server;

namespace JobService.Extensions
{
    public static class PerformContextExtensions
    {
        private static Dictionary<string, dynamic> _viewBags = new Dictionary<string, dynamic>();

        public static dynamic GetViewBag(this PerformContext context)
        {
            string id = context.BackgroundJob.Id;
            if (_viewBags.ContainsKey(id))
            {
                _viewBags.Add(id, new System.Dynamic.ExpandoObject());
            }
            return _viewBags[id];
        }

        public static void RemoveViewBag(this PerformContext context)
        {
            string id = context.BackgroundJob.Id;
            if (_viewBags.ContainsKey(id))
            {
                _viewBags.Remove(id);
            }
        }
    }
}
