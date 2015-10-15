using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subset
{
    public static class VisibilityMatrixExtension
    {
        public static string Show(this Dictionary<bool, Dictionary<VisibilityLevel, List<string>>> matrix)
        {
            StringBuilder outputString = new StringBuilder();

            foreach (bool bypassValue in matrix.Keys)
            {
                foreach (VisibilityLevel visibilityLevel in matrix[bypassValue].Keys)
                {
                    outputString.AppendLine(
                        string.Format("{0}\t\t {1}\t\t\t {2}",
                            bypassValue,
                            visibilityLevel,
                            string.Join(",", matrix[bypassValue][visibilityLevel])
                        )
                        );
                }
            }

            return outputString.ToString();
        }
    }
}
