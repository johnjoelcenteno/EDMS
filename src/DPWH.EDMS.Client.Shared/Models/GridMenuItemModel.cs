using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.SvgIcons;

namespace DPWH.EDMS.Client.Shared.Models
{
    public class GridMenuItemModel
    {
        public string Text { get; set; }
        public ISvgIcon Icon { get; set; }
        public Action Action { get; set; }
        public string CommandName { get; set; }
    }
}
