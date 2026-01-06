namespace Domain.Entities;

public class AppSettings
{
        public string SiteTitle { get; set; }
        public string LibraryName { get; set; }
        public string LibraryLogo { get; set; }
        public string LibraryLogoAlt { get; set; }
        public string LibrarySlogan { get; set; }
        public string FooterText { get; set; }
        public string SupportEmail { get; set; }
        public string SupportPhone { get; set; }
        public string LibraryHours { get; set; }
        public List<NavbarItem> NavbarItems { get; set; }
        public List<NavbarItem> AdminNavbarItems { get; set; }
        public List<QuickLink> QuickLinks { get; set; }
}

// En Domain.Entities o donde tengas NavbarItem
public class NavbarItem
{
    public string Text { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }
    public string Icon { get; set; }
    public string RequiredPolicy { get; set; } // Nueva propiedad
}

public class QuickLink
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
    }
    