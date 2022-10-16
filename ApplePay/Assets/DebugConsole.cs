using UnityEngine;

namespace Pay.Debug
{
    public class DebugConsole : MonoBehaviour
    {
        [SerializeField] private KeyCode ReturnKey;
        [SerializeField] private KeyCode ConsoleToggleKey;
        [HideInInspector] public bool IsActive;
        [SerializeField] private Entity defaultExecuter;
        public bool CloseAfterExecute;
        string input = "/";
        private Vector2 scrollPosition;
        [SerializeField] private System.Collections.Generic.List<string> ConsoleList = new System.Collections.Generic.List<string>();
        [SerializeField] private byte MaxConsoleSize;
        private void Update()
        {
            if(Input.GetKeyDown(ConsoleToggleKey)) ToggleConsole();
        }
        private void OnGUI()
        {
            if (Event.current.Equals (Event.KeyboardEvent (ReturnKey.ToString()))) OnReturn();
            if(IsActive)
            {
                float y = 20f;
                GUI.backgroundColor = Color.grey;
                GUI.Box(new Rect(0, y, Screen.width/4, Screen.height / 6), "");
                input = GUI.TextField(new Rect(0, y + 100f, Screen.width/4, 30f), input);
                
            }
        }
        private void AddConsoleMessage(string message)
        {
            ConsoleList.Add(message);
            if(ConsoleList.Count > MaxConsoleSize) ConsoleList.RemoveAt(0);
        }
        public void ToggleConsole()
        {
            input = "/";
            IsActive = !IsActive;
        }
        public void OnReturn()
        {
            string[] args = ConsoleCommandHandler.ParseCommand(input);
            if(args[0] == "/")
            {
                string[] arguments = new string[args.Length - 1];
                for(int i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = args[i + 1];
                }
                ConsoleCommandHandler.ExecuteCommand(defaultExecuter, arguments, out string output);
                AddConsoleMessage(output);
                if(CloseAfterExecute) ToggleConsole();
            }
            else
            {
                ConsoleList.Add(input);
            }
            input = "/";
        }
    }
}