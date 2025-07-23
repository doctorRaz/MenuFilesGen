using MenuFilesGen.Models;

namespace MenuFilesGen.CFG
{
    public partial class CfgCreater
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CfgCreater"/> class.
        /// </summary>
        /// <param name="cmds">The CMDS.</param>
        /// <param name="_addonNameGlobal">The addon name global.</param>
        public CfgCreater(List<CommandDefinition> cmds, string _addonNameGlobal)
        {
            commandDefinitions = cmds;
            addonNameGlobal = _addonNameGlobal;

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
