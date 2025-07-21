using MenuFilesGen.Models;
using MenuFilesGen.Service;

namespace MenuFilesGen.Repositories
{
    public partial class CfgCreater
    {
        /// <summary>
        /// Заполняем панельки, регистрируем команды
        /// </summary>
        void PanelCmd()
        {
            #region Формируем панельки и прописываем команды


            //todo группировка по панелькам
            foreach (PanelDefinition panel in groupingPanel)
            {
                //  регистрация панельки
                Cfg.Toolbars.Add($"[\\toolbars\\{panel.NameEn}]");
                Cfg.Toolbars.Add($"name=s{panel.Name}]");

                //регистрация ее команды
                //+регистрируем команду вызова панели
                Cfg.ToolbarsCmd.Add("");
                Cfg.ToolbarsCmd.Add($"[\\configman\\commands\\{panel.Intername}]");
                Cfg.ToolbarsCmd.Add($"weight=i10");
                Cfg.ToolbarsCmd.Add($"cmdtype=i0");
                Cfg.ToolbarsCmd.Add($"Intername=s{panel.Intername}");
                Cfg.ToolbarsCmd.Add($"StatusText=sОтображение панели {panel.Name}");
                Cfg.ToolbarsCmd.Add($"ToolTipText=sОтображение панели {panel.Name}");
                Cfg.ToolbarsCmd.Add($"DispName=sОтображение панели {panel.Name}");
                Cfg.ToolbarsCmd.Add($"LocalName=s{panel.LocalName}");

                //добавлять к команде показа панели иконку, по первой команде панели
                Cfg.ToolbarsCmd.AddRange(Utils.IconDefinition(panel.Command[0]));

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
                    Cfg.Toolbars.Add($"[\\toolbars\\{panel.NameEn}\\{cmd.InterName}]");
                    Cfg.Toolbars.Add($"Intername=s{cmd.InterName}");
                    #endregion
                }
            }

            #endregion
        }
    }
}
