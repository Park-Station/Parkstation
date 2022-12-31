using Content.Client.SimpleStation14.Documentation.UI.Pages;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.XAML;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface;

namespace Content.Client.SimpleStation14.Documentation.UI
{
    [GenerateTypedNameReferences]
    public sealed partial class DocsWindow : DefaultWindow
    {
        public DocsWindow()
        {
            RobustXamlLoader.Load(this);

            UICloseButton.OnPressed += _ => Close();

            foreach (Button? button in Categories.Children)
            {
                if (button != null)
                {
                    button.OnPressed += ev =>
                    {
                        if (ev.Button != null)
                        {
                            SetPage(ev.Button);
                        }
                    };
                }
            }
        }

        public void SetPage(BaseButton page)
        {
            var Page = new WikiPages();
            var contents = Page.contents;

            foreach (var child in contents.Children)
            {
                if (child.Name == null || page.Name == null) return;

                if (child.Name == $"{page.Name}Contents")
                {
                    child.Orphan();
                    Contens.AddChild(child);
                    return;
                }
            }
        }
    }
}
