using Hints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardcoreSL.Util
{
    public static class HintsUtil
    {

        public static void ShowText(this HintDisplay hints, string text, float duration)
        {
            hints.Show(new TextHint(text, new HintParameter[] { new StringHintParameter("") }, null, duration));
        }

    }
}
