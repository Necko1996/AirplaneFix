using ColossalFramework;
using ColossalFramework.UI;
using System;
using UnityEngine.SceneManagement;
using AirplaneFix.Log;

namespace AirplaneFix
{
    public class Prompt
    {
        public static void Info(string title, string message, bool bShowWarningIcon = true)
        {
            MessageBoxPanel(title, message, bShowWarningIcon);
        }
        public static void Warning(string title, string message)
        {
            ExceptionPanel(title, message, false);
        }

        public static void WarningFormat(string title, string messageFormat, params object[] args)
        {
            ExceptionPanel(title, string.Format(messageFormat, args), false);
        }

        public static void Error(string title, string message)
        {
            ExceptionPanel(title, message, true);
        }

        public static void ErrorFormat(string title, string messageFormat, params object[] args)
        {
            ExceptionPanel(title, string.Format(messageFormat, args), true);
        }

        internal static void ExceptionPanel(string title, string message, bool isError)
        {
            Action prompt = () => {
                UIView.library
                    .ShowModal<ExceptionPanel>("ExceptionPanel")
                    .SetMessage(title, message, isError);
            };

            try
            {
                if (SceneManager.GetActiveScene().name == "Game")
                {
                    Singleton<SimulationManager>.instance.m_ThreadingWrapper.QueueMainThread(prompt);
                }
                else
                {
                    prompt();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private static void MessageBoxPanel(string title, string message, bool bShowWarningIcon)
        {
            Action prompt = () => {

                MessageBoxPanel messageBoxPanel = UIView.library.ShowModal<MessageBoxPanel>("MessageBoxPanel");

                messageBoxPanel.Find<UILabel>("Message").text = message;
                messageBoxPanel.Find<UILabel>("Caption").text = title;

                // Hide warning icon if not needed.
                if (!bShowWarningIcon)
                {
                    UIComponent? icon = messageBoxPanel.Find<UIComponent>("WarningIcon");

                    if (icon != null)
                    {
                        icon.isVisible = false;
                    }
                }

                UIButton uiButton = messageBoxPanel.Find<UIButton>("Close");

                if (!uiButton)
                {
                    return;
                }

                uiButton.isVisible = false;
            };

            try
            {
                if (SceneManager.GetActiveScene().name == "Game")
                {
                    Singleton<SimulationManager>.instance.m_ThreadingWrapper.QueueMainThread(prompt);
                }
                else
                {
                    prompt();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}