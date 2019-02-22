using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
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
