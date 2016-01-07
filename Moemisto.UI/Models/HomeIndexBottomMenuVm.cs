using System.Collections.Generic;

namespace Moemisto.UI.Models
{
    public class HomeIndexBottomMenuVm
    {
        public Dictionary<string, string> News { get; set; }
        public Dictionary<string, string> Events { get; set; }
        public Dictionary<string, string> Travels { get; set; }
        public Dictionary<string, string> Places { get; set; }
    }
}