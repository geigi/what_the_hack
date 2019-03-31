using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.NotificationSystem;
using GameTime;
using NSubstitute;
using NUnit.Framework;
using UE.Events;
using UnityEditor.SceneManagement;
using UnityEngine.Events;

namespace Assets.Tests
{
    public class NotificationManagerTests
    {

        private NotificationManager manager;
        private UnityAction<object> act;
        private GameTime.GameTime time;

        [SetUp]
        public void SetUp()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/MainGame.unity");
            manager = new NotificationManager();
            NetObjectEvent evt = new NetObjectEvent();
            act = Substitute.For<UnityAction<object>>();
            manager.NewNotification = evt;
            evt.AddListener(act);
            time = Substitute.ForPartsOf<GameTime.GameTime>();
            time.GetData().Returns(new GameTimeData() {Date = new GameDate() {DateTime = new DateTime(1, 1, 1)}});
            manager.Initialize();
            manager.time = time;
        }

        [Test]
        public void NewNotificationTest()
        {
            manager.newNotification("A Message", Notification.NotificationType.Fail);
            Assert.IsTrue(manager.GetData().Notifications.Exists(n => n.Message.Equals("A Message") 
                                                                         && n.Category == Notification.NotificationType.Fail
                                                                         && n.Date.DateTime == new DateTime(1, 1, 1)));
            Assert.AreEqual(1, manager.GetData().Notifications.Count);
            act.Received(1);
            time.Received(1);
        }
    }
}
