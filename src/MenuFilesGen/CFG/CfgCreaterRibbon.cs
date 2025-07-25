using MenuFilesGen.Models;
using System.Xml.Linq;

namespace MenuFilesGen.CFG
{
    public partial class CfgCreater
    {
        /// <summary>
        /// Заполняем ленту
        /// </summary>
        void Ribbon()
        {
            #region Ribbon
            // Ленточное меню
            //Создание XML документа 
            /*XDocument*/
            XDoc = new XDocument();

            //Корневой элемент
            XElement ribbonRoot = new XElement("RibbonRoot");
            XDoc.Add(ribbonRoot);

            XElement ribbonPanelSourceCollection = new XElement("RibbonPanelSourceCollection");
            ribbonRoot.Add(ribbonPanelSourceCollection);

            XElement ribbonTabSourceCollection = new XElement("RibbonTabSourceCollection");
            ribbonRoot.Add(ribbonTabSourceCollection);

            XElement ribbonTabSource = new XElement("RibbonTabSource");
            ribbonTabSource.Add(new XAttribute("Text", addonNameGlobal));
            ribbonTabSource.Add(new XAttribute("UID", $"{addonNameGlobal.Replace(" ", "")}_Tab"));
            ribbonTabSourceCollection.Add(ribbonTabSource);

            foreach (IGrouping<string, CommandDefinition> cmd in groupsPanel)
            {
                XElement ribbonPanelSource = new XElement("RibbonPanelSource");
                ribbonPanelSource.Add(new XAttribute("UID", cmd.Key));
                ribbonPanelSource.Add(new XAttribute("Text", cmd.Key));
                ribbonPanelSourceCollection.Add(ribbonPanelSource);

                // Временный контейнер для сбора кнопок
                XElement panelButtons = new XElement("Temp");

                // Группируем команды по тому, объединяются ли они в RibbonSplitButton5
                List<IGrouping<string, CommandDefinition>> unitedCommands = cmd.GroupBy(c => c.RibbonSplitButtonName).ToList();

                foreach (IGrouping<string, CommandDefinition> unitedCommandGroup in unitedCommands)
                {
                    XElement container = panelButtons;
                    if (!string.IsNullOrWhiteSpace(unitedCommandGroup.Key))
                    {
                        XElement ribbonSplitButton = new XElement("RibbonSplitButton");
                        ribbonSplitButton.Add(new XAttribute("Text", unitedCommandGroup.Key));
                        ribbonSplitButton.Add(new XAttribute("Behavior", "SplitFollowStaticText"));
                        ribbonSplitButton.Add(new XAttribute("ButtonStyle", unitedCommandGroup.First().RibbonSize));

                        panelButtons.Add(ribbonSplitButton);
                        container = ribbonSplitButton;
                    }

                    foreach (CommandDefinition commandData in unitedCommandGroup)
                    {
                        if (commandData.HideCommand) continue;
                        container.Add(CreateButton(commandData));
                    }
                }

                Dictionary<bool, IGrouping<bool, XElement>> sortedButtons = panelButtons
                    .Elements()
                    .GroupBy(button => button.Attributes().First(attr => attr.Name == "ButtonStyle").Value.Contains("Small"))
                    .ToDictionary(g => g.Key);

                if (sortedButtons.ContainsKey(false))
                {
                    foreach (XElement button in sortedButtons[false])
                        ribbonPanelSource.Add(button);
                }

                if (sortedButtons.ContainsKey(true))
                {
                    XElement ribbonRowPanel = null;
                    int ribbonRowPanelButtonsCount = 3;

                    foreach (XElement button in sortedButtons[true])
                    {
                        if (ribbonRowPanelButtonsCount == 3)
                        {
                            ribbonRowPanel = new XElement("RibbonRowPanel");
                            ribbonPanelSource.Add(ribbonRowPanel);
                            ribbonRowPanelButtonsCount = 0;
                        }

                        XElement ribbonRow = new XElement("RibbonRow");
                        ribbonRow.Add(button);
                        ribbonRowPanel.Add(ribbonRow);

                        ribbonRowPanelButtonsCount++;
                    }
                }

                // Дублирование под табом ленты
                XElement ribbonPanelBreak = new XElement("RibbonPanelBreak");
                ribbonPanelSource.Add(ribbonPanelBreak);
                var ribbonRowDuplicatePanel = new XElement("RibbonRowPanel");
                ribbonPanelSource.Add(ribbonRowDuplicatePanel);

                var items = panelButtons.Elements().ToArray();
                int nameSymbolsCountMax = 0;

                for (int itemIndex = 0; itemIndex < items.Count(); itemIndex += 2)
                {
                    var nameSymbolsCount =
                        items[itemIndex].Attributes().First(attr => attr.Name == "Text").Value.Count();

                    if (nameSymbolsCount > nameSymbolsCountMax)
                        nameSymbolsCountMax = nameSymbolsCount;
                }

                for (int itemIndex = 0; itemIndex < items.Count(); itemIndex++)
                {
                    var item = items[itemIndex];
                    XElement[] itemButtons;

                    if (item.Name == "RibbonSplitButton")
                        itemButtons = item.Elements().ToArray();
                    else
                    {
                        itemButtons = new[]
                        {
                                      item,
                                  };
                    }

                    for (int buttonIndex = 0; buttonIndex < itemButtons.Count(); buttonIndex++)
                    {
                        var button = itemButtons[buttonIndex];
                        button.Attributes().First(attr => attr.Name == "ButtonStyle").Value = "LargeWithHorizontalText";
                        ribbonRowDuplicatePanel.Add(button);

                        if (itemIndex < items.Count() - 1 || buttonIndex < itemButtons.Count() - 1)
                        {
                            XElement separator = new XElement("RibbonSeparator");
                            ribbonRowDuplicatePanel.Add(separator);
                        }
                    }
                }



                XElement ribbonPanelSourceReference = new XElement("RibbonPanelSourceReference");
                ribbonPanelSourceReference.Add(new XAttribute("PanelId", cmd.Key));
                ribbonTabSource.Add(ribbonPanelSourceReference);
            }

            #endregion
        }
        public static XElement CreateButton(CommandDefinition commandData)
        {
            XElement ribbonCommandButton = new XElement("RibbonCommandButton");
            ribbonCommandButton.Add(new XAttribute("Text", commandData.DispName));
            ribbonCommandButton.Add(new XAttribute("ButtonStyle", commandData.RibbonSize));
            ribbonCommandButton.Add(new XAttribute("MenuMacroID", commandData.InterName));
            return ribbonCommandButton;
        }

        public XDocument XDoc { get; private set; }

    }
}
