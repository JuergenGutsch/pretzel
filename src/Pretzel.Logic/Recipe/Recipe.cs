﻿using System;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Abstractions;
using Pretzel.Logic.Extensions;

namespace Pretzel.Logic
{
    public class Recipe
    {
        public Recipe(IFileSystem fileSystem, string engine, string directory, bool withProject)
        {
            this.fileSystem = fileSystem;
            this.engine = engine;
            this.directory = directory;
            this.withProject = withProject;
        }

        private readonly IFileSystem fileSystem;
        private readonly string engine;
        private readonly string directory;
        private readonly bool withProject;

        public void Create()
        {
            try
            {
                if (!fileSystem.Directory.Exists(directory))
                    fileSystem.Directory.CreateDirectory(directory);

                if (string.Equals("razor", engine, StringComparison.InvariantCultureIgnoreCase))
                {
                    CreateDirectories();

                    fileSystem.File.WriteAllText(Path.Combine(directory, @"rss.xml"), Properties.Razor.Rss);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"atom.xml"), Properties.Razor.Atom);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"_layouts\layout.cshtml"), Properties.Razor.Layout);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"_layouts\post.cshtml"), Properties.Razor.Post);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"index.md"), Properties.Razor.Index);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"about.md"), Properties.Razor.About);
                    fileSystem.File.WriteAllText(Path.Combine(directory, string.Format(@"_posts\{0}-myfirstpost.md", DateTime.Today.ToString("yyyy-MM-dd"))), Properties.Razor.FirstPost);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"css\style.css"), Properties.Resources.Style);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"_config.yml"), Properties.Razor.Config);

                    CreateImages();

                    if (withProject)
                        CreateProject();

                    Tracing.Info("Pretzel site template has been created");
                }
                else if (string.Equals("liquid", engine, StringComparison.InvariantCultureIgnoreCase))
                {
                    CreateDirectories();

                    fileSystem.File.WriteAllText(Path.Combine(directory, @"rss.xml"), Properties.Liquid.Rss);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"atom.xml"), Properties.Liquid.Atom);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"_layouts\layout.html"), Properties.Liquid.Layout);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"_layouts\post.html"), Properties.Liquid.Post);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"index.md"), Properties.Liquid.Index);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"about.md"), Properties.Liquid.About);
                    fileSystem.File.WriteAllText(Path.Combine(directory, string.Format(@"_posts\{0}-myfirstpost.md", DateTime.Today.ToString("yyyy-MM-dd"))), Properties.Liquid.FirstPost);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"css\style.css"), Properties.Resources.Style);
                    fileSystem.File.WriteAllText(Path.Combine(directory, @"_config.yml"), Properties.Liquid.Config);

                    CreateImages();

                    Tracing.Info("Pretzel site template has been created");
                }
                else
                {
                    Tracing.Info("Templating Engine not found");
                }
            }
            catch (Exception ex)
            {
                Tracing.Error(string.Format("Error trying to create template: {0}", ex));
            }
        }

        private void CreateProject()
        {
            var layoutDirectory = Path.Combine(directory, "_layouts");
            fileSystem.Directory.CreateDirectory(Path.Combine(layoutDirectory, @"Properties"));
            fileSystem.Directory.CreateDirectory(Path.Combine(layoutDirectory, @"PretzelClasses"));
            fileSystem.Directory.CreateDirectory(Path.Combine(layoutDirectory, @".nuget"));

            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @"Properties\AssemblyInfo.cs"), Properties.RazorCsProject.AssemblyInfo_cs);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @"PretzelClasses\Category.cs"), Properties.RazorCsProject.Category_cs);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @"LayoutProject.csproj"), Properties.RazorCsProject.LayoutProject_csproj);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @"layoutSolution.sln"), Properties.RazorCsProject.LayoutSolution_sln);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @"PretzelClasses\NonProcessedPage.cs"), Properties.RazorCsProject.NonProcessedPage_cs);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @".nuget\NuGet.config"), Properties.RazorCsProject.NuGet_Config);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @".nuget\NuGet.exe"), Properties.RazorCsProject.NuGet_exe);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @".nuget\NuGet.targets"), Properties.RazorCsProject.NuGet_targets);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @"PretzelClasses\PageContext.cs"), Properties.RazorCsProject.PageContext_cs);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @"PretzelClasses\Page.cs"), Properties.RazorCsProject.Page_cs);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @"PretzelClasses\Paginator.cs"), Properties.RazorCsProject.Paginator_cs);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @"PretzelClasses\SiteContext.cs"), Properties.RazorCsProject.SiteContext_cs);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @"PretzelClasses\Tag.cs"), Properties.RazorCsProject.Tag_cs);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @"web.config"), Properties.RazorCsProject.Web_config);
            fileSystem.File.WriteAllBytes(Path.Combine(layoutDirectory, @"packages.config"),
                                          Properties.RazorCsProject.packages_config);
        }

        private void CreateImages()
        {
            var ms = new MemoryStream();
            Properties.Resources._25.Save(ms, ImageFormat.Png);
            fileSystem.File.WriteAllBytes(Path.Combine(directory, @"img\25.png"), ms.ToArray());

            ms = new MemoryStream();
            Properties.Resources.faviconpng.Save(ms, ImageFormat.Png);
            fileSystem.File.WriteAllBytes(Path.Combine(directory, @"img\favicon.png"), ms.ToArray());

            ms = new MemoryStream();
            Properties.Resources.logo.Save(ms, ImageFormat.Png);
            fileSystem.File.WriteAllBytes(Path.Combine(directory, @"img\logo.png"), ms.ToArray());

            ms = new MemoryStream();
            Properties.Resources.faviconico.Save(ms);
            fileSystem.File.WriteAllBytes(Path.Combine(directory, @"img\favicon.ico"), ms.ToArray());
        }

        private void CreateDirectories()
        {
            fileSystem.Directory.CreateDirectory(Path.Combine(directory, @"_posts"));
            fileSystem.Directory.CreateDirectory(Path.Combine(directory, @"_layouts"));
            fileSystem.Directory.CreateDirectory(Path.Combine(directory, @"css"));
            fileSystem.Directory.CreateDirectory(Path.Combine(directory, @"img"));
        }
    }
}
