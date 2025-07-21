using MenuFilesGen.Models;
using MenuFilesGen.Service;
using System.Text;

namespace MenuFilesGen.Repositories
{
    public partial class CfgCreater
    {

        public CfgCreater(List<CommandDefinition> cmds, string _addonNameGlobal)
        {
            commandDefinitions = cmds;
            addonNameGlobal = _addonNameGlobal;

            Cfg = new CfgDefinition();//конфиг
                   
            cfgHeader();//осталась только лента

            PanelCmd();

            AppPanel();

            AppAddonPanel();

        }



        public CfgDefinition Cfg
        {
            get => _cfg;
            set => _cfg = value;
        }
        List<CommandDefinition> commandDefinitions { get; set; }
        string addonNameGlobal { get; set; }

        CfgDefinition _cfg;



    }
}
