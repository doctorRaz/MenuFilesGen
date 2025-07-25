using MenuFilesGen.Models;
using MenuFilesGen.Service;

namespace MenuFilesGen.CFG
{
    public partial class CfgCreator
    {
        /// <summary>
        /// Заполняем панельки, регистрируем команды
        /// </summary>
        void PanelCmd()
        {
            #region Формируем панельки и прописываем команды
            string panelNameEn = "";
            string panelName = "";
            string panelIntername = "";
            foreach (PanelDefinition panel in groupingPanel)
            {
                if (panelNameEn != panel.NameEn)
                {
                    panelNameEn = panel.NameEn;

                    panelName = panel.Name;
                    panelIntername = panel.Intername;


                    //  регистрация панельки
                    Cfg.ToolBars.Add("");
                    Cfg.ToolBars.Add($"[\\toolbars\\{panelNameEn}]");
                    Cfg.ToolBars.Add($"name=s{panel.Name}");

                    //регистрация ее команды
                    //+регистрируем команду вызова панели
                    Cfg.ToolBarsCmd.Add("");
                    Cfg.ToolBarsCmd.Add($"[\\configman\\commands\\{panelIntername}]");
                    Cfg.ToolBarsCmd.Add($"weight=i10");
                    Cfg.ToolBarsCmd.Add($"cmdtype=i0");
                    Cfg.ToolBarsCmd.Add($"Intername=s{panelIntername}");
                    Cfg.ToolBarsCmd.Add($"StatusText=sОтображение панели {panelName}");
                    Cfg.ToolBarsCmd.Add($"ToolTipText=sОтображение панели {panelName}");
                    Cfg.ToolBarsCmd.Add($"DispName=sОтображение панели {panelName}");
                    Cfg.ToolBarsCmd.Add($"LocalName=s{panel.LocalName}");

                    //добавлять к команде показа панели иконку, по первой команде панели
                    Cfg.ToolBarsCmd.AddRange(Utils.IconDefinition(panel.Command[0]));
                }

                foreach (CommandDefinition cmd in panel.Command)//по описаниям команд
                {
                    #region Регистрация команд

                    Cfg.Configman.Add("");
                    Cfg.Configman.Add($"[\\configman\\commands\\{cmd.InterName}]");
                    Cfg.Configman.Add($"weight=i{cmd.Weight}");
                    Cfg.Configman.Add($"cmdtype=i{cmd.CmdType}");
                    Cfg.Configman.Add($"intername=s{cmd.InterName}");
                    Cfg.Configman.Add($"DispName=s{cmd.DispName}");
                    Cfg.Configman.Add($"StatusText=s{cmd.StatusText}");

                    //необязательные ключи
                    if (!string.IsNullOrEmpty(cmd.ToolTipText)) Cfg.Configman.Add($"ToolTipText=s{cmd.ToolTipText}");
                    if (!string.IsNullOrEmpty(cmd.LocalName)) Cfg.Configman.Add($"LocalName=s{cmd.LocalName}");
                    if (!string.IsNullOrEmpty(cmd.RealCommandName)) Cfg.Configman.Add($"RealCommandName=s{cmd.RealCommandName}");
                    if (!string.IsNullOrEmpty(cmd.Keyword)) Cfg.Configman.Add($"Keyword=s{cmd.Keyword}");

                    //иконка                
                    Cfg.Configman.AddRange(Utils.IconDefinition(cmd));
                    #endregion

                    //хоткеи
                    if (!string.IsNullOrEmpty(cmd.Accelerators))
                    {
                        Cfg.Accelerators.Add($"{cmd.InterName}=s{cmd.Accelerators}");
                    }

                    if (cmd.HideCommand) continue;// не добавлять в меню пропуск

                    #region Панели

                    if (cmd.IsCommandSeparator)
                    {
                        Cfg.ToolBars.Add($"[\\toolbars\\{panel.NameEn}\\sep_{cmd.InterName}]");
                    }
                    Cfg.ToolBars.Add($"[\\toolbars\\{panel.NameEn}\\{cmd.InterName}]");
                    Cfg.ToolBars.Add($"Intername=s{cmd.InterName}");
                    #endregion
                }
            }

            #endregion
        }
    }
}
