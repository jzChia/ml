using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLCommon.Entities
{
    public class ModelDisplay
    {
        public String mdoelName { get; set; }
        public String keyWords { get; set; }
        public int keyWordsCount { get; set; }
        public int factorCount { get; set; }
        public double dataMatch { get; set; }
        public double ddgzwzMatch { get; set; }
        public double cksdMatch { get; set; }
        public double sbMatch { get; set; }
        public double cycMatch { get; set; }

    }
}
