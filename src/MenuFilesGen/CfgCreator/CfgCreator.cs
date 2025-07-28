using MenuFilesGen.Models;

namespace MenuFilesGen.CFG
{
    public partial class CfgCreator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CfgCreator"/> class.
        /// </summary>
        /// <param name="cmds">The CMDS.</param>
        /// <param name="_addOnNameGlobal">The addon name global.</param>
        public CfgCreator(List<CommandDefinition> cmds, string _addOnNameGlobal)
        {
            commandDefinitions = cmds;
            addonNameGlobal = _addOnNameGlobal;

            Cfg = new CfgDefinition(addonNameGlobal);//конфиг

            PanelCmd();

            AppPanel();

            AppAddonPanel();

            Ribbon();

        }


        /// <summary>
        /// Gets or sets the CFG.
        /// </summary>
        /// <value>
        /// The CFG.
        /// </value>
        public CfgDefinition Cfg
        {
            get => _cfg;
            set => _cfg = value;
        }

        List<CommandDefinition> commandDefinitions { get; set; }

        /// <summary>
        /// "глобальное" имя аддона, имя файла cfg и ленты, root menu если не задано в шаблоне
        /// </summary>
        /// <value>
        /// The addon name global.
        /// </value>
        string addonNameGlobal { get; set; }

        CfgDefinition _cfg;



    }
}
