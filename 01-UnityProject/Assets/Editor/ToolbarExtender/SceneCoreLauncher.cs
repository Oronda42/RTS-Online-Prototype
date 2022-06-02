using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace UnityToolbarExtender.Examples
{
    static class ToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyle;

        static ToolbarStyles()
        {
            commandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold,
                fixedWidth = 50,
            };
        }

    }

    [InitializeOnLoad]
    public static class SceneCoreLauncher
    {

        static SceneCoreLauncher()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            EditorApplication.pauseStateChanged += OnPauseChanged;

            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnPauseChanged(PauseState obj)
        {

        }

        static void OnPlayModeChanged(PlayModeStateChange obj)
        {
            //Entered Edit Mode
            if(obj.ToString() == "EnteredEditMode" && EditorPrefs.GetString("CurrentOpenedScene","") != "")
            {
                Debug.Log("Restore previous scene : " + EditorPrefs.GetString("CurrentOpenedScene", ""));
                EditorSceneManager.OpenScene(EditorPrefs.GetString("CurrentOpenedScene", ""));
                EditorPrefs.DeleteKey("CurrentOpenedScene");
            }
        }

        static void OnToolbarGUI()
        {
            //Browse icons : https://unitylist.com/p/5c3/Unity-editor-icons
            var tex = EditorGUIUtility.IconContent(@"Collab.BuildSucceeded").image;

            //Start Server
            if (GUILayout.Button(EditorGUIUtility.IconContent(@"Collab.BuildSucceeded").image, ToolbarStyles.commandButtonStyle))
            {
                ServerHelperEditor.StartServer();
            }

            //Stop Server
            if (GUILayout.Button(EditorGUIUtility.IconContent(@"Collab.BuildFailed").image, ToolbarStyles.commandButtonStyle))
            {
                ServerHelperEditor.StopServer();
            }

            //Start Scene
            if (GUILayout.Button(EditorGUIUtility.IconContent(@"d_StepButton On").image, ToolbarStyles.commandButtonStyle))
            {
                if (EditorSceneManager.GetActiveScene().name != Constants.Scenes.Core.APPLICATION_MANAGER)
                {
                    //Save scene
                    EditorPrefs.SetString("CurrentOpenedScene", EditorSceneManager.GetActiveScene().path);

                    //Save reference scene in editor prefs
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

                    //Load scene
                    EditorSceneManager.OpenScene("Assets/Scenes/Core/"+Constants.Scenes.Core.APPLICATION_MANAGER+".unity");
                    EditorApplication.EnterPlaymode();
                }
                else
                {
                    EditorApplication.EnterPlaymode();
                }
            }

            //Stop Server
            if (GUILayout.Button(EditorGUIUtility.IconContent(@"SortingGroup Icon").image, ToolbarStyles.commandButtonStyle))
            {
                BundleBuilder.BuildAllAssetBundles();
            }
        }

    }
}