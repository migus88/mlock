using Migs.Examples.Shared;
using Migs.MLock.Examples.Car.UiElements.Code.Controls;
using UnityEngine;
using UnityEngine.UIElements;

namespace Migs.Examples.Examples.MLockExample.Runtime
{
    /// <summary>
    /// Builds the UI programmatically using UIElements for the MLock Car Example
    /// </summary>
    public class CarExampleUiBuilder : MonoBehaviour
    {
        public const string LanguageTooltipName = "language-tooltip";
        public const string MenuPopupName = "menu-popup";
        public const string MenuButtonName = "menu-button";
        public const string TempLockButtonName = "temp-lock-button";
        public const string ExitButtonName = "exit-button";
        public const string SelectLanguageButtonName = "select-language-button";
        public const string MusicVolumeSliderName = "music-volume-slider";
        public const string AnalyticsToggleName = "analytics-toggle";
        
        public const string EnglishLanguageButtonName = "english-button";
        public const string DeutschLanguageButtonName = "deutsch-button";
        public const string FrenchLanguageButtonName = "french-button";
        
        [SerializeField] private UIDocument _uiDocument;
        
        public void BuildUI()
        {
            var root = _uiDocument.rootVisualElement;
            root.Clear();
            root.panel.visualTree.RegisterCallback<NavigationMoveEvent>(e => e.StopImmediatePropagation(), TrickleDown.TrickleDown);
            
            // Build all UI components
            var menuButton = CreateMenuButton();
            var menuPopup = CreateMenuPopup();
            var tempLockButton = CreateTempLockButton();

            // Add to root
            root.Add(menuButton);
            root.Add(menuPopup);
            root.Add(tempLockButton);
        }
        
        private Button CreateMenuButton()
        {
            var button = new LockableButton(CarLockTags.GameHud) { text = "Menu", name = MenuButtonName };
            
            button.style.position = Position.Absolute;
            button.style.top = 10;
            button.style.left = 10;
            button.style.width = 100;
            button.style.height = 40;
            button.style.fontSize = 16;
            button.style.unityFontStyleAndWeight = FontStyle.Bold;
            button.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            button.style.color = Color.white;
            button.style.borderTopLeftRadius = 5;
            button.style.borderTopRightRadius = 5;
            button.style.borderBottomLeftRadius = 5;
            button.style.borderBottomRightRadius = 5;
            
            return button;
        }

        private Button CreateTempLockButton()
        {
            var button = new LockableButton(CarLockTags.GameHud) { text = "Temp Lock", name = TempLockButtonName };

            button.style.position = Position.Absolute;
            button.style.top = 10;
            button.style.right = 10;
            button.style.width = 100;
            button.style.height = 40;
            button.style.fontSize = 16;
            button.style.unityFontStyleAndWeight = FontStyle.Bold;
            button.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            button.style.color = Color.white;
            button.style.borderTopLeftRadius = 5;
            button.style.borderTopRightRadius = 5;
            button.style.borderBottomLeftRadius = 5;
            button.style.borderBottomRightRadius = 5;

            return button;
        }
        
        private VisualElement CreateMenuPopup()
        {
            var menuPopup = new VisualElement() { name = MenuPopupName };
            
            // Style menu popup
            menuPopup.style.position = Position.Absolute;
            menuPopup.style.top = new UnityEngine.UIElements.Length(50, UnityEngine.UIElements.LengthUnit.Percent);
            menuPopup.style.left = new UnityEngine.UIElements.Length(50, UnityEngine.UIElements.LengthUnit.Percent);
            menuPopup.style.marginLeft = -150;
            menuPopup.style.marginTop = -200;
            menuPopup.style.width = 300;
            menuPopup.style.height = 400;
            menuPopup.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            menuPopup.style.borderTopLeftRadius = 10;
            menuPopup.style.borderTopRightRadius = 10;
            menuPopup.style.borderBottomLeftRadius = 10;
            menuPopup.style.borderBottomRightRadius = 10;
            menuPopup.style.paddingTop = 40;
            menuPopup.style.paddingBottom = 20;
            menuPopup.style.paddingLeft = 20;
            menuPopup.style.paddingRight = 20;
            menuPopup.style.display = DisplayStyle.None;
            
            // Create menu title
            var title = new Label("Game Menu");
            title.style.fontSize = 24;
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.color = Color.white;
            title.style.unityTextAlign = TextAnchor.MiddleCenter;
            title.style.marginBottom = 20;
            
            // Create exit button
            var exitButton = new LockableButton(CarLockTags.MainMenu) { name = ExitButtonName };
            exitButton.style.position = Position.Absolute;
            exitButton.style.top = 10;
            exitButton.style.right = 10;
            exitButton.style.width = 30;
            exitButton.style.height = 30;
            exitButton.style.backgroundColor = new Color(0.8f, 0.2f, 0.2f, 0.8f);
            exitButton.style.borderTopLeftRadius = 15;
            exitButton.style.borderTopRightRadius = 15;
            exitButton.style.borderBottomLeftRadius = 15;
            exitButton.style.borderBottomRightRadius = 15;
            exitButton.style.alignItems = UnityEngine.UIElements.Align.Center;
            exitButton.style.justifyContent = UnityEngine.UIElements.Justify.Center;

            var exitIcon = new Label("X");
            exitIcon.style.unityTextAlign = TextAnchor.MiddleCenter;
            exitIcon.style.fontSize = 16;
            exitIcon.style.unityFontStyleAndWeight = FontStyle.Bold;
            exitIcon.style.color = Color.white;
            
            exitButton.Add(exitIcon);
            
            // Create menu container for menu items
            var menuContainer = new VisualElement();
            menuContainer.style.flexGrow = 1;
            
            // Add language selection
            var languageButton = CreateMenuItem("Select Language", SelectLanguageButtonName);
            var languageTooltip = CreateLanguageTooltip();
            
            // Add volume slider
            var volumeContainer = new VisualElement();
            volumeContainer.style.flexDirection = FlexDirection.Row;
            volumeContainer.style.justifyContent = Justify.SpaceBetween;
            volumeContainer.style.alignItems = Align.Center;
            volumeContainer.style.marginTop = 20;
            volumeContainer.style.marginBottom = 20;
            
            var volumeLabel = new Label("Music Volume");
            volumeLabel.style.color = Color.white;
            volumeLabel.style.fontSize = 16;
            volumeLabel.style.flexGrow = 0;
            volumeLabel.style.width = 120;
            
            var volumeSlider = new LockableSlider(CarLockTags.MainMenu, 0, 100) { name = MusicVolumeSliderName, value = 75 };
            volumeSlider.style.flexGrow = 1;
            
            volumeContainer.Add(volumeLabel);
            volumeContainer.Add(volumeSlider);
            
            // Add analytics toggle
            var analyticsContainer = new VisualElement();
            analyticsContainer.style.flexDirection = FlexDirection.Row;
            analyticsContainer.style.justifyContent = Justify.SpaceBetween;
            analyticsContainer.style.alignItems = Align.Center;
            analyticsContainer.style.marginTop = 20;
            analyticsContainer.style.marginBottom = 20;
            
            var analyticsLabel = new Label("Send Analytics");
            analyticsLabel.style.color = Color.white;
            analyticsLabel.style.fontSize = 16;
            
            var analyticsToggle = new LockableToggle(CarLockTags.MainMenu) { name = AnalyticsToggleName, value = true };
            
            analyticsContainer.Add(analyticsLabel);
            analyticsContainer.Add(analyticsToggle);
            
            // Add all menu items to container
            menuContainer.Add(languageButton);
            menuContainer.Add(languageTooltip);
            menuContainer.Add(volumeContainer);
            menuContainer.Add(analyticsContainer);
            
            // Add all elements to menu popup
            menuPopup.Add(title);
            menuPopup.Add(exitButton);
            menuPopup.Add(menuContainer);
            
            return menuPopup;
        }
        
        private Button CreateMenuItem(string text, string name)
        {
            var button = new LockableButton(CarLockTags.MainMenu) { text = text, name = name };
            
            button.style.height = 40;
            button.style.fontSize = 16;
            button.style.color = Color.white;
            button.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.8f);
            button.style.borderTopLeftRadius = 5;
            button.style.borderTopRightRadius = 5;
            button.style.borderBottomLeftRadius = 5;
            button.style.borderBottomRightRadius = 5;
            button.style.marginTop = 10;
            button.style.marginBottom = 10;
            
            return button;
        }
        
        private VisualElement CreateLanguageTooltip()
        {
            var tooltip = new VisualElement() { name = LanguageTooltipName };
            
            // Style tooltip
            tooltip.style.position = Position.Absolute;
            tooltip.style.top = 0;
            tooltip.style.right = -220;
            tooltip.style.width = 200;
            tooltip.style.backgroundColor = new Color(0.25f, 0.25f, 0.25f, 0.95f);
            tooltip.style.borderTopLeftRadius = 5;
            tooltip.style.borderTopRightRadius = 5;
            tooltip.style.borderBottomLeftRadius = 5;
            tooltip.style.borderBottomRightRadius = 5;
            tooltip.style.paddingTop = 10;
            tooltip.style.paddingBottom = 10;
            tooltip.style.paddingLeft = 10;
            tooltip.style.paddingRight = 10;
            tooltip.style.display = DisplayStyle.None;
            
            // Create language options
            var englishButton = CreateLanguageOption("English", EnglishLanguageButtonName);
            var deutschButton = CreateLanguageOption("Deutsch", DeutschLanguageButtonName);
            var frenchButton = CreateLanguageOption("French", FrenchLanguageButtonName);
            
            tooltip.Add(englishButton);
            tooltip.Add(deutschButton);
            tooltip.Add(frenchButton);
            
            return tooltip;
        }
        
        private Button CreateLanguageOption(string language, string name)
        {
            var button = new LockableButton(CarLockTags.None) { text = language, name = name };
            
            button.style.height = 30;
            button.style.fontSize = 14;
            button.style.color = Color.white;
            button.style.backgroundColor = new Color(0.4f, 0.4f, 0.4f, 0.8f);
            button.style.borderTopLeftRadius = 5;
            button.style.borderTopRightRadius = 5;
            button.style.borderBottomLeftRadius = 5;
            button.style.borderBottomRightRadius = 5;
            button.style.marginTop = 5;
            button.style.marginBottom = 5;
            
            return button;
        }
    }
}