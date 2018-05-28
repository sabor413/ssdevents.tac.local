using Sitecore.FakeDb;
using Xunit;

namespace ssdevents.tac.test
{
	public class SitecoreFakeDbTest
	{
		[Fact]
		public void HowToCreateSimpleItem()
		{
			using (var db = new Db
			{  new DbItem("Home") { { "Title", "Welcome!" } }
			})
			{
				Sitecore.Data.Items.Item home = db.GetItem("/sitecore/content/home");
				Xunit.Assert.Equal("Welcome!", home["Title"]);
			}
		}

		[Fact]
		public void HowToCreateItemOnSpecificTemplate()
		{
			Sitecore.Data.ID templateId = Sitecore.Data.ID.NewID;

			using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
			{
				new DbTemplate("products", templateId)
				{
					"Name"
				},
				new DbItem("Apple") { TemplateID = templateId }
			})
			{
				Sitecore.Data.Items.Item item =
				  db.GetItem("/sitecore/content/apple");

				Xunit.Assert.Equal(templateId, item.TemplateID);
				Xunit.Assert.NotNull(item.Fields["Name"]);
			}
		}
	}
}
