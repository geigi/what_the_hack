using NUnit.Framework;
using UnityEditor.SceneManagement;

namespace Assets.Tests
{
    class EmojiBubbleFactoryTests : EmojiBubbleFactory
    {

        private EmojiBubbleFactory factory;

        [SetUp]
        public void SetUp()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/MainGame.unity");
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
