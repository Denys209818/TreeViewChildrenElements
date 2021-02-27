using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeViewAddElement.Entities;

namespace TreeViewAddElement.Services
{
    public static class DbSeeder
    {
        public static void SeedAll(EFContext context) 
        {
            SeedCategories(context);
        }

        private static void SeedCategories(EFContext context) 
        {
            if (context.Categories.Count() == 0) 
            {
                string UrlSlug = "cars";
                CreateParent(context, "Cars", UrlSlug);

                CreateChildren(context, "Mercedes", UrlSlug, "mercedes-cars");
                CreateChildren(context, "Audi", UrlSlug, "bmw-cars");
                CreateChildren(context, "Chevrolet", UrlSlug, "chevrolet-cars");

                CreateChildren(context, "BMW x7", "bmw-cars", "bmw-x7");
                CreateChildren(context, "BMW x6", "bmw-cars", "bmw-x6");

                UrlSlug = "trucks";
                CreateParent(context, "Trucks", UrlSlug);
                CreateChildren(context, "Mercedes", UrlSlug, "mercedes-trucks");
                CreateChildren(context, "MAN", UrlSlug, "man-trucks");
            }
        }

        private static void CreateParent(EFContext context, string name, string urlSlug) 
        {
            context.Categories.Add(new TreeViewCategoryElement { 
                Name = name,
                UrlSlug = urlSlug,
                ParentId = null
            });
            context.SaveChanges();
        }

        private static void CreateChildren(EFContext context, string name, string parentUrlSlug, string urlSlug) 
        {
            TreeViewCategoryElement parent = context.Categories.FirstOrDefault(x => x.UrlSlug == parentUrlSlug);
            if (parent != null) 
            {
                context.Categories.Add(new TreeViewCategoryElement { 
                    Name = name,
                    UrlSlug = urlSlug,
                    Parent = parent
                });

                context.SaveChanges();
            }
        }
    }
}
