namespace Gamebook.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<MsSqlDbContext>
    {
        const string AdministratorUserName = "ivo@ivo.bg";
        const string AdministratorUserPassword = "ivoivo";
        const string RegularUserName = "user@user.bg";
        const string RegularUserPassword = "useruser";

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
            this.AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(MsSqlDbContext context)
        {
            this.SeedUsers(context);
            this.SeedSampleBooks(context);
            this.SeedSamplePages(context);

            base.Seed(context);
        }

        private void SeedUsers(MsSqlDbContext context)
        {
            const string adminRoleName = "Admin";
            const string regularRoleName = "Regular";

            if (!context.Roles.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var roleAdmin = new IdentityRole { Name = adminRoleName };
                roleManager.Create(roleAdmin);
                var roleRegular = new IdentityRole { Name = regularRoleName };
                roleManager.Create(roleRegular);

                var userStore = new UserStore<User>(context);
                var userManager = new UserManager<User>(userStore);

                var userAdmin = new User
                {
                    UserName = AdministratorUserName,
                    Email = AdministratorUserName,
                    EmailConfirmed = true,
                    CreatedOn = DateTime.Now
                };
                userManager.Create(userAdmin, AdministratorUserPassword);
                userManager.AddToRole(userAdmin.Id, adminRoleName);

                var userRegular = new User
                {
                    UserName = RegularUserName,
                    Email = RegularUserName,
                    EmailConfirmed = true,
                    CreatedOn = DateTime.Now
                };
                userManager.Create(userRegular, RegularUserPassword);
                userManager.AddToRole(userRegular.Id, regularRoleName);
            }
        }

        private void SeedSampleBooks(MsSqlDbContext context)
        {
            if (!context.Books.Any())
            {
                for (int i = 1; i < 51; i++)
                {
                    var book = new Book()
                    {
                        CatalogueNumber = i,
                        Title = "Book " + i,
                        Resume = "Book " + i + " Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        Author = context.Users.First(x => x.Email == AdministratorUserName),
                        CreatedOn = DateTime.Now
                    };

                    context.Books.Add(book);
                }

                context.SaveChanges();
            }
        }

        private void SeedSamplePages(MsSqlDbContext context)
        {
            if (!context.Pages.Any())
            {
                for (int i = 1; i < 51; i++)
                {
                    var page = new Page()
                    {
                        Book = context.Books.First(x => x.CatalogueNumber == 1),
                        Number = i,
                        Text = "Page " + i + " Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                        Author = context.Users.First(x => x.Email == AdministratorUserName),
                        CreatedOn = DateTime.Now
                    };

                    context.Pages.Add(page);

                    var pageConnection = new PageConnection()
                    {
                        Book = context.Books.First(x => x.CatalogueNumber == 1),
                        ParentPageNumber = 1,
                        Text = "Go to ",
                        ChildPageNumber = i,
                        Author = context.Users.First(x => x.Email == AdministratorUserName),
                        CreatedOn = DateTime.Now
                    };

                    context.PageConnections.Add(pageConnection);
                }
            }
        }
    }
}
