using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.NotificationSystem;
using GameTime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Assets.Tests_PlayMode
{
    public class SingleNotificationTests : MonoBehaviour
    {
        [SetUp]
        public void SetUp()
        {
            SceneManager.LoadScene("MainGame");
        }

        [UnityTest]
        public IEnumerator NotificationBarUpTest()
        {   
            var obj = GetSingleNotificationObject();
            var notify = obj.GetComponent<SingleNotification>();
            notify.animationTime = 2f;
            notify.notifications.Enqueue(new Notification("", Notification.NotificationType.Fail, new GameDate()));
            notify.notificationBarRect = obj.transform.parent.GetComponent<RectTransform>().rect;
            notify.up();
            yield return new WaitForSeconds(0.5f);
            Assert.IsTrue(obj.transform.parent.GetComponent<RectTransform>().rect.height < notify.maxHeight);
            Assert.IsTrue(obj.transform.parent.GetComponent<RectTransform>().rect.height > 0);
            yield return new WaitForSeconds(2f);
            Assert.IsTrue(obj.transform.parent.GetComponent<RectTransform>().rect.height == notify.maxHeight);
        }

        [UnityTest]
        public IEnumerator NotificationBarDownTest()
        {
            var obj = GetSingleNotificationObject();
            var notify = obj.GetComponent<SingleNotification>();
            obj.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(100, notify.maxHeight);
            notify.animationTime = 2f;
            notify.notificationBarRect = obj.transform.parent.GetComponent<RectTransform>().rect;
            notify.down();
            yield return new WaitForSeconds(0.5f);
            Assert.IsTrue(obj.transform.parent.GetComponent<RectTransform>().rect.height < notify.maxHeight);
            Assert.IsTrue(obj.transform.parent.GetComponent<RectTransform>().rect.height > 0);
            yield return new WaitForSeconds(2f);
            Assert.IsTrue(obj.transform.parent.GetComponent<RectTransform>().rect.height == 0);
        }

        [UnityTest]
        public IEnumerator ShowNotificationQueueTest_OneNotification()
        {
            var obj = GetSingleNotificationObject();
            var notify = obj.GetComponent<SingleNotification>();
            notify.animationTime = 0.2f;
            notify.xSeconds = 1f;
            notify.SetNotification(new Notification("text", Notification.NotificationType.Fail, new GameDate()));
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(ContentHub.Instance.FailNotificationSprite, notify.icon.sprite);
            Assert.AreEqual("text", notify.text.text);
            yield return new WaitForSeconds(1.5f);
            Assert.IsNull(notify.icon.sprite);
            Assert.IsEmpty(notify.text.text);
        }

        [UnityTest]
        public IEnumerator ShowNotificationQueueTest_MultipleNotification()
        {
            var obj = GetSingleNotificationObject();
            var notify = obj.GetComponent<SingleNotification>();
            notify.animationTime = 0.2f;
            notify.xSeconds = 1f;
            notify.SetNotification(new Notification("text", Notification.NotificationType.Fail, new GameDate()));
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(ContentHub.Instance.FailNotificationSprite, notify.icon.sprite);
            Assert.AreEqual("text", notify.text.text);
            notify.SetNotification(new Notification("another text", Notification.NotificationType.Info, new GameDate()));
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(ContentHub.Instance.InfoNotificationSprite, notify.icon.sprite);
            Assert.AreEqual("another text", notify.text.text);
            yield return new WaitForSeconds(1.5f);
            Assert.IsNull(notify.icon.sprite);
            Assert.IsEmpty(notify.text.text);
        }

        private GameObject GetSingleNotificationObject()
        {
            var parent = GameObject.FindWithTag("Notification");
            return parent.transform.GetChild(1).GetChild(0).gameObject;
        }
    }
}
