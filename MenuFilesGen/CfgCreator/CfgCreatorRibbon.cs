using MenuFilesGen.Models;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace MenuFilesGen.CFG
{
    public partial class CfgCreator
    {
        /// <summary>
        /// Заполняем ленту. Для каждого аддона создаётся отдельная вкладка.
        /// </summary>
        void Ribbon(bool isDuplicatePanel)
        {
            // Создание XML документа.
            XDoc = new XDocument();

            XElement ribbonRoot = new XElement("RibbonRoot");
            XDoc.Add(ribbonRoot);

            XElement ribbonPanelSourceCollection = new XElement("RibbonPanelSourceCollection");
            ribbonRoot.Add(ribbonPanelSourceCollection);

            XElement ribbonTabSourceCollection = new XElement("RibbonTabSourceCollection");
            ribbonRoot.Add(ribbonTabSourceCollection);

            foreach (AppDefinition app in groupingAppAddonPanel)
            {
                foreach (AddonDefinition addon in app.Addons)
                {
                    CommandDefinition firstCommand = addon.Panel
                        .SelectMany(panel => panel.Command)
                        .First();

                    // Если имя аддона не задано, используем имя приложения,
                    // а для старых таблиц без обоих значений — глобальное имя.
                    string tabName = GetDisplayName(addon.NameRaw, app.Name);

                    string tabUid = CreateRibbonUid("Tab", firstCommand.AppName, addon.NameRaw);

                    XElement ribbonTabSource = new XElement("RibbonTabSource");
                    ribbonTabSource.Add(new XAttribute("Text", tabName));
                    ribbonTabSource.Add(new XAttribute("UID", tabUid));
                    ribbonTabSourceCollection.Add(ribbonTabSource);

                    foreach (PanelDefinition panel in addon.Panel)
                    {
                        string panelUid = CreateRibbonUid(
                            "Panel",
                            firstCommand.AppName,
                            addon.NameRaw,
                            panel.NameRaw);

                        XElement ribbonPanelSource = new XElement("RibbonPanelSource");
                        ribbonPanelSource.Add(new XAttribute("UID", panelUid));
                        ribbonPanelSource.Add(new XAttribute("Text", GetDisplayName(panel.NameRaw)));
                        ribbonPanelSourceCollection.Add(ribbonPanelSource);

                        // Временный контейнер для сбора кнопок.
                        XElement panelButtons = new XElement("Temp");

                        // Группируем команды по тому, объединяются ли они в RibbonSplitButton.
                        List<IGrouping<string, CommandDefinition>> unitedCommands =
                            panel.Command.GroupBy(c => c.RibbonSplitButtonName).ToList();

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
                            XElement? ribbonRowPanel = null;
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
                                ribbonRowPanel!.Add(ribbonRow);

                                ribbonRowPanelButtonsCount++;
                            }
                        }

                        if (!isDuplicatePanel) // Если false, генерируем выпадающее меню под табом ленты.
                        {
                            XElement ribbonPanelBreak = new XElement("RibbonPanelBreak");
                            ribbonPanelSource.Add(ribbonPanelBreak);
                            XElement ribbonRowDuplicatePanel = new XElement("RibbonRowPanel");
                            ribbonPanelSource.Add(ribbonRowDuplicatePanel);

                            XElement[] items = panelButtons.Elements().ToArray();
                            int nameSymbolsCountMax = 0;

                            for (int itemIndex = 0; itemIndex < items.Count(); itemIndex += 2)
                            {
                                int nameSymbolsCount =
                                    items[itemIndex].Attributes().First(attr => attr.Name == "Text").Value.Count();

                                if (nameSymbolsCount > nameSymbolsCountMax)
                                    nameSymbolsCountMax = nameSymbolsCount;
                            }

                            for (int itemIndex = 0; itemIndex < items.Count(); itemIndex++)
                            {
                                XElement item = items[itemIndex];
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
                        }

                        XElement ribbonPanelSourceReference = new XElement("RibbonPanelSourceReference");
                        ribbonPanelSourceReference.Add(new XAttribute("PanelId", panelUid));
                        ribbonTabSource.Add(ribbonPanelSourceReference);
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает первое отображаемое имя из значения с разделителями.
        /// </summary>
        static string GetDisplayName(string? rawName, string fallback = "")
        {
            return string.IsNullOrWhiteSpace(rawName)
                ? fallback
                : rawName.Split('|')[0].Trim();
        }

        /// <summary>
        /// Создаёт стабильный UID из контекста приложения, аддона и панели.
        /// </summary>
        static string CreateRibbonUid(string prefix, params string?[] parts)
        {
            // Длина каждого значения исключает неоднозначность при конкатенации.
            string key = string.Concat(parts.Select(part =>
                part is null ? "-1:" : $"{part.Length}:{part}"));

            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(key));
            string suffix = Convert.ToHexString(hash, 0, 16);
            return $"{prefix}_{suffix}";
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
