using Wexxle.Guide.Data;
using PipServices3.Commons.Data;
using System.Threading.Tasks;
using PipServices3.Commons.Random;
using Xunit;

namespace Wexxle.Guide.Persistence
{
    public class GuidesPersistenceFixture
    {
		private readonly IGuidesPersistence _persistence;

        public GuidesPersistenceFixture(IGuidesPersistence persistence)
        {
			_persistence = persistence;
        }
        public async Task It_Should_Create_Guide()
        {
	        // arrange 
	        var guide = TestModel.CreateGuide();

	        // act
	        var result = await _persistence.CreateAsync(null, guide);

	        // assert
	        TestModel.AssertEqual(guide, result);
        }

        public async Task It_Should_Update_Guide()
        {
	        // arrange
	        var guide1 = await _persistence.CreateAsync(null, TestModel.CreateGuide());

	        // act
	        guide1.Name = RandomText.Word();
	        var result = await _persistence.UpdateAsync(null, guide1);

	        // assert
	        Assert.NotNull(result);
	        TestModel.AssertEqual(guide1, result);
        }

        public async Task It_Should_Delete_Guide()
        {
	        // arrange 
	        var guide = await _persistence.CreateAsync(null, TestModel.CreateGuide());

	        // act
	        var deletedGuide = await _persistence.DeleteByIdAsync(null, guide.Id);
	        var result = await _persistence.GetByIdAsync(null, guide.Id);

	        // assert
	        TestModel.AssertEqual(guide, deletedGuide);
	        Assert.Null(result);
        }

        public async Task It_Should_Get_Guide_By_Id()
        {
	        // arrange 
	        var guide = await _persistence.CreateAsync(null, TestModel.CreateGuide());

	        // act
	        var result = await _persistence.GetByIdAsync(null, guide.Id);

	        // assert
	        TestModel.AssertEqual(guide, result);
        }

        public async Task It_Should_Get_All_Guides()
        {
	        // arrange 
	        await _persistence.CreateAsync(null, TestModel.CreateGuide());
	        await _persistence.CreateAsync(null, TestModel.CreateGuide());

	        // act
	        var page = await _persistence.GetPageByFilterAsync(
		        null,
		        new FilterParams(),
		        new PagingParams()
	        );

	        // assert
	        Assert.NotNull(page);
	        Assert.Equal(2, page.Data.Count);
        }

		public async Task It_Should_Get_Guides_By_Filters()
        {
			// Create items
			var guide1 = await _persistence.CreateAsync(null, TestModel.CreateGuide());
			var guide2 = await _persistence.CreateAsync(null, TestModel.CreateGuide());
			var guide3 = await _persistence.CreateAsync(null, TestModel.CreateGuide());


			// Filter by id
			var page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "id", guide1.Id
                ),
                new PagingParams()
            );

            Assert.Single(page.Data);

            // Filter by name
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "name", guide2.Name
				),
                new PagingParams()
            );

            Assert.Single(page.Data);

            // Filter by tag
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "tag",  "tag1"
                ),
                new PagingParams()
            );

            Assert.Equal(3, page.Data.Count);

			// Filter by tag
			page = await _persistence.GetPageByFilterAsync(
				null,
				FilterParams.FromTuples(
					"tag", guide3.Tags[1]
				),
				new PagingParams()
			);

			Assert.Single(page.Data);

			int expTypes = 1;
			if (guide3.Type == guide1.Type) expTypes++;
			if (guide3.Type == guide2.Type) expTypes++;
			// Filter by type
			page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "type", guide3.Type
				),
                new PagingParams()
            );

            Assert.Equal(expTypes, page.Data.Count);

			// Filter by app
			page = await _persistence.GetPageByFilterAsync(
				null,
				FilterParams.FromTuples(
					"app", guide1.App
				),
				new PagingParams()
			);

			Assert.Single(page.Data);

			// Filter by ids
			page = await _persistence.GetPageByFilterAsync(
				null,
				FilterParams.FromTuples(
					"ids", guide1.Id+","+guide2.Id+","+ "2,3"
				),
				new PagingParams()
			);

			Assert.Equal(2, page.Data.Count);

			// Filter combine
			page = await _persistence.GetPageByFilterAsync(
				null,
				FilterParams.FromTuples(
					"ids", guide1.Id + "," + guide2.Id + "," + "2,3"
				,	"tag", guide2.Tags[1]
				),

				new PagingParams()
			);

			Assert.Single(page.Data);


		}
	}
}
