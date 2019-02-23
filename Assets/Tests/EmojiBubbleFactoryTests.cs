using NUnit.Framework;

namespace Assets.Tests
{
    class EmojiBubbleFactoryTests : EmojiBubbleFactory
    {

        private EmojiBubbleFactory factory;

        [SetUp]
        public void SetUp()
        {
            factory = EmojiBubbleFactory.Instance;
            factory.contentHub = ContentHub.Instance;
        }

        [Test]
        public void GetSpriteFromEmojiTest()
        {
            Assert.NotNull(factory.GetSpriteFromEmojiType(EmojiBubbleFactory.EmojiType.ANGRY));
        }
    }
}
