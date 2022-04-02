using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosaic.Core
{
    public enum WidgetType
    {
        Native = 0,   // native
        Html = 1,     // html
        Generated = 2 // [r]untime generated widget such as web thumbnail or app shortcut
    }
}
