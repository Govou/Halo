using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloBiz.Helpers
{
    public enum AppDefinedHttpStatusCodes : int
    {
        TOKEN_SUPPLIED_BUT_INVALID = 440,
        NO_REFRESH_TOKEN_SUPPLIED = 441,
        NO_ACCESS_TOKEN_SUPPLIED = 442,
        NOT_AUTHORIZED = 443,
        ACCESS_CODE_SENT_BUT_UNUSED = 444,
        LOGIN_AGAIN = 445
    }
}
