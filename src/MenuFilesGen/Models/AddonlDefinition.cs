using MenuFilesGen.Service;

namespace MenuFilesGen.Models
{
    /// <summary> Общие данные по палитрам инструментов  </summary>
    public class AddonDefinition
    {
        /// <summary> Родительская панель. Если null, то панель не является подчиненной </summary>
        public string Parent { get; set; }


        /// <summary> Имя аддона </summary>
        List<string> Names => NameRaw.RawSplit();

        public bool IsAddonSeparator => Names.Count > 1;

        public string Name => Names[0].Trim();

        /// <summary>
        /// с разделителем
        /// </summary>
        /// <value>
        /// The name raw.
        /// </value>
        public string NameRaw { get; set; }

        public List<PanelDefinition> Panel { get; set; }
    }
}
