using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omega_Sudoku.Helpers
{
    internal class Enum
    {
        public enum Result
        {
            NoChange,        // No hidden pairs were applied; state remains unchanged.
            Changed,         // Hidden pairs were applied and state updated successfully.
            Contradiction    // A contradiction was detected; branch should backtrack.
        }

    }
}