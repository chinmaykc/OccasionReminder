using FuncOccasionReminder.Model;
using FuncOccasionReminder.Utils;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FuncOccasionReminder.Occasions
{
    internal class OccasionRepoHandler
    {

        private static readonly string _jsonDBFilePath;
        private readonly List<OccasionModel> _OccasionList;

        static OccasionRepoHandler()
        {
            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _jsonDBFilePath = Path.GetFullPath(Path.Combine(binDirectory, $"..\\{AppConstants.DB_FILE_NAME}"));
        }

        public OccasionRepoHandler()
        {
            if (File.Exists(_jsonDBFilePath))
                _OccasionList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OccasionModel>>(File.ReadAllText(_jsonDBFilePath));
            else
                _OccasionList = new();
        }

        public List<OccasionModel> Occasions { get { return _OccasionList; } }
    }
}
