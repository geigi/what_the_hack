using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.NotificationSystem;
using GameTime;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Assets.Tests_PlayMode
{
    public class NotificationViewTests
    {

        [SetUp]
        public void SetUp()
        {
            SceneManager.LoadScene("MainGame");
        }

        [UnityTest]
        public IEnumerator ShowDisplayedNotificationsTests()
        {
            var view = GetView();
            SetUpDisplayedNotification(5);
            view.DisplayedNotifications();
            view.Show();
            Assert.AreEqual(5, view.Content.transform.childCount);
            for (var i = 0; i < 5; i++)
            {
                Assert.IsTrue(view.Content.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                    ContentHub.Instance.FailNotificationSprite);
                Assert.IsTrue(view.Content.transform.GetChild(i).GetChild(2).GetComponent<Text>().text == "a");
            }
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShowSpecificNotificationsTest()
        {
            var view = GetView();
            view.SpecificNotifications(new List<Notification>()
            {
                new Notification("b", Notification.NotificationType.Fail, new GameDate()),
                new Notification("b", Notification.NotificationType.Fail, new GameDate()),
                new Notification("b", Notification.NotificationType.Fail, new GameDate())
            });
            view.Show();
            Assert.AreEqual(3, view.Content.transform.childCount);
            for (var i = 0; i < 3; i++)
            {
                Assert.IsTrue(view.Content.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                    ContentHub.Instance.FailNotificationSprite);
                Assert.IsTrue(view.Content.transform.GetChild(i).GetChild(2).GetComponent<Text>().text == "b");
            }
            yield return null;
        }

        private void SetUpDisplayedNotification(int num)
        {
            for(var i = 0; i < num; i++)
                NotificationManager.Instance.GetData().Notifications.Add(new Notification("a", Notification.NotificationType.Fail, new GameDate()){Displayed = true});
        }

        private NotificationView GetView()
        {
            var parent = GameObject.FindWithTag("Notification");
            for (var i = 0; i < parent.transform.childCount; i++)
            {
                if (parent.transform.GetChild(i).name == "NotificationCenter")
                {
                    parent.transform.GetChild(i).gameObject.SetActive(true);
                    return parent.transform.GetChild(i).transform.GetChild(0).GetComponent<NotificationView>();
                }
            }

            return null;
        }
    }
}
