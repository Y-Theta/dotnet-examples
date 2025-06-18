using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace form_test.Utils
{
    public interface IInputContainer
    {
        object GetResult();

        UIElement Content { get; }

        double? DesiredWidth { get; }

        double? DesiredHeight { get; }
    }
}
