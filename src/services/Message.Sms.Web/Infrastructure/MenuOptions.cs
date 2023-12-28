namespace Message.Sms.Web.Infrastructure
{
    public class MenuOptions
    {
        public static List<MenuDate> Get(bool? isAdmin = false)
        {
            if (isAdmin == false)
            {
                return new List<MenuDate>
                {
                    new MenuDate
                    {
                        Name = "Home",
                        Icon = "bx bxs-home",
                        Href = "",
                        ChildNode = new List<MenuDate>()
                        {
                            new MenuDate
                            {
                                Name = "Project",
                                Icon = "",
                                Href = "/project/index"
                            },
                            new MenuDate
                            {
                                Name = "User Profile",
                                Icon = "",
                                Href = "/users/details"
                            },
                        new MenuDate
                        {
                            Name = "MobleHistory",
                            Icon = "",
                            Href = "/channel/phoneNumberLogs",
                            IsAdmin = true,
                        },
                        }
                    },
                    
                };
            }


            return new List<MenuDate>
            {
                new MenuDate
                {
                    Name = "Dashboard",
                    Icon = "bx bxs-home",
                    Href = "/Home/Index",
                    IsAdmin = true
                },
                new MenuDate
                {
                    Name = "Project",
                    Icon = "bx bxs-bolt",
                    Href = "",
                    ChildNode = new List<MenuDate>
                    {
                        new MenuDate
                        {
                            Name = "Project",
                            Icon = "",
                            Href = "/project/index"
                        },
                        new MenuDate
                        {
                            Name = "Project Details",
                            Icon = "",
                            Href = "/channel/index",
                            IsAdmin = true,
                        },
                        new MenuDate
                        {
                            Name = "New Project",
                            Icon = "",
                            Href = "/project/create",
                            IsAdmin = true,
                        },
                    },
                },
                new MenuDate
                {
                    Name = "Account",
                    Icon = "bx bxs-user",
                    Href = "",
                    ChildNode = new List<MenuDate>
                    {
                        new MenuDate
                        {
                            Name = "Users",
                            Icon = "",
                            Href = "/users/index",
                            IsAdmin = true,
                        },
                        new MenuDate
                        {
                            Name = "PhoneNumberLog",
                            Icon = "",
                            Href = "/channel/phoneNumberLogs",
                            IsAdmin = true,
                        },
                        new MenuDate
                        {
                            Name = "User Profile",
                            Icon = "",
                            Href = "/users/details"
                        },
                    },
                },
                new MenuDate
                {
                    Name = "Settings",
                    Icon = "bx bx-cog mr-10",
                    Href = "",
                    IsAdmin = true,
                    ChildNode = new List<MenuDate>
                    {
                        new MenuDate
                        {
                            Name = "Open SDK",
                            Icon = "",
                            Href = "/opensdk/index",
                            IsAdmin = true,
                        },
                        new MenuDate
                        {
                            Name = "Recharge Card",
                            Icon = "",
                            Href = "/rechargeCard/index",
                            IsAdmin = true,
                        },
                    },
                },
            };
        }
    }

    public class MenuDate
    {
        public string Icon { get; set; }

        public string Name { get; set; }

        public string Href { get; set; }

        public bool IsAdmin { get; set; }

        public List<MenuDate> ChildNode { get; set; } = new();
    }
}