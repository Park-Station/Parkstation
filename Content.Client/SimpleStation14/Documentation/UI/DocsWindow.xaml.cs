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

            // Add SetPage function to all buttons.
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
            // TODO: clear page content THEN add page content

            var Page = new WikiPages();
            var contents = Page.contents;

            foreach (var child in contents.Children)
            {
                // These are needed, if they don't exist for some reason stop
                if (child.Name == null || page.Name == null) return;

                // See if the button matches a content ID
                if (child.Name == $"{page.Name}Contents")
                {
                    // Add the content to the visible UI
                    child.Orphan();
                    Contens.AddChild(child);
                    // Stop looking for matches
                    return;
                }
            }
        }
    }
}
