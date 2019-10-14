using Guides.Data.Version1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Guides.Data
{
	public class TestModel
	{
		private GuidePageV1 GUIDEPAGE1 = new GuidePageV1
		{
			Title = new Dictionary<string, string>() { { "Page1Title1", "Some in title1" }, { "Page1Title2", "Some in title2" } },
			Content = new Dictionary<string, string>() { { "Page1Content1", "Some in content1" }, { "Page1Content2", "Some in content2" } },
			Color = "yellow",
			PicId = "1",
			PicUri = "000001"
		};

		private GuidePageV1 GUIDEPAGE2 = new GuidePageV1
		{
			Title = new Dictionary<string, string>() { { "Page2Title1", "Some in title1" }, { "Page2Title2", "Some in title2" } },
			Content = new Dictionary<string, string>() { { "Page2Content1", "Some in content1" }, { "Page2Content2", "Some in content2" } },
			Color = "blue",
			PicId = "2",
			PicUri = "000002"
		};

		private GuidePageV1 GUIDEPAGE3 = new GuidePageV1
		{
			Title = new Dictionary<string, string>() { { "Page3Title1", "Some in title1" }, { "Page3Title2", "Some in title2" } },
			Content = new Dictionary<string, string>() { { "Page3Content1", "Some in content1" }, { "Page3Content2", "Some in content2" } },
			Color = "red",
			PicId = "3",
			PicUri = "000003"
		};

		private GuidePageV1 GUIDEPAGE4 = new GuidePageV1
		{
			Title = new Dictionary<string, string>() { { "Page4Title1", "Some in title1" }, { "Page4Title2", "Some in title2" } },
			Content = new Dictionary<string, string>() { { "Page4Content1", "Some in content1" }, { "Page4Content2", "Some in content2" } },
			Color = "black",
			PicId = "4",
			PicUri = "000004"
		};

		private GuideV1 GUIDE1 = new GuideV1
		{
			Id = "1",
			Name = "TestGuide1",
			Type = GuideTypeV1.Home,
			App = "App1",
			Min_ver = 12,
			Max_ver = 50,
			Tags = new List<string> { "tag1", "tag2", "tag3" },
			AllTags = new List<string> { "tag1", "tag2", "tag3", "tag4", "tag5" },
			Status = "active",
			CreateTime = DateTime.UtcNow,
		};
		private GuideV1 GUIDE2 = new GuideV1
		{
			Id = "2",
			Name = "TestGuide2",
			Type = GuideTypeV1.Introduction,
			App = "App2",
			Min_ver = 5,
			Max_ver = 11,
			Tags = new List<string> { "tag1", "tag2", "tag3" },
			AllTags = new List<string> { "tag1", "tag2", "tag3", "tag4", "tag5" },
			Status = "new",
			CreateTime = DateTime.UtcNow,
		};
		private GuideV1 GUIDE3 = new GuideV1
		{
			Id = "3",
			Name = "TestGuide3",
			Type = GuideTypeV1.Introduction,
			App = "App3",
			Min_ver = 11,
			Max_ver = 14,
			Tags = new List<string> { "tag1", "tag2", "tag3" },
			AllTags = new List<string> { "tag1", "tag2", "tag3", "tag4", "tag5" },
			Status = "new",
			CreateTime = DateTime.UtcNow,
		};

		public TestModel()
		{
			GUIDE1.Pages = new List<GuidePageV1> { GUIDEPAGE1, GUIDEPAGE2 };

			GUIDE3.Pages = new List<GuidePageV1> { GUIDEPAGE3, GUIDEPAGE4 };

		}

		public async Task<GuideV1> TestCreateGuidesAsync(Func<string, GuideV1, Task<GuideV1>> CreateAsync, string param = null)
		{
			// Create the first guide
			var guide = await CreateAsync(param, GUIDE1);

			AssertGuides(GUIDE1, guide);
			AssertPages(GUIDEPAGE1, guide.Pages[0]);
			AssertPages(GUIDEPAGE2, guide.Pages[1]);

			// Create the second guide
			guide = await CreateAsync(param, GUIDE2);
			AssertGuides(GUIDE2, guide);

			// Create the third guide
			guide = await CreateAsync(param, GUIDE3);
			AssertGuides(GUIDE3, guide);
			AssertPages(GUIDEPAGE3, guide.Pages[0]);
			AssertPages(GUIDEPAGE4, guide.Pages[1]);

			return guide;

		}

		public static void AssertGuides(GuideV1 expectedGuide, GuideV1 actualGuide)
		{
			Assert.NotNull(actualGuide);
			Assert.Equal(expectedGuide.App, actualGuide.App);
			Assert.Equal(expectedGuide.Max_ver, actualGuide.Max_ver);
			Assert.Equal(expectedGuide.Min_ver, actualGuide.Min_ver);
			Assert.Equal(expectedGuide.Name, actualGuide.Name);
			Assert.Equal(expectedGuide.Status, actualGuide.Status);
			Assert.Equal(expectedGuide.Type, actualGuide.Type);

			if (expectedGuide.Pages == null)
				Assert.Null(actualGuide.Pages);
			else
				Assert.NotNull(actualGuide.Pages);
			Assert.NotNull(actualGuide.Tags);
			Assert.NotNull(actualGuide.AllTags);
		}

		public static void AssertPages(GuidePageV1 expectedPage, GuidePageV1 actualPage)
		{
			Assert.Equal(expectedPage.Color, actualPage.Color);
			Assert.Equal(expectedPage.MoreUrl, actualPage.MoreUrl);
			Assert.Equal(expectedPage.PicId, actualPage.PicId);
			Assert.Equal(expectedPage.PicUri, actualPage.PicUri);
			Assert.Equal(expectedPage.Title, actualPage.Title);
			foreach (var key in expectedPage.Content.Keys)
				Assert.Equal(expectedPage.Content[key], actualPage.Content[key]);
			foreach (var key in expectedPage.Title.Keys)
				Assert.Equal(expectedPage.Title[key], actualPage.Title[key]);
		}
	}
}
