using System.Collections.Generic;
using System.Linq;

namespace FuncReminder.Utils
{
    public static class Extensions
    {
        public static string ToFirstUpper(this string input)
        {
            List<string> inputList = input.ToLower().Split(' ').ToList();
            
            var res = inputList.Select(x => $"{x[0].ToString().ToUpper()}{x.Substring(1)}").ToList();
            return string.Join(" ", res);
        }
    }
}
