namespace Social.Network.Tests.RepositoryTests
{
	using NUnit.Framework;
	using Repository.BinaryTree;

	[TestFixture]
	public class BinaryTreeTests
	{
		[Test]
		public void AddNewTreeWithNoChildren_Test()
		{
			// act
			dynamic obj = new { Name = "test" };

			var tree = new Tree<dynamic>(obj);

			// actual
			var actual = tree.Root.Value;

			// assert
			Assert.AreEqual(actual.Name, obj.Name);
			Assert.AreEqual(tree.Root.ChildrenCount, 0);
		}

		[Test]
		public void AddNewTreeWithChildren_Test()
		{
			// act
			dynamic obj1 = new { Name = "test" };
			dynamic obj2 = new { Name = "child" };

			var tree = new Tree<dynamic>(obj1, new Tree<dynamic>(obj2));

			// actual
			var actual = tree.Root.GetChild(0).Value;

			// assert
			Assert.AreEqual(actual.Name, obj2.Name);
			Assert.AreEqual(tree.Root.ChildrenCount, 1);
		}

		[Test]
		public void AddNewTreeWithThreeChildren_Test()
		{
			// act
			dynamic obj1 = new { Name = "test" };
			dynamic obj2 = new { Name = "child1" };
			dynamic obj3 = new { Name = "child2" };
			dynamic obj4 = new { Name = "child3" };

			var tree = new Tree<dynamic>(obj1, new Tree<dynamic>(obj2)
				 , new Tree<dynamic>(obj3)
				 , new Tree<dynamic>(obj4));

			// actual
			var actual = tree.Root.GetChild(0).Value;

			// assert
			Assert.AreEqual(actual.Name, obj2.Name);
			Assert.AreEqual(tree.Root.ChildrenCount, 3);
		}

		[Test]
		[ExpectedException]
		public void AddEmptyTree_Exception_Test()
		{
			dynamic obj1 = new { Name = "test" };
			var tree = new Tree<dynamic>(obj1);

			tree.Root.AddChild(null);
		}

		[Test]
		[ExpectedException]
		public void AddEmptyChildTree_Exception_Test()
		{
			var tree = new Tree<dynamic>(null);
		}
	}
}