using NickBuhro.Translit;

namespace MenuFilesGen.Models
{
    /// <summary> Общие данные по палитрам инструментов  </summary>
    public class PanelDefinition
    {
        /// <summary> Родительская панель. Если null, то панель не является подчиненной </summary>
        public string Parent { get; set; }

        /// <summary> Имя панели </summary>
        public string Name { get; set; }

        /// <summary>
        /// Имя панели без пробелов
        /// </summary>
        /// <value>
        /// The name ru.
        /// </value>
        public string NameRu => Name.Replace(' ', '_');

        /// <summary>
        /// Имя панели без пробелов транслит
        /// </summary>
        /// <value>
        /// The panel name en.
        /// </value>
        public string NameEn => Transliteration.CyrillicToLatin(NameRu, Language.Russian);

        public string Intername => $"ShowToolbar_{NameEn}";
        public string LocalName => $"Панель_{NameRu}";

        public List<CommandDefinition> Command { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is panel added.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is panel added; otherwise, <c>false</c>.
        /// </value>
        public bool IsPanelAdded { get; set; }

        

    }
}
