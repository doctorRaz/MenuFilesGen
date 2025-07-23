using MenuFilesGen.Models;

namespace MenuFilesGen.CFG
{
    public partial class CfgCreater
    {
        //группировка по PanelName        
        /// <summary>
        /// группировка по PanelName для ленты  
        /// </summary>
        /// <value>
        /// The groups panel.
        /// </value>
        List<IGrouping<string, CommandDefinition>> groupsPanel => commandDefinitions
                                                                .GroupBy(e => e.PanelName).ToList();

        // https://stackoverflow.com/questions/1159233/multi-level-grouping-in-linq        
        /// <summary>
        /// Группируем по приложению, аддону , панелям
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
                     .GroupBy(e => e.AddonNameRaw)
                     .Select(addon => new AddonDefinition
                     {
                         Parent = string.IsNullOrEmpty(appName.Key) ? addonNameGlobal : appName.Key,
                         NameRaw = addon.Key,
                         Panel = addon
                         .GroupBy(e => e.PanelNameRaw)
                         .Select(panel => new PanelDefinition
                         {
                             Parent = addon.Key,
                             NameRaw = panel.Key,
                             Command = panel.ToList() /*new List<CommandDefinition>(panel)*/,
                         }).ToList()
                     }).ToList()
                     }).ToList();

            }
        }

        /// <summary>
        /// группировка по приложению, панелям
        /// </summary>
        /// <value>
        /// The grouping application panel.
        /// </value>
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
                      .GroupBy(e => e.PanelNameRaw)
                         .Select(panel => new PanelDefinition
                         {
                             Parent = appName.Key,
                             NameRaw = panel.Key,
                             Command = panel.ToList() /*new List<CommandDefinition>(panel)*/,
                         }).ToList()

                     }).ToList();

            }
        }

        /// <summary>
        /// группировка по панелям
        /// </summary>
        /// <value>
        /// The grouping panel.
        /// </value>
        List<PanelDefinition> groupingPanel
        {
            get
            {
                return commandDefinitions

                      .GroupBy(e => e.PanelNameRaw)
                         .Select(panel => new PanelDefinition
                         {
                             NameRaw = panel.Key,
                             Command = panel.ToList(),
                         }).ToList();

            }
        }
    }
}
