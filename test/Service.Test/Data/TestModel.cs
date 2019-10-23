using Wexxle.Guide.Data.Version1;
using System;
using System.Collections.Generic;
using PipServices3.Commons.Data;
using PipServices3.Commons.Random;
using Xunit;

namespace Wexxle.Guide.Data
{
	public static class TestModel
	{
		public static GuideV1 CreateGuide()
		{
			return new GuideV1
			{
				Id = IdGenerator.NextLong(),
				Name = RandomText.Name(),
				Type = RandomGuideType(),
				App = RandomText.Phrase(1, 50),
				MinVer = RandomLong.NextLong(int.MaxValue),
				MaxVer = RandomLong.NextLong(int.MaxValue),
				Tags = new List<string>
				{
					RandomText.Stuff(), IdGenerator.NextLong() ,RandomText.Color(), RandomText.Name(), RandomText.Phone(),RandomText.Adjective(),"tag1"
				},
				AllTags = new List<string>
				{
					RandomText.Stuff(), RandomText.Color(), RandomText.Name(), RandomText.Phone(),RandomText.Adjective()
				},
				Status = RandomText.Verb(),
				CreateTime = DateTime.UtcNow,
				Pages = new List<GuidePageV1>
				{
					new GuidePageV1
					{
						Title = new Dictionary<string, string>
						{
							{IdGenerator.NextShort(), RandomText.Word()}, {IdGenerator.NextShort(), RandomText.Word()}
						},
						Content = new Dictionary<string, string>
						{
							{IdGenerator.NextShort(), RandomText.Word()}, {IdGenerator.NextShort(), RandomText.Word()}
						},
						Color = RandomText.Color(),
						PicId = IdGenerator.NextShort(),
						PicUri = IdGenerator.NextLong()
					},
					new GuidePageV1
					{
						Title = new Dictionary<string, string>
						{
							{IdGenerator.NextShort(), RandomText.Word()}, {IdGenerator.NextShort(), RandomText.Word()}
						},
						Content = new Dictionary<string, string>
						{
							{IdGenerator.NextShort(), RandomText.Word()}, {IdGenerator.NextShort(), RandomText.Word()}
						},
						Color = RandomText.Color(),
						PicId = IdGenerator.NextShort(),
						PicUri = IdGenerator.NextLong()
					}

				}
			};
		}

		private static string RandomGuideType()
		{
			var num = RandomInteger.NextInteger(1, 3);
			switch (num)
			{
				case 1:
					return GuideTypeV1.Home;
				case 2:
					return GuideTypeV1.Introduction;
				case 3:
					return GuideTypeV1.NewRelease;
				default:
					return null;
			}
		}

		public static void AssertEqual(GuideV1 expectedGuide, GuideV1 actualGuide)
		{
			Assert.NotNull(actualGuide);
			Assert.Equal(expectedGuide.App, actualGuide.App);
			Assert.Equal(expectedGuide.MaxVer, actualGuide.MaxVer);
			Assert.Equal(expectedGuide.MinVer, actualGuide.MinVer);
			Assert.Equal(expectedGuide.Name, actualGuide.Name);
			Assert.Equal(expectedGuide.Status, actualGuide.Status);
			Assert.Equal(expectedGuide.Type, actualGuide.Type);
			Assert.NotNull(actualGuide.Pages);
			for (int i = 0; i < actualGuide.Pages.Count; i++)
				AssertPages(actualGuide.Pages[i], expectedGuide.Pages[i]);
			Assert.NotNull(actualGuide.Tags);
			for (int i = 0; i < actualGuide.Tags.Count; i++)
				Assert.Equal(expectedGuide.Tags[i], actualGuide.Tags[i]);
			Assert.NotNull(actualGuide.AllTags);
			for (int i = 0; i < actualGuide.AllTags.Count; i++)
				Assert.Equal(expectedGuide.AllTags[i], actualGuide.AllTags[i]);

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
