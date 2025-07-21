using MenuFilesGen.Models;
using MenuFilesGen.Service;
using System.Text;

namespace MenuFilesGen.Repositories
{
    public partial class CfgCreater
    {
         

        // https://stackoverflow.com/questions/1159233/multi-level-grouping-in-linq        
        /// <summary>
        /// Группируем по приложеию, AppName , по панелям
        /// </summary>
        /// <value>
        /// The hierarchical grouping.
        /// </value>
        List<AppDefinition> groupingAppAddonPanel
        {
            get
            {

                return commandDefinitions
                     .GroupBy(e => e.AppName)
                     .Select(appName => new AppDefinition
                     {
                         Name = string.IsNullOrEmpty(appName.Key) ? addonNameGlobal : appName.Key,
                         Addons = appName
                     .GroupBy(e => e.AddonName)
                     .Select(addon => new AddonDefinition
                     {
                         Parent = string.IsNullOrEmpty(appName.Key) ? addonNameGlobal : appName.Key,
                         Name = addon.Key,
                         Panel = addon
                         .GroupBy(e => e.PanelName)
                         .Select(panel => new PanelDefinition
                         {
                             Parent = addon.Key,
                             Name = panel.Key,
                             Command = panel.ToList() /*new List<CommandDefinition>(panel)*/,
                         }).ToList()
                     }).ToList()
                     }).ToList();

            }
        }

        List<AppDefinition> groupingAppPanel
        {
            get
            {

                return commandDefinitions
                     .GroupBy(e => e.AppName)
                     .Select(appName => new AppDefinition
                     {
                         Name = string.IsNullOrEmpty(appName.Key) ? addonNameGlobal : appName.Key,
                         Panels = appName
                      .GroupBy(e => e.PanelName)
                         .Select(panel => new PanelDefinition
                         {
                             Parent = appName.Key,
                             Name = panel.Key,
                             Command = panel.ToList() /*new List<CommandDefinition>(panel)*/,
                         }).ToList()

                     }).ToList();

            }
        }

        List<PanelDefinition> groupingPanel
        {
            get
            {
                return commandDefinitions

                      .GroupBy(e => e.PanelName)
                         .Select(panel => new PanelDefinition
                         {
                             Name = panel.Key,
                             Command = panel.ToList(),
                         }).ToList();

            }
        } 



    }
}
